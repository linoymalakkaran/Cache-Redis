using Dapper;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Linq;

namespace SVC_General.DataLayer
{
	public class TranslationData
	{
		public dynamic GetEntityTranslation(string langID, string entityType, string id)
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
				{
					JArray EntityTranslationList = new JArray();
					connection.Open();
					var contacts = connection.Query(@"SELECT * FROM ENTITY_DESCRIPTION "
				+ " WHERE ENTITY_TYPE = :entity_type AND LANG_ID = :lang_id AND ENTITY_REF = :entity_ref", new { entity_type = entityType, lang_id = langID, entity_ref = id }).ToList();
					contacts.ForEach(x =>
					{
						EntityTranslationList.Add(new JObject() {
							{ "type", x.ENTITY_TYPE },
							{ "id", x.ENTITY_REF },
							{ "name", x.DESCRIPTION },
							{ "length",x.LEN_TYPE }

						});
					});

					return EntityTranslationList.Count > 0 ? EntityTranslationList : null;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public dynamic GetEntityTranslation(string langID, string entityType)
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
				{
					JArray EntityTranslationMapList = new JArray();
					connection.Open();
					var contacts = connection.Query(@"SELECT * FROM ENTITY_DESCRIPTION "
				+ " WHERE ENTITY_TYPE = :entity_type AND LANG_ID = :lang_id", new { entity_type = entityType, lang_id = langID }).ToList();
					contacts.ForEach(x =>
					{
						EntityTranslationMapList.Add(new JObject() {
							{ "type", x.ENTITY_TYPE },
							{ "id", x.ENTITY_REF },
							{ "name", x.DESCRIPTION },
							{ "length",x.LEN_TYPE }

						});
					});
					return EntityTranslationMapList.Count > 0 ? EntityTranslationMapList : null;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public dynamic GetViewDescriptionMap(string langID, string contextType)
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
				{
					JObject ViewDescriptionMapList = new JObject();
					connection.Open();
					var contacts = connection.Query(@"SELECT key_name, key_value "
				+ " FROM VIEW_DESCRIPTION " + " WHERE LANG_ID = :lang_id "
				+ " AND CONTEXT_TYPE = :context_type " + " AND LEN_TYPE = 'L' ", new { lang_id = langID, context_type = contextType }).ToList();
					contacts.ForEach(x =>
					{
						ViewDescriptionMapList.Add(x.KEY_NAME, x.KEY_VALUE);
					});
					return ViewDescriptionMapList.Count > 0 ? ViewDescriptionMapList : null;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public void CreateEntity(string langID, string entityType, string lenType, string key, string value)
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
				{
					connection.Open();
					string processQuery = @"INSERT INTO ENTITY_DESCRIPTION
							(LANG_ID, ENTITY_TYPE, LEN_TYPE, ENTITY_REF, DESCRIPTION)
							VALUES('"+langID+"', '"+entityType+"', '"+lenType+"', '"+key+"', '"+value+"')";
					connection.Execute(processQuery);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}
		public void CreateUI(string langID, string entityType, string lenType, string key, string value, string channel)
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
				{
					connection.Open();
					CommandDefinition cmd = new CommandDefinition(@"Insert Into View_Description " +
				" (Lang_Id,Context_Type,Len_Type,Key_Name,Updated_By,Key_Value,Updated_On) " +
				"  Values " +
				" (:lang_id , :context_type ,:len_type ,:key_name, :updated_by, :key_value, SYSDATE) ", new { lang_id = langID, context_type = entityType, len_type = lenType, key_name = key, updated_by = channel, key_value = value });
					connection.Execute(cmd);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public void UpdateEntity(string langID, string entityType, string lenType, string key, string value)
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
				{
					connection.Open();
					CommandDefinition cmd = new CommandDefinition(@"UPDATE ENTITY_DESCRIPTION "
			+ " SET LEN_TYPE = '"+lenType+"', DESCRIPTION = '"+value+"' "
			+ " WHERE LANG_ID = '"+langID+"' AND ENTITY_TYPE = '"+entityType+"' AND ENTITY_REF = '"+key+"'");
					connection.Execute(cmd);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public void UpdateUI(string langID, string contextType, string lenType, string key, string value, string channel)
		{
			try
			{
				using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
				{
					connection.Open();
					CommandDefinition cmd = new CommandDefinition(@"UPDATE View_Description "
			+ " SET LEN_TYPE = :len_type, KEY_VALUE = :value, UPDATED_ON = SYSDATE "
			+ " WHERE LANG_ID = :lang_id AND CONTEXT_TYPE = :context_type AND KEY_NAME = :key_name AND Updated_By = :channel", new { len_type = lenType, value = value, lang_id = langID, context_type = contextType, key_name = key, channel = channel });
					connection.Execute(cmd);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}