using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Parsing;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seq.Input.Octopus
{
    public class Parsers
    {
        private static readonly MessageTemplateParser parser = new MessageTemplateParser();

        public LogEvent ParseSeqEvent(OctopusEvent octopusEvent)
        {
            var message = octopusEvent.Payload.Event.Message;

            var refs = octopusEvent.Payload.Event.MessageReferences
                .Select(r => new { Key = message.Substring(r.StartIndex, r.Length), Value = r.ReferencedDocumentId.Remove(r.ReferencedDocumentId.IndexOf("-")).TrimEnd('s') });

            var messageTemplate = parser.Parse(refs.Aggregate(message, (current, value) => current.Replace(value.Key, $"{{{value.Value}}}")));

            var properties = refs.Select(p => new LogEventProperty(p.Value, new ScalarValue(p.Key))).ToArray();

            var seqEvent = new LogEvent(octopusEvent.Timestamp, LogEventLevel.Information, null, messageTemplate, properties);

            // TODO: need a simple way to deal with duplicate webhooks
            //  See: https://octopus.com/blog/notifications-with-subscriptions-and-webhooks#process-an-event-only-once
            return seqEvent;
        }

        public async Task<OctopusEvent> ParseOctopusEvent(HttpRequest request)
        {
            request.EnableRewind();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            var octopusEvent = JsonConvert.DeserializeObject<OctopusEvent>(bodyAsText);
            return octopusEvent;
        }
    }
}
