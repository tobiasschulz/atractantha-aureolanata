using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Calendar.University
{
	public class Timeslot
	{
		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("start")]
		public TimeSpan Start { get; set; }

		[JsonProperty ("end")]
		public TimeSpan End { get; set; }

		public Timeslot ()
		{
		}

		#region Singleton

		private static Dictionary<string,Timeslot> instances = new Dictionary<string,Timeslot> ();

		public static Timeslot GetInstance (string shortName)
		{
			if (instances.ContainsKey (shortName.ToLower ())) {
				return instances [shortName.ToLower ()];
			} else {
				return null;
			}
		}

		public void Register ()
		{
			instances [Name.ToLower ()] = this;
		}

		#endregion
	}
}

