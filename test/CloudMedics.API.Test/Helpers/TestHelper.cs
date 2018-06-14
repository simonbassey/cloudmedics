using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
namespace CloudMedics.API.Test.Helpers
{
    public class TestHelper
    {
        public static IConfigurationRoot GetApplicationConfiguration(string settingsBasePath = null)
        {

            return new ConfigurationBuilder()
                .SetBasePath(settingsBasePath ?? GetSettingsFilePath())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        private static string GetSettingsFilePath()
        {
            string fullPath = typeof(TestHelper).GetTypeInfo().Assembly.Location;
            return string.Empty;
        }
    }
}
