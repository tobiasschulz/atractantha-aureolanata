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
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Start", () => StartDate = raw.Start);
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("End", () => EndDate = raw.End);
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("IsAllDayEvent", () => IsAllDayEvent = raw.IsAllDayEvent);
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("UniqueId", () => UID = raw.Id.UniqueId);
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Body", () => Body = raw.Body ?? "");
			ExchangeHelper.Catch<ServiceObjectPropertyException> ("Location", () => Location = raw.Location ?? "");
		}
	}
}

