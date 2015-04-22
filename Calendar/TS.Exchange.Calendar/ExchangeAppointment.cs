using System;
using TS.Common.Calendar;
using Microsoft.Exchange.WebServices.Data;

namespace TS.Exchange.Calendar
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

