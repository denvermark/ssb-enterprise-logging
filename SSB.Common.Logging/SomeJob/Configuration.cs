using System;
using Autofac;
using Ssb.Common.Logging.Adapters;
using Ssb.Common.Logging.Sinks.Serilog;
using SSB.Common.Logging;

namespace SomeJob
{
    internal static class Configuration
    {
        public static IContainer Configure(string direction, string integration, string batchId, string instanceId)
        {
            var builder = new ContainerBuilder();

            // all classes that need the logger go here
            builder.RegisterType<SomeProcess>().As<ISomeProcess>();
            builder.RegisterType<SomeOtherProcess>().As<ISomeOtherProcess>();

            // register the logger itself
            builder.RegisterType<SsbLogger>().As<ISsbLogger>();

            // replace with whichever logging framework you'd like to use
            builder.RegisterType<SerilogAdapter>().As<ILoggerAdapter>()
                .WithParameter("loggingConfigSettings", LoggerAdapterBase.GetLoggingConfig());

            builder.RegisterType<SerilogSinkFactory>()
                .WithParameter("direction", direction)
                .WithParameter("integration", integration)
                .WithParameter("batchId", batchId)
                .WithParameter("instanceId", instanceId); 
 
            return builder.Build();
        }

    }
}
