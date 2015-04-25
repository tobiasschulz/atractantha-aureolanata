using System;
using Core.Common;
using Newtonsoft.Json;
using Calendar.Google;

namespace Calendar.University
{
	public class UniConfig : IGoogleConfig
	{
		public InternalConfig Config { get; private set; }

		public UniConfig ()
		{
			const string configPath = @"../../../config.json";

			Config = ConfigHelper.OpenConfig<InternalConfig> (fullPath: configPath);

			if (Config.Timeslots.Length == 0) {
				Config.Timeslots = new Timeslot[] {
					new Timeslot {
						Name = "1",
						Start = new TimeSpan (8, 15, 0),
						End = new TimeSpan (9, 45, 0),
					},
					new Timeslot {
						Name = "2",
						Start = new TimeSpan (10, 0, 0),
						End = new TimeSpan (11, 30, 0),
					},
					new Timeslot {
						Name = "3",
						Start = new TimeSpan (11, 45, 0),
						End = new TimeSpan (13, 15, 0),
					},
					new Timeslot {
						Name = "4",
						Start = new TimeSpan (14, 15, 0),
						End = new TimeSpan (15, 45, 0),
					},
					new Timeslot {
						Name = "5",
						Start = new TimeSpan (16, 0, 0),
						End = new TimeSpan (17, 30, 0),
					},
					new Timeslot {
						Name = "6",
						Start = new TimeSpan (17, 45, 0),
						End = new TimeSpan (19, 15, 0),
					},
				};
			}

			if (Config.Subjects.Length == 0) {
				Config.Subjects = new Subject[] {
					new Subject {
						ShortName = "DisMa",
						FullName = "Diskrete Mathematik",
						Professor = "Kamlage",
					},
					new Subject {
						ShortName = "TI",
						FullName = "Theoretische Informatik",
						Professor = "Andersson",
					},
					new Subject {
						ShortName = "RT",
						FullName = "Realtime Systems",
						Professor = "Deegener",
					},
					new Subject {
						ShortName = "SWED",
						FullName = "SWE Design",
						Professor = "Godehardt",
					},
				};
			}

			foreach (var entry in Config.Subjects)
				entry.Register ();
			foreach (var entry in Config.Timeslots)
				entry.Register ();

			if (Config.Semesters.Length == 0) {
				Config.Semesters = new Semester[] {
					new Semester {
						Season = Season.Summer,
						Year = 2015,
						Start = new DateTime (2015, 04, 13),
						End = new DateTime (2015, 07, 17),
						Courses = new Course[] {
							new Course {
								SubjectShortName = "ti",
								TimeslotName = "1",
								DayOfWeek = DayOfWeek.Monday,
								CourseType = CourseType.Lecture,
								Building = "1",
								Room = "131",
							},
							new Course {
								SubjectShortName = "ti",
								TimeslotName = "2",
								DayOfWeek = DayOfWeek.Monday,
								CourseType = CourseType.Exercise,
								Building = "1",
								Room = "129",
							},
							new Course {
								SubjectShortName = "rt",
								TimeslotName = "2",
								DayOfWeek = DayOfWeek.Wednesday,
								CourseType = CourseType.Lecture,
								Room = "Audimax",
							},
							new Course {
								SubjectShortName = "rt",
								TimeslotName = "3",
								DayOfWeek = DayOfWeek.Wednesday,
								CourseType = CourseType.Exercise,
								Building = "1",
								Room = "250",
							},
							new Course {
								SubjectShortName = "disma",
								TimeslotName = "2",
								DayOfWeek = DayOfWeek.Thursday,
								CourseType = CourseType.Lecture,
								Room = "Audimax",
							},
							new Course {
								SubjectShortName = "disma",
								TimeslotName = "3",
								DayOfWeek = DayOfWeek.Thursday,
								CourseType = CourseType.Exercise,
								Building = "1",
								Room = "130",
							},
							new Course {
								SubjectShortName = "disma",
								TimeslotName = "1",
								DayOfWeek = DayOfWeek.Friday,
								CourseType = CourseType.Lecture,
								Room = "Audimax",
							},
							new Course {
								SubjectShortName = "swed",
								TimeslotName = "2",
								DayOfWeek = DayOfWeek.Friday,
								CourseType = CourseType.Lecture,
								Room = "Audimax",
							},
							new Course {
								SubjectShortName = "swed",
								TimeslotName = "3",
								DayOfWeek = DayOfWeek.Friday,
								CourseType = CourseType.Exercise,
								Building = "1",
								Room = "249",
							},
						},
					},
				};
			}

			ConfigHelper.SaveConfig (fullPath: configPath, stuff: Config);
		}

		#region IGoogleConfig implementation

		public string GoogleUser {
			get {
				return "tobiasschulz.frauas@gmail.com";
			}
		}

		#endregion

		public class InternalConfig
		{
			[JsonProperty ("semester")]
			public Semester[] Semesters = new Semester[0];

			[JsonProperty ("subjects")]
			public Subject[] Subjects = new Subject[0];

			[JsonProperty ("timeslots")]
			public Timeslot[] Timeslots = new Timeslot[0];
		}
	}
}

