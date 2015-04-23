using System;
using Core.Common;
using TS.Google.Calendar;

namespace UniversityCalendar
{
	public class UniConfig : IGoogleConfig
	{
		public UniConfig ()
		{
		}

		#region IGoogleConfig implementation

		public string GoogleUser {
			get {
				return "tobiasschulz.frauas@gmail.com";
			}
		}

		#endregion
	}
}

