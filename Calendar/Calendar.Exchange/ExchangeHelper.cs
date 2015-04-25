using System;
using Core.Common;

namespace Calendar.Exchange
{
	public static class ExchangeHelper
	{
		public static void Catch<T> (string propName, Action action) where T : Exception
		{
			ExceptionHelper.Catch<T> (tryAction: action, catchAction: ex => Log.Error ("Error: ", propName, ": ", ex.Message));
		}
	}
}

