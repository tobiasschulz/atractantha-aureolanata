using System;
using TS.Common.Calendar;
using System.Collections.Generic;
using Core.Common;

namespace Calendar.University
{
	public class UniCalendar : CalendarBase
	{
		private readonly List<UniAppointment> appointments = new List<UniAppointment> ();

		public override IEnumerable<AppointmentBase> Appointments { get { return appointments; } }

		public UniCalendar (UniConfig config)
		{
			foreach (Semester semester in config.Config.Semesters) {
				foreach (Course course in semester.Courses) {
					// choose right day of week
					DateTime date;
					for (date = semester.Start; date.DayOfWeek != course.DayOfWeek; date += TimeSpan.FromDays (1)) {
					}
					// iterate over every week in the semester
					for (; date < semester.End; date += TimeSpan.FromDays (7)) {
						string courseType = course.CourseType == CourseType.Lecture ? "Vorlesung" : "Übung";
						UniAppointment appointment = new UniAppointment {
							Title = course.Subject.ShortName + " " + courseType + " " + course.Subject.Professor,
							StartDate = date + course.Timeslot.Start,
							EndDate = date + course.Timeslot.End,
							Body = "Subject: " + course.Subject.FullName,
							IsAllDayEvent = false,
							Location = ((course.Building ?? "") + "-" + (course.Room ?? "")).Trim ('-', ' '),
						};
						Log.Info (appointment.StartDate, ", ", appointment.Title, ", ", appointment.Location);
						appointments.Add (appointment);
					}
				}
			}
		}

		public void Add (UniAppointment appointment)
		{
			appointments.Add (appointment);
		}
	}
}

