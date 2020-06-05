using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using SVC_General.Models;
using SVC_General.ServiceReferenceAccess;

namespace SVC_General.DataLayer
{
	public class MessageData
	{		
		public MiddlewareServiceSEIClient _MiddlewareServiceSEIClient;
		public MessageData()
		{
			_MiddlewareServiceSEIClient = new MiddlewareServiceSEIClient();
		}
		public JObject SendEmail(string sender, string recepientList, string subject, string body)
		{
			using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
			{
				JObject resultMap = new JObject();
				string serviceXML = "<EMAIL_SEND> ' || '<Sender>{0}</Sender>' || '<Receiver>{1}</Receiver>' || '<Subject>{2}</Subject>' || '<Content><![CDATA[{3}]]></Content> ' || '</EMAIL_SEND>";
				serviceXML = string.Format(serviceXML, sender, recepientList, subject, body);
				try
				{

					resultMap.Add("detail", _MiddlewareServiceSEIClient.doService(GeneralConstants.MIDDLEWARE_USER_ID, GeneralConstants.MIDDLEWARE_PWD, serviceXML));
					resultMap.Add("status", "OK");
				}
				catch (Exception ex)
				{
					resultMap.Add("status", "ERROR");
					resultMap.Add("detail", ex.Message);
				}
				return resultMap;
			}
		}
		public JObject SendSMS(string sender, string recepientList, string message, int priority)
		{
			using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["esvcconfigdb"].ConnectionString))
			{
				JObject resultMap = new JObject();
				string serviceXML = "<SMS_SEND><Sender>{0}</Sender><Receiver>{1}</Receiver><Content>{2}</Content> <Priority>{3}</Priority></SMS_SEND>";
				serviceXML = string.Format(serviceXML, sender, recepientList, message,priority);
				try
				{
					resultMap.Add("detail", _MiddlewareServiceSEIClient.doService(GeneralConstants.MIDDLEWARE_USER_ID, GeneralConstants.MIDDLEWARE_PWD, serviceXML));
					resultMap.Add("status", "OK");
				}
				catch (Exception ex)
				{
					resultMap.Add("status", "ERROR");
					resultMap.Add("detail", ex.Message);
				}
				return resultMap;
			}
		}
	}
}