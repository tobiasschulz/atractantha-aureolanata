using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Calendar.University
{
	public class Course
	{
		[JsonProperty ("subject_short_name")]
		public string SubjectShortName { get; set; }

		[JsonIgnore]
		public Subject Subject { get { return Subject.GetInstance (SubjectShortName); } set { SubjectShortName = value.ShortName; } }

		[JsonProperty ("timeslot_name")]
		public string TimeslotName { get; set; }

		[JsonIgnore]
		public Timeslot Timeslot { get { return Timeslot.GetInstance (TimeslotName); } set { TimeslotName = value.Name; } }

		[JsonProperty ("day_of_week"), JsonConverter (typeof(StringEnumConverter))]
		public DayOfWeek DayOfWeek { get; set; }

		[JsonProperty ("course_type"), JsonConverter (typeof(StringEnumConverter))]
		public CourseType CourseType { get; set; }

		[JsonProperty ("building")]
		public string Building { get; set; }

		[JsonProperty ("room")]
		public string Room { get; set; }

		public Course ()
		{
		}
	}
}

