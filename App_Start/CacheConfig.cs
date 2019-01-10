using StackExchange.Redis;
using System;

namespace ElasticCacheSample
{
    public class CacheConfig
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = null;
        public static void CreateCacheClient()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { "localhost:6379" },
                    AbortOnConnectFail = false,
                };
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }

        public static ConnectionMultiplexer Get()
        {
            if (lazyConnection != null)
                return lazyConnection.Value;
            else
                return null;
        }
    }
}
