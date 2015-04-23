using Core.Common;
using TS.Google.Calendar;
using System;

namespace UniversityCalendar
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
				google.Sync (cal);
			} catch (Exception ex) {
				Log.FatalError (ex);
			}
		}
	}
}
