using Core.Common;
using System;
using Core.Calendar.Google;
using System.Reflection;

namespace Calendar.University
{
	public class Program
	{
		static void Main (string[] args)
		{
			GoogleBindingRedirect.Apply ();

			Logging.Enable ();
			Networking.DisableCertificateValidation ();

			Assembly ass = Assembly.LoadFrom ("Google.Apis.Calendar.v3.dll");
			foreach (Type type in ass.GetTypes()) {
				Console.WriteLine (type.FullName);
			}

			try {
				UniConfig config = new UniConfig ();
				UniCalendar cal = new UniCalendar (config);

				GoogleCalendarService google = new GoogleCalendarService (config);
				google.Clear ();
				google.Sync (cal);
			} catch (Exception ex) {
				Log.FatalError (ex);
			}
		}
	}
}
