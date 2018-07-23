using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ExpenseMgr.Data
{
    public interface IConfigSettingsReader
    {
        IConfiguration GetConfiguration();
    }
    public class StaticServiceResolver
    {
        public static StaticServiceResolver Instance { get; private set; }
        public IServiceProvider _serviceProvider { get; }

        private StaticServiceResolver(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public static StaticServiceResolver Create(IServiceProvider provider)
        {
            return Instance ?? new StaticServiceResolver(provider);
        }

        public static T Resolve<T>()
        {
            var configuration = Instance._serviceProvider.GetRequiredService<IConfiguration>();
            return (T)Instance._serviceProvider.GetService(typeof(T));
        }
    }
}
