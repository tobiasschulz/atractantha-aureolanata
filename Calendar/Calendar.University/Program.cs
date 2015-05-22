using System;
using System.Linq;
using System.Reflection;
using Core.Calendar;
using Core.Calendar.Google;
using Core.Common;
using Core.IO;
using Core.Net;
using Core.Portable;
using Core.Platform;

namespace Calendar.University
{
	public class Program
	{
		static void Main (string[] args)
		{
			DesktopPlatform.Start ();
			Networking.DisableCertificateValidation ();

			Assembly ass = Assembly.LoadFrom ("Google.Apis.Calendar.v3.dll");
			foreach (Type type in ass.GetTypes()) {
				Console.WriteLine (type.FullName);
			}

			new Program ().Run ();

			DesktopPlatform.Finish ();
		}

		public void Run ()
		{
			try {
				UniConfig config = new UniConfig ();
				UniCalendar cal = new UniCalendar (config);

				GoogleCalendarService google = new GoogleCalendarService (config);
				//google.Clear ();

				Sync (source: cal, dest: google);
			} catch (Exception ex) {
				Log.FatalError (ex);
			}
		}

		public void Sync (ICalendar source, IEditableCalendar dest)
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
