using System;
using Microsoft.Exchange.WebServices.Data;
using System.IO;
using Core.Common;
using Calendar.Exchange;
using Core.Calendar;

namespace ExchangeSync
{
	public class ExchangeCalendarService
	{
		public ExchangeConfig Config { get; private set; }

		public ExchangeService Service { get { return Config.Service; } }

		private ExchangeCalendar calendar = new ExchangeCalendar ();

		public ExchangeCalendarService (ExchangeConfig config)
		{
			Config = config;
		}

		public void Fetch ()
		{
			// Initialize values for the start and end times, and the number of appointments to retrieve.
			DateTime startDate = DateTime.Now.AddDays (-360);
			DateTime endDate = DateTime.Now.AddDays (360);
			const int NUM_APPTS = 5000;

			// Initialize the calendar folder object with only the folder ID. 
			CalendarFolder calendarFolder = CalendarFolder.Bind (Service, WellKnownFolderName.Calendar, new PropertySet ());

			// Set the start and end time and number of appointments to retrieve.
			CalendarView view = new CalendarView (startDate, endDate, NUM_APPTS);

			// Retrieve a collection of appointments by using the calendar view.
			FindItemsResults<Appointment> appointments = calendarFolder.FindAppointments (view);

			// retrieve the properties we need
			PropertySet props = new PropertySet (ItemSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.IsAllDayEvent, ItemSchema.Body, AppointmentSchema.Location);

			Service.LoadPropertiesForItems (appointments, props);

			foreach (Appointment rawAppointment in appointments) {
				ExchangeAppointment appointment = new ExchangeAppointment (raw: rawAppointment);
				appointment.Organizer = Config.User;
				calendar.Add (appointment);
				Log.Debug (appointment);
			}
		}

		public void ExportICal (string fullPath)
		{
			string ical = ICalFormat.Export (calendar);
			File.WriteAllText (fullPath, ical);
		}
	}
}

