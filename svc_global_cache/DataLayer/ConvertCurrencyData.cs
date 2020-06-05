using Newtonsoft.Json.Linq;

namespace SVC_General.DataLayer
{
	public class ConvertCurrencyData
	{
		public JObject ConvertCurrency( string fromCurrencyCode,string toCurrencyCode,double amount)
		{
			JObject obj = new JObject();
			obj.Add("result", amount * 3.675);
			return obj;
		}
	}
}