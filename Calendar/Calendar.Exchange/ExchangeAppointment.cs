using System;
using Microsoft.Exchange.WebServices.Data;
using Core.Calendar;

namespace Calendar.Exchange
{
	public class ExchangeAppointment : AppointmentBase
	{
		public ExchangeAppointment (Appointment raw)
		{
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Subject", () => Title = raw.Subject);
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Start", () => StartDate = raw.Start.ToLocalTime ());
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("End", () => EndDate = raw.End.ToLocalTime ());
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("IsAllDayEvent", () => IsAllDayEvent = raw.IsAllDayEvent);
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("UniqueId", () => UID = raw.Id.UniqueId);
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Body", () => Body = raw.Body ?? "");
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Location", () => Location = raw.Location ?? "");
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Organizer", () => Organizer = raw.Organizer.Address ?? "");
		}

		public override string ToString ()
		{
			return string.Format ("[ExchangeAppointment: Title={0}, Organizer={1}, StartDate={2}, EndDate={3}, Body={4}, Location={5}, IsAllDayEvent={6}, UID={7}]", Title, Organizer, StartDate, EndDate, Body != null ? Body.Length : 0, Location, IsAllDayEvent, UID);
		}
	}
}

