using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System.Threading;
using TS.Common.Calendar;
using System.Collections.Generic;
using System.Linq;

namespace TS.Google.Calendar
{
	public class GoogleCalendarService
	{
		private readonly string ClientId = StringHelper.Base64Decode ("MTUxMjM0ODM1NjE5LXA3b2ZwbTI3NXBqamlqZjVwOTcwbzk0ZWEzM2VubjE1LmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29t");
		private readonly string ClientSecret = StringHelper.Base64Decode ("X21XdFpRMEJxS21zdFhjNnVVcTVzaHJR");
		private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
		private const string ApplicationName = "Atractantha Aureolanata\n";

		private CalendarService service;
		private string calendarId;

		public GoogleCalendarService (IGoogleConfig config)
		{
			UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync (
				                            new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret },
				                            new[] { CalendarService.Scope.Calendar }, 
				                            "user", CancellationToken.None, 
				                            new FileDataStore ("Drive.Auth.Store"))
				.Result;

			service = new CalendarService (new BaseClientService.Initializer () {
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

			var calendars = service.CalendarList.List ().Execute ().Items;
			foreach (CalendarListEntry entry in calendars) {
				Log.Info (entry.Summary + " - " + entry.Id);
				if (entry.Summary.ToLower ().Contains ("stundenplan")) {
					calendarId = entry.Id;
					Log.Indent++;
					Log.Info ("We'll use this!");
					Log.Indent--;
				}
			}
		}

		public void Sync (CalendarBase cal)
		{
			Events existingEvents = ListEvents ();
			IEnumerable<Event> allEvents = ConvertEvents (cal);
			IEnumerable<Event> newEvents = from e in allEvents
			                               where existingEvents.Items.Any (ee => ee.Summary == e.Summary)
			                               select e; 
			//AddEvents (newEvents);
		}

		Events ListEvents ()
		{
			Events request = null;
			var lr = service.Events.List (calendarId);

			lr.TimeMin = DateTime.Now.AddDays (-360); //five days in the past
			lr.TimeMax = DateTime.Now.AddDays (360); //five days in the future

			request = lr.Execute ();
			return request;
		}

		IEnumerable<Event> ConvertEvents (CalendarBase cal)
		{
			foreach (AppointmentBase appointment in cal.Appointments) {
				var newEvent = new Event {
					Start = new EventDateTime { DateTime = appointment.StartDate },
					End = new EventDateTime { DateTime = appointment.EndDate },
					Description = "Description",
					Summary = appointment.Title,
				};
				yield return newEvent;
			}
		}

		void AddEvents (IEnumerable<Event> events)
		{
			foreach (Event e in events) {
				Log.Debug ("Add event: ", e.Summary);
				var request = service.Events.Insert (e, calendarId);
				request.Execute ();
			}
		}
	}
}

