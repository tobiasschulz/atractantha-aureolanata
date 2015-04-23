using Core.Common;
using TS.Google.Calendar;

namespace UniversityCalendar
{
	public class Program
	{
		static void Main (string[] args)
		{
			Logging.Enable ();
			Networking.DisableCertificateValidation ();

			UniConfig config = new UniConfig ();
			UniCalendar cal = new UniCalendar (config);

			GoogleCalendarService google = new GoogleCalendarService (config);
			google.Sync (cal);
		}
	}
}

