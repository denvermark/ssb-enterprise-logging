using System;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.Extensions.Configuration;
using Ssb.Common.Logging.Utilities;

namespace Ssb.Common.Logging.Sinks.Serilog
{
    internal class SerilogConsoleSink
    {
        private const string DefaultTheme = "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console";
        private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}";
        private readonly string _theme;
        private readonly string _outputTemplate;

        public SerilogConsoleSink(AppSettingsSection consoleSettings)
        {
            if (consoleSettings != null)
            {
                var configTheme = consoleSettings.Settings["Theme"];
                _theme = configTheme?.Value ?? "";

                var configOutputTemplate = consoleSettings.Settings["OutputTemplate"];
                _outputTemplate = configOutputTemplate?.Value ?? "";
            }
        }

        public IConfiguration GetConfiguration()
        {
            var jsonObject = new
            {
                Serilog = new
                {
                    WriteTo = new[] {
                        new {
                            Name = "Console",
                            Args = new {
                                theme = !string.IsNullOrEmpty(_theme) ? _theme : DefaultTheme,
                                outputTemplate = !string.IsNullOrEmpty(_outputTemplate) ? _outputTemplate : DefaultOutputTemplate
                            }
                        }
                    }
                }
            };

            var jsonString = new JavaScriptSerializer().Serialize(jsonObject);
            var memoryJsonFile = new MemoryFileInfo("config.json", Encoding.UTF8.GetBytes(jsonString), DateTimeOffset.Now);
            var memoryFileProvider = new MockFileProvider(memoryJsonFile);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(memoryFileProvider, "config.json", false, false)
                .Build();

            return configuration;
        }
    }
}

