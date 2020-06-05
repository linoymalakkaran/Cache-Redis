using StackExchange.Redis;
using System;
using System.Net;

namespace svc_global_cache.Utilities
{
	public class RedisConnectorHelper
	{
		static RedisConnectorHelper()
		{

            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
			{
				var configurationOptions = new ConfigurationOptions
				{
					AbortOnConnectFail = false,
					AllowAdmin = true
					
				};
				configurationOptions.EndPoints.Add(new DnsEndPoint("localhost",6379));
				return ConnectionMultiplexer.Connect(configurationOptions);
			});
		}

		private static Lazy<ConnectionMultiplexer> lazyConnection;
		public static ConnectionMultiplexer Connection
		{
			get
			{
				return lazyConnection.Value;
			}
		}
	}
}