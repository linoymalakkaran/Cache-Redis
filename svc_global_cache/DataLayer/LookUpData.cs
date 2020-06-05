using Dapper;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using SVC_General.Utilities;
using System;
using System.Configuration;
using System.Linq;
using System.Transactions;

namespace SVC_General.DataLayer
{
	public class LookUpData
	{
		public dynamic GetCountryList()
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcdb"].ConnectionString))
				{
					using (var transactionScope = new TransactionScope())
					{
						JArray CountryList = new JArray();
						connection.Open();
						var contacts = connection.Query(@"SELECT * FROM MAIL_COUNTRY ORDER BY ID").ToList();
						transactionScope.Complete();
						contacts.ForEach(x =>
						{
							CountryList.Add(new JObject() {
							{ "id", x.ID },
							{ "code",x.CODE }

						});
						});

						return CountryList.Count > 0 ? CountryList : null;
					}
				}
			}

			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				throw new Exception(ex.Message);
			}
		}
		public dynamic GetEmirateList()
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcdb"].ConnectionString))
				{
					using (var transactionScope = new TransactionScope())
					{
						JArray EmirateList = new JArray();
						connection.Open();
						var contacts = connection.Query(@"SELECT * FROM LOCAL_CITY ORDER BY ID").ToList();
						transactionScope.Complete();
						contacts.ForEach(x =>
						{
							EmirateList.Add(new JObject() {
							{ "id", x.ID },
							{ "code",x.CODE }

						});
						});

						return EmirateList.Count > 0 ? EmirateList : null;
					}
				}
			}

			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				throw new Exception(ex.Message);
			}
		}
	}
}