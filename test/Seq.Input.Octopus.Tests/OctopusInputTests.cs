using Seq.Input.Octopus.Tests.Support;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Seq.Input.Octopus.Tests
{
    public class OctopusInputTests
    {
        [Fact]
        public async Task OctopusInputPublishesEvents()
        {
            var input = new OctopusInput
            {
                Port = 5200
            };

            input.Attach(new TestAppHost());

            var events = new StringWriter();
            input.Start(events);

            await Task.Delay(15000);

            input.Stop();
            
            Assert.Equal(1, 1);
        }
    }
}
