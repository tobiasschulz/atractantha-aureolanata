using TS.Common.Calendar;
using System.Collections.Generic;

namespace TS.Exchange.Calendar
{
	public class ExchangeCalendar : CalendarBase
	{
		private readonly List<ExchangeAppointment> appointments = new List<ExchangeAppointment> ();

		public override IEnumerable<AppointmentBase> Appointments { get { return appointments; } }

		public ExchangeCalendar ()
		{
		}

		public void Add (ExchangeAppointment appointment)
		{
			appointments.Add (appointment);
		}
	}
}

