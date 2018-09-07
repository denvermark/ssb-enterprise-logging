using System;
using System.Collections.Generic;
using System.Globalization;
using Collector.Serilog.Sinks.AzureEventHub;
using Microsoft.ServiceBus.Messaging;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.EventGrid;
using Ssb.Common.Logging.Sinks.Serilog;

namespace Ssb.Common.Logging.Adapters
{
    public class SerilogAdapter : LoggerAdapterBase, ILoggerAdapter
    {
        private readonly Logger _logger;

        public SerilogAdapter(SerilogSinkFactory sinkFactory, Dictionary<string, Dictionary<string, string>> loggingConfigSettings)
        {
            var loggerConfig = new LoggerConfiguration();
            
            foreach (var sinkConfig in loggingConfigSettings)
            {
                var sink = sinkFactory.GetSink(sinkConfig);

                if (sink != null)
                {
                    loggerConfig.WriteTo.Sink(sink);
                }
                else
                {
                    var consoleSink = new SerilogConsoleSink(sinkConfig.Value);
                    loggerConfig.ReadFrom.Configuration(consoleSink.GetConfiguration());
                }
            }

            _logger = loggerConfig.CreateLogger();
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }

        public void Fatal(string message, Exception ex)
        {
            _logger.Fatal(ex, message);
        }

        public void Information(string message)
        {
            _logger.Information(message);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }
    }
}
