using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Calendar.University
{
	public class Subject
	{
		[JsonProperty ("short_name")]
		public string ShortName { get; set; }

		[JsonProperty ("full_name")]
		public string FullName { get; set; }

		[JsonProperty ("professor")]
		public string Professor { get; set; }

		public Subject ()
		{
		}

		#region Singleton

		private static Dictionary<string,Subject> instances = new Dictionary<string,Subject> ();

		public static Subject GetInstance (string shortName)
		{
			if (instances.ContainsKey (shortName.ToLower ())) {
				return instances [shortName.ToLower ()];
			} else {
				return null;
			}
		}

		public void Register ()
		{
			instances [ShortName.ToLower ()] = this;
		}

		#endregion
	}
}

