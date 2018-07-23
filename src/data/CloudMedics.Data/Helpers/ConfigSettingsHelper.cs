using System;
using Microsoft.Extensions.Configuration;
namespace CloudMedics.Data.Helpers
{
    public class ConfigSettingsHelper
    {
        public static ConfigSettingsHelper Instance { get; private set; }
        public IConfiguration _config { get; }

        private ConfigSettingsHelper()
        {
        }
        private ConfigSettingsHelper(IConfiguration config)
        {
            _config = config;
        }

        public static ConfigSettingsHelper Create(IConfiguration provider)
        {
            Instance = Instance ?? new ConfigSettingsHelper(provider);
            return Instance;
        }

        public static IConfiguration GetConfiguration()
        {
            return Instance._config;
        }
    }
}
