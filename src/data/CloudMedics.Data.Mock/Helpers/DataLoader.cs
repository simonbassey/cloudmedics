using System;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace CloudMedics.Data.Mock.Helpers
{
    public class DataLoader
    {
        public static async Task<string> LoadData(string resourceName)
        {
            try
            {
                var embeddedResources = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceNames();
                var resource = embeddedResources.
                                                FirstOrDefault(resourceName_ => resourceName_.Contains(resourceName));
                if (string.IsNullOrEmpty(resource))
                    throw new FileNotFoundException($"Could not find any embedded resource with name {resourceName}");
                using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    var data = await (new StreamReader(resourceStream).ReadToEndAsync());
                    return data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
