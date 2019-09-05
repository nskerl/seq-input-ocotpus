using Seq.Apps;
using Serilog;
using System.Collections.Generic;

namespace Seq.Input.Octopus.Tests.Support
{
    class TestAppHost : IAppHost
    {
        public App App { get; } = new App("test", "Test", new Dictionary<string, string>(), ".");
        public Host Host { get; } = new Host("https://seq.example.com", null);
        public ILogger Logger { get; } = new LoggerConfiguration().CreateLogger();
        public string StoragePath { get; } = ".";
    }
}
