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
            Trace.WriteLine(message, entryType.ToString());
        }

        public void LogException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception ex, LogEntryType entryType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}