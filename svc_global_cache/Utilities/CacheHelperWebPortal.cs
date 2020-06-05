using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace svc_global_cache.Utilities
{
    public class CacheHelperWebPortal
    {

        public bool SaveToCacheEsvcPortal(string context_s, string key_s, string text_s, int expiryInSeconds_i)
        {
            string cachekey = context_s + key_s;
            string value = text_s;
            TimeSpan tvalue = new TimeSpan(0, 3, 0, 0, 0);
            if (expiryInSeconds_i != 0)
            {
                tvalue = new TimeSpan(0, 0, 0, expiryInSeconds_i, 0);
            }

            try
            {
                var cache = RedisConnectorHelper.Connection.GetDatabase();
                cache.StringSet(cachekey, value, tvalue);
                cache.KeyExpire(cachekey, tvalue);
                //Logger.Info($"Save: {cachekey}={value}");
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Error($"Save error: {cachekey}={ex.Message}");
                return false;
            }
        }

        public string ReadFromCacheEsvcPortal(string context_s,string key_s)
        {
            string cachekey = context_s + key_s;
            try
            {
                var cache = RedisConnectorHelper.Connection.GetDatabase();
                var value = cache.StringGet(cachekey);
                //Logger.Info($"Read: {cachekey}={value}");
                return value.ToString();
            }
            catch (Exception ex)
            {
                //Logger.Error($"Read error: {cachekey}={ex.Message}");
                return null;
            }
        }

        public string ClearCacheKeyForEsvcPortal(string context_s, string key_s)
        {
            string cachekey = context_s + key_s;
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
                //Logger.Info($"Clear: {cachekey}");
                return "OK";
            }
            catch (Exception ex)
            {
               // Logger.Error($"Clear error: {cachekey}={ex.Message}");
                return "ERROR";
            }
        }
    }
}