using System;
using Core.Calendar.Google;
using Core.Common;
using Core.Google.Auth.Desktop;
using Core.IO;
using Microsoft.Exchange.WebServices.Data;

namespace Calendar.Exchange
{
	public class ExchangeConfig : IGoogleConfig
	{
		readonly Config config;

		public ExchangeService Service { get; private set; }

		public string Pass { get { return config.pass; } }

		public string User { get { return config.user; } }

		public string ExchangeUri { get { return config.exchange_uri; } }

		internal readonly string ClientId = StringHelper.Base64Decode ("MTUxMjM0ODM1NjE5LXA3b2ZwbTI3NXBqamlqZjVwOTcwbzk0ZWEzM2VubjE1LmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29t");
		internal readonly string ClientSecret = StringHelper.Base64Decode ("X21XdFpRMEJxS21zdFhjNnVVcTVzaHJR");

		public ExchangeConfig ()
		{
			config = ConfigHelper.OpenConfig<Config> (Storage.DefaultConfigFile);

			const string timeZoneName = "W. Europe Standard Time";
			TimeSpan timeZoneOffset = new TimeSpan (01, 00, 00);
			TimeZoneInfo timeZone;
			try {
				timeZone = TimeZoneInfo.FindSystemTimeZoneById (timeZoneName);
			} catch (TimeZoneNotFoundException) {
				timeZone = TimeZoneInfo.CreateCustomTimeZone (timeZoneName, timeZoneOffset, timeZoneName, timeZoneName);
			}

			ExchangeService service = new ExchangeService (ExchangeVersion.Exchange2007_SP1, timeZone);
			service.Credentials = new WebCredentials (config.user, config.pass);
			service.TraceEnabled = true;
			service.TraceListener = new LogTraceListener ();
			service.TraceFlags = TraceFlags.All;
			service.Url = new Uri (config.exchange_uri);

			System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
			Service = new ExchangeService (ExchangeVersion.Exchange2007_SP1, TimeZoneInfo.Utc);
			Service.Credentials = new WebCredentials (config.user, config.pass);
			//Service.TraceEnabled = true;
			Service.TraceFlags = TraceFlags.All;
			Service.Url = new Uri (config.exchange_uri);
		}


		#region IGoogleConfig implementation

		public string GoogleUser {
			get {
				return "tobiasschulz.digitalkraft@gmail.com";
			}
		}

		public IGoogleAuth Auth {
			get {
				return new GoogleWebAuth (clientId: ClientId, clientSecret: ClientSecret);
			}
		}

		public string CalendarName {
			get {
				return "exchange";
			}
		}

		#endregion

		private class Config
		{
			public string exchange_uri = "https://exchange.digitalkraft.de/EWS/Exchange.asmx";
			public string user = "";
			public string pass = "";
		}
	}
}

