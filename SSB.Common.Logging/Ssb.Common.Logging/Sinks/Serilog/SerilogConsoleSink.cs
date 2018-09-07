using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.Extensions.Configuration;
using Ssb.Common.Logging.Utilities;

namespace Ssb.Common.Logging.Sinks.Serilog
{
    public class SerilogConsoleSink
    {
        private const string DefaultTheme = "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console";
        private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}";
        private readonly string _theme;
        private readonly string _outputTemplate;

        public SerilogConsoleSink(Dictionary<string, string> consoleSettings)
        {
            _theme = DefaultTheme;
            _outputTemplate = DefaultOutputTemplate;

            if (consoleSettings != null)
            {
                if (consoleSettings.ContainsKey("Theme")){
                    var theme = consoleSettings["Theme"];
                    _theme = !string.IsNullOrEmpty(theme) ? theme : DefaultTheme;
                }

                if (consoleSettings.ContainsKey("OutputTemplate"))
                {
                    var outputTemplate = consoleSettings["OutputTemplate"];
                    _outputTemplate = !string.IsNullOrEmpty(outputTemplate) ? outputTemplate : DefaultOutputTemplate;
                }
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

