﻿using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System.Threading;
using TS.Common.Calendar;
using System.Collections.Generic;
using System.Linq;
using Core.Common;

namespace TS.Google.Calendar
{
	public class GoogleCalendarService
	{
		private readonly string ClientId = StringHelper.Base64Decode ("MTUxMjM0ODM1NjE5LXA3b2ZwbTI3NXBqamlqZjVwOTcwbzk0ZWEzM2VubjE1LmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29t");
		private readonly string ClientSecret = StringHelper.Base64Decode ("X21XdFpRMEJxS21zdFhjNnVVcTVzaHJR");
		private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
		private const string ApplicationName = "AtractanthaAureolanata";

		private CalendarService service;
		private string calendarId;

		public GoogleCalendarService (IGoogleConfig config)
		{
			UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync (
				                            new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret },
				                            new[] { CalendarService.Scope.Calendar },
				                            config.GoogleUser, CancellationToken.None, 
				                            new FileDataStore ("Drive.Auth.Store"))
				.Result;

			service = new CalendarService (new BaseClientService.Initializer () {
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

			var calendars = service.CalendarList.List ().Execute ().Items;
			Log.Info ("List of google calendars:");
			Log.Indent++;
			foreach (CalendarListEntry entry in calendars) {
				Log.Info (entry.Summary + " - " + entry.Id);
				if (entry.Summary.ToLower ().Contains ("stundenplan")) {
					calendarId = entry.Id;
					Log.Indent++;
					Log.Info ("We'll use this one!");
					Log.Indent--;
				}
			}
			Log.Indent--;

			if (string.IsNullOrWhiteSpace (calendarId)) {
				throw new ArgumentException ("The google account has no calendar that matches the required name.");
			}
		}

		public void Sync (CalendarBase cal)
		{
			Events existingEvents = ListEvents ();
			IEnumerable<Event> allEvents = ConvertEvents (cal);

			IEnumerable<Event> newEvents = from e in allEvents
			                               where existingEvents.Items.All (ee => ee.Summary != e.Summary)
			                               select e; 
			AddEvents (newEvents);

			foreach (Event e in existingEvents.Items) {
				IEnumerable<Event> sameEvents = existingEvents.Items.Where (ee => ee.Summary == e.Summary && ee.Start.DateTimeRaw == e.Start.DateTimeRaw);
				Log.Debug ("e: ", e.Summary, "/", e.Start.DateTimeRaw, "/", string.Join (",", sameEvents));
			}

			foreach (Event e in allEvents) {
				IEnumerable<Event> sameEvents = existingEvents.Items.Where (ee => ee.Summary == e.Summary && ee.Start.DateTime == e.Start.DateTime);
				Log.Debug (e.Summary, "/", e.Start.DateTimeRaw, "/", string.Join (",", sameEvents));
				UpdateEvents (sameEvents, e);
			}
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
					Description = appointment.Body,
					Summary = appointment.Title,
					Location = appointment.Location,
				};
				yield return newEvent;
			}
		}

		void AddEvents (IEnumerable<Event> events)
		{
			foreach (Event e in events) {
				Log.Debug ("Add event: ", e.Start, e.Summary, e.Location);
				var request = service.Events.Insert (e, calendarId);
				request.Execute ();
			}
		}

		void UpdateEvents (IEnumerable<Event> events, Event update)
		{
			foreach (Event e in events) {
				Log.Debug ("Update event: ", e.Start, e.Summary, e.Location);
				e.Summary = update.Summary;
				e.Location = update.Location;
				e.End = update.End;
				e.Description = update.Description;
				var request = service.Events.Update (body: e, calendarId: calendarId, eventId: e.Id);
				request.Execute ();
			}
		}
	}
}

