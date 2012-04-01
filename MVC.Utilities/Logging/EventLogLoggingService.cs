using System;
using System.Diagnostics;

namespace MVC.Utilities.Logging
{
    public class EventLogLoggingService : ILoggingService
    {
        private readonly EventLog _log;

        public EventLogLoggingService(string eventLogSource) : this(eventLogSource, "Application"){}

        public EventLogLoggingService(string eventLogSource, string logName)
        {
            if (!EventLog.SourceExists(eventLogSource))
            {
                EventLog.CreateEventSource(eventLogSource, logName);
            }

            _log = new EventLog(logName, ".", eventLogSource);
        }

        #region Implementation of ILoggingService

        public void LogMessage(string message)
        {
            _log.WriteEntry(message);
        }

        public void LogMessage(string message, LogEntryType entryType)
        {
            _log.WriteEntry(message, MapLogEntryType(entryType));
        }

        public void LogException(Exception ex)
        {
            _log.WriteEntry(ex.ToTraceString(), EventLogEntryType.Error);
        }

        public void LogException(Exception ex, LogEntryType entryType)
        {
            _log.WriteEntry(ex.ToTraceString(), MapLogEntryType(entryType));
        }

        #endregion

        public static EventLogEntryType MapLogEntryType(LogEntryType entryType)
        {
            switch(entryType)
            {
                case LogEntryType.Error:
                    return EventLogEntryType.Error;
                case LogEntryType.Failure:
                    return EventLogEntryType.FailureAudit;
                case LogEntryType.Success:
                    return EventLogEntryType.SuccessAudit;
                case LogEntryType.Information:
                    return EventLogEntryType.Information;
                case LogEntryType.Warning:
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }

    }
}