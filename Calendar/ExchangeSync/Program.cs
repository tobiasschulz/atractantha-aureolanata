using Core.Common;

namespace ExchangeSync
{
	public class Program
	{
		static void Main (string[] args)
		{
			Logging.Enable ();
			Networking.DisableCertificateValidation ();
			ExchangeConfig config = new ExchangeConfig ();
			ExchangeCalendarService cal = new ExchangeCalendarService (config);
			cal.Fetch ();
			cal.ExportICal (fullPath: "output.ical");
		}
	}
}

