using System;
using TS.Common.Calendar;
using System.Collections.Generic;

namespace UniversityCalendar
{
	public class UniCalendar : CalendarBase
	{
		private readonly List<UniAppointment> appointments = new List<UniAppointment> ();

		public override IEnumerable<AppointmentBase> Appointments { get { return appointments; } }

		public  UniCalendar ()
		{
		}

		public void Add (UniAppointment appointment)
		{
			appointments.Add (appointment);
		}
	}
}

