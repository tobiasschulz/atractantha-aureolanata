using System;
using System.IO;
using Microsoft.Exchange.WebServices.Data;
using Core.Common;

namespace Calendar.Exchange
{
	public class LogTraceListener : ITraceListener
	{
		public void Trace (string traceType, string traceMessage)
		{
			Log.Trace ("TraceType: " + traceType);
			Log.Trace ("TraceMessage: ");
			Log.Indent++;
			Log.Trace (traceMessage);
			Log.Indent--;
		}
	}
}

