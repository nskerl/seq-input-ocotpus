using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Seq.Apps;
using Serilog.Formatting.Compact;
using System;
using System.IO;
using System.Threading;

namespace Seq.Input.Octopus
{
    [SeqApp("Octopus webhook receiver", Description = "Receives Octopus webhooks as structured events.")]
    public class OctopusInput : SeqApp, IPublishJson, IDisposable
    {
        private readonly Parsers parsers = new Parsers();
        private readonly CompactJsonFormatter formatter = new CompactJsonFormatter();
        private readonly CancellationTokenSource cancel = new CancellationTokenSource();

        [SeqAppSetting(
            DisplayName = "Webhook port",
            IsOptional = true,
            HelpText = "The port on which to listen for webhook events (default: `5200`). " +
             "The URL to use in Octopus subscription will be of the form: `http:\\<seq host>:<webhook port>`")]
        public int Port { get; set; } = 5200;    

        public IWebHostBuilder CreateWebHostBuilder(TextWriter inputWriter) =>

           // Task.Run(() => HeartbeatAsync(inputWriter), _cancel.Token)

           WebHost.CreateDefaultBuilder()
                .UseUrls($"http://localhost:{Port}")
                .UseKestrel()
                .Configure(app =>
                {
                    app.Run(async (context) =>
                    {
                        try
                        {
                            var octopusEvent = await parsers.ParseOctopusEvent(context.Request);
                            if (octopusEvent?.Payload == null)
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            }
                            else
                            {
                                var seqEvent = parsers.ParseSeqEvent(octopusEvent);

                                formatter.Format(seqEvent, inputWriter);

                                context.Response.StatusCode = StatusCodes.Status202Accepted;
                            }
                        }
                        catch(Exception ex)
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync(ex.Message);
                        }                         
                    });
                });

        public void Start(TextWriter inputWriter)
        {
            CreateWebHostBuilder(inputWriter).Build().RunAsync(cancel.Token);
        }

        public void Stop()
        {
            cancel.Cancel();
        }

        public void Dispose()
        {
            
        }
    }
}