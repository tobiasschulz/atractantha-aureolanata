using Core.Common;
using System;
using Calendar.Google;

namespace Calendar.University
{
	public class Program
	{
		static void Main (string[] args)
		{
			Logging.Enable ();
			Networking.DisableCertificateValidation ();

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
