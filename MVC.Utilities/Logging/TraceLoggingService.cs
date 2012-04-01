using System;
using System.Diagnostics;

namespace MVC.Utilities.Logging
{
    public class TraceLoggingService : ILoggingService
    {
        #region Implementation of ILoggingService

        public void LogMessage(string message)
        {
            Trace.WriteLine(message);
        }

        public void LogMessage(string message, LogEntryType entryType)
        {
            WriteToTraceLevel(message, entryType);
        }

        public void LogException(Exception ex)
        {
            Trace.TraceError(ex.ToTraceString());
        }

        public void LogException(Exception ex, LogEntryType entryType)
        {
            Trace.TraceInformation(string.Format("Error level: {0}\r\n{1}", entryType, ex.ToTraceString()));
        }

        /* Handler designed to use the appropriate tracing tool depending upon the intent of the end-user */
        private static void WriteToTraceLevel(string message, LogEntryType entryType)
        {
            switch(entryType)
            {
                case LogEntryType.Error: //Designed to fall-through to the failure case
                case LogEntryType.Failure:
                    Trace.TraceError(message);
                    break;
                case LogEntryType.Information:
                    Trace.TraceInformation(message);
                    break;
                case LogEntryType.Warning:
                    Trace.TraceWarning(message);
                    break;
                default:
                    Trace.WriteLine(message, entryType.ToString());
                    break;
            }
        }

        #endregion
    }
}