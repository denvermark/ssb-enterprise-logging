using System;
using System.Collections.Generic;
using System.Configuration;

namespace Ssb.Common.Logging.Adapters
{
    public abstract class LoggerAdapterBase
    {
        //// Factory method
        //public delegate ILoggerAdapter Factory(string direction, string integration, string batchId, string instanceId);

        public static Dictionary<string, Dictionary<string, string>> GetLoggingConfig()
        {             // loop through config and add loggers
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSectionGroup appSettingsGroup = config.SectionGroups["applicationSettings"];
            if (appSettingsGroup == null)
            {
                throw new Exception();
            }

            ConfigurationSectionGroup loggingConfigSettings = appSettingsGroup.SectionGroups["ApiEtlQd.Logging.Settings"];
            if (loggingConfigSettings == null)
            {
                throw new Exception();
            }

            Dictionary<string, Dictionary<string, string>> loggingFrameworks = new Dictionary<string, Dictionary<string, string>>();
            foreach (ConfigurationSection section in loggingConfigSettings.Sections)
            {
                var settingsCollection = new Dictionary<string, string>();
                loggingFrameworks.Add(section.SectionInformation.Name, settingsCollection);

                foreach (KeyValueConfigurationElement nameVal in ((AppSettingsSection)section).Settings)
                {
                    settingsCollection.Add(nameVal.Key, nameVal.Value);
                }
            }

            return loggingFrameworks;
        }
    }
}
