using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CloudMedics.Data.Mock.Helpers
{
    public class DataReader
    {
        public static async Task<T> Read<T>(string filename)
        {
            var data = await DataLoader.LoadData(filename);
            T output = JsonConvert.DeserializeObject<T>(data);
            return output;
        }
    }
}
