using System;
using Microsoft.Exchange.WebServices.Data;
using Core.Common;

namespace ExchangeSync
{
	public class ExchangeConfig
	{
		readonly Config config;

		public ExchangeService Service { get; private set; }

		public string Pass { get { return config.pass; } }

		public string User { get { return config.user; } }

		public string ExchangeUri { get { return config.exchange_uri; } }

		public ExchangeConfig ()
		{
			config = ConfigHelper.OpenConfig<Config> (Storage.DefaultConfigFile);

			ExchangeService service = new ExchangeService (ExchangeVersion.Exchange2007_SP1, TimeZoneInfo.Utc);
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

		private class Config
		{
			public string exchange_uri = "https://exchange.digitalkraft.de/EWS/Exchange.asmx";
			public string user = "";
			public string pass = "";
		}
	}
}

