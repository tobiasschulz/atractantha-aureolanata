using System;
using System.Linq;
using Core.Calendar;
using Core.Calendar.Google;
using Core.Common;
using Core.IO;
using Core.Net;
using Core.Portable;
using Core.Platform;
using System.Collections.Generic;

namespace Calendar.Exchange
{
	public class Program
	{
		static void Main (string[] args)
		{
			DesktopPlatform.Start ();
			Networking.DisableCertificateValidation ();

			new Program ().Run ();

			DesktopPlatform.Finish ();
		}

		public void Run ()
		{
			try {
				ExchangeConfig config = new ExchangeConfig ();
				ExchangeCalendarService cal = new ExchangeCalendarService (config);
				cal.Fetch ();
				//cal.ExportICal (fullPath: "output.ical");

				GoogleCalendarService google = new GoogleCalendarService (config);
				//google.Clear ();

				OvertimeHours (source: cal);

				Sync (source: cal, dest: google);

			} catch (Exception ex) {
				Log.FatalError (ex);
			}
		}

		void OvertimeHours (ICalendar source)
		{
			var hoursPerMonth = new Dictionary<string,int> {
				["2015-02" ] = 45,
				["2015-03" ] = 60,
				["2015-04" ] = 60,
				["2015-05" ] = 60,
				["2015-06" ] = 60,
				["2015-07" ] = 60,
				["2015-08" ] = 160,
				["2015-09" ] = 160,
				["2015-10" ] = 60,
				["2015-11" ] = 60,
				["2015-12" ] = 60,
				["2016-01" ] = 60,
				["2016-02" ] = 60,
				["2016-03" ] = 60,
				["2016-04" ] = 60,
				["2016-05" ] = 60,
				["2016-06" ] = 60,
				["2016-07" ] = 60,
				["2016-08" ] = 60,
				["2016-09" ] = 60,
				["2016-10" ] = 60,
				["2016-11" ] = 60,
				["2016-12" ] = 60,
			};
			var hoursPerMonthSick = new Dictionary<string,int> {
				["2015-06" ] = 15,
			};

			AppointmentBase[] sourceEvents = source.Appointments.ToArray ();

			double ueberstunden = 0;

			Log.Info ("Stunden: ");
			Log.Indent++;
			foreach (string month in sourceEvents.Select(a => a.StartDate.ToString ("yyyy-MM")).Distinct().OrderBy(m => m)) {

				Log.Info ("Monat: ", month);
				Log.Indent++;

				double hoursSick = hoursPerMonthSick.ContainsKey (month) ? hoursPerMonthSick [month] : 0;

				double hoursIst = hoursSick;
				double hoursSoll = hoursPerMonth [month];

				foreach (string date in sourceEvents.Select(a => a.StartDate.ToString ("yyyy-MM-dd")).Distinct().OrderBy(d => d)) {
					if (!date.StartsWith (month))
						continue;

					DateTime start = default(DateTime);
					DateTime end = default(DateTime);

					foreach (AppointmentBase app in sourceEvents) {
						string _date = app.StartDate.ToString ("yyyy-MM-dd");

						if (date != _date)
							continue;

						//Log.Debug (_date);

						if (app.Title == "START")
							start = app.StartDate;
						if (app.Title == "ENDE")
							end = app.EndDate;
					}

					double hours = (end - start).TotalHours;
					if (hours < 0)
						hours = 0;
					hoursIst += hours - 1;

					Log.Info (string.Format ("Datum: {0} Stunden: {1}", date, hours));
				}

				double ueberstundenThisMonth = (hoursIst < hoursSoll) ? hoursIst - hoursSoll : hoursIst - hoursSoll - 5;

				Log.Info ("Stunden im Monat: ", hoursIst, " von ", hoursSoll, " (darin inklusive ", hoursSick, " Stunden krank)");
				Log.Info ("Überstunden aus Vormonat: ", ueberstunden);
				Log.Info ("Überstunden diesen Monat (minus 5): ", ueberstundenThisMonth);

				ueberstunden += ueberstundenThisMonth;

				Log.Indent--;
			}
			Log.Indent--;
		}

		void Sync (ICalendar source, IEditableCalendar dest)
		{
			AppointmentBase[] sourceEvents = source.Appointments.ToArray ();
			AppointmentBase[] destEvents = dest.Appointments.ToArray ();

			Log.Info ("source: ");
			Log.Indent++;
			foreach (AppointmentBase app in sourceEvents) {
				Log.Info (app);
			}
			Log.Indent--;

			Log.Info ("destination: ");
			Log.Indent++;
			foreach (IEditableAppointment app in destEvents) {
				Log.Info (app);
			}
			Log.Indent--;

			foreach (AppointmentBase app in sourceEvents.Except(destEvents)) {
				Log.Debug ("insert: ", app);

				dest.AddAppointment (app);
				Thread.Sleep (500);
			}

			foreach (IEditableAppointment app in destEvents.Except(sourceEvents)) {
				Log.Debug ("delete: ", app);

				app.Delete ();
				Thread.Sleep (500);
			}

			foreach (IEditableAppointment app in destEvents.GroupBy(s => s).SelectMany (grp => grp.Skip(1))) {
				Log.Debug ("delete: ", app);

				app.Delete ();
				Thread.Sleep (500);
			}

			foreach (IEditableAppointment destEvent in destEvents.Intersect(sourceEvents)) {
				AppointmentBase sourceEvent = sourceEvents.First (e => e.Equals (destEvent));
				//Log.Debug ("update: ", destEvent);

				//sourceEvent.CopyTo (destEvent);

				//destEvent.Update ();
				//PortableThread.Sleep (500);
			}

			/*
			GoogleAppointment existingEvents = ListEvents ();
			IEnumerable<Event> allEvents = ConvertEvents (cal);

			IEnumerable<Event> newEvents = from e in allEvents
			                               where existingEvents.Items.All (ee => ee.Start.DateTime != e.Start.DateTime)
			                               select e; 
			AddEvents (newEvents);

			foreach (Event e in allEvents) {
				IEnumerable<Event> sameEvents = existingEvents.Items.Where (ee => ee.Start.DateTime == e.Start.DateTime); // ee.Summary == e.Summary && 
				Log.Debug (e.Summary, "/", e.Start.DateTimeRaw, "/", string.Join (",", sameEvents));
				UpdateEvents (sameEvents, e);
			}*/
		}
	}
}

