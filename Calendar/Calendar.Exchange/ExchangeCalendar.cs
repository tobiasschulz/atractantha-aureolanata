using System.Collections.Generic;
using Core.Calendar;

namespace Calendar.Exchange
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

