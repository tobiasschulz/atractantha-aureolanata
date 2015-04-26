using System;
using System.Collections.Generic;
using Core.Calendar;

namespace Calendar.University
{
	public class UniAppointment : AppointmentBase
	{


		public override string ToString ()
		{
			return string.Format ("[UniAppointment: Title={0}, Organizer={1}, StartDate={2}, EndDate={3}, Body={4}, Location={5}, IsAllDayEvent={6}, UID={7}]", Title, Organizer, StartDate, EndDate, Body != null ? Body.Length : 0, Location, IsAllDayEvent, UID);
		}
	}
}

