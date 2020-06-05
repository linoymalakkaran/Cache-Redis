using StackExchange.Redis;
using svc_global_cache.Models;
using System;
using System.Net;

namespace svc_global_cache.Utilities
{
    public class CacheHelper
    {
        public string ReadFromCache(string cachekey)
        {
            try
            {
                var cache = RedisConnectorHelper.Connection.GetDatabase(); //RedisConnectorHelper.Connection.GetDatabase();
                var value = cache.StringGet(cachekey);
                return value;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ClearCacheDatabase()
        {
            try
            {
                var configurationOptions = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    AllowAdmin = true
                };
                configurationOptions.EndPoints.Add(new DnsEndPoint("localhost", 6379));
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configurationOptions);
                var server = redis.GetServer("localhost:6379");
                server.FlushDatabase();
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
        public string ClearCacheAllKeys()
        {
            try
            {
                var configurationOptions = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    AllowAdmin = true
                };
                configurationOptions.EndPoints.Add(new DnsEndPoint("localhost", 6379));
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configurationOptions);
                var server = redis.GetServer("localhost:6379");
                var cache = RedisConnectorHelper.Connection.GetDatabase();
                var keys = server.Keys();
                foreach (var key in keys)
                {
                    Console.WriteLine("Removing Key {0} from cache", key.ToString());
                    cache.KeyDelete(key);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
        public string ClearCacheKey(string cachekey)
        {
            try
            {
                var configurationOptions = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    AllowAdmin = true
                };
                configurationOptions.EndPoints.Add(new DnsEndPoint("localhost", 6379));
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configurationOptions);
                var server = redis.GetServer("localhost:6379");
                var cache = RedisConnectorHelper.Connection.GetDatabase();
                var keys = server.Keys();
                foreach (var key in keys)
                {
                    if (key == cachekey)
                    {
                        Console.WriteLine("Removing Key {0} from cache", key.ToString());
                        cache.KeyDelete(key);
                    }
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
        public string SaveToCache(CacheSaveModel model)
        {
            string cachekey = model.Context + model.Key;
            TimeSpan tvalue = new TimeSpan(1, 0, 0, 0, 0);
            if (model.ExpiryInSeconds != 0)
            {
                tvalue = new TimeSpan(0, 0, 0, model.ExpiryInSeconds, 0);
            }

            try
            {
                var cache = RedisConnectorHelper.Connection.GetDatabase();
                cache.StringSet(cachekey, model.Data, tvalue);
                cache.KeyExpire(cachekey, tvalue);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

    }
}