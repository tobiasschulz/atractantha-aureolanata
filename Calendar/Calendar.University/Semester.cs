using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Calendar.University
{
	public class Semester
	{
		[JsonProperty ("season"), JsonConverter (typeof(StringEnumConverter))]
		public Season Season { get; set; }

		[JsonProperty ("year")]
		public int Year { get; set; }

		[JsonProperty ("start")]
		public DateTime Start { get; set; }

		[JsonProperty ("end")]
		public DateTime End { get; set; }

		[JsonProperty ("vacation_days")]
		public DateTime[] VacationDays { get; set; }

		[JsonProperty ("courses")]
		public Course[] Courses { get; set; }

		public Semester ()
		{
			Courses = new Course[0];
			VacationDays = new DateTime[0];
		}
	}
}

