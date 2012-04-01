using System.Diagnostics;

namespace MVC.Utilities.Logging
{
    public class EventLogLoggingService : ILoggingService
    {
        private EventLog log;

        public EventLogLoggingService(string eventLogSource) : this(eventLogSource, "Application"){}

        public EventLogLoggingService(string eventLogSource, string logName)
        {
            if (!EventLog.SourceExists(eventLogSource))
            {
                EventLog.CreateEventSource(eventLogSource, logName);
            }

            log = new EventLog(logName, ".", eventLogSource);
        }

        #region Implementation of ILoggingService

        public void LogMessage(string message)
        {
            log.WriteEntry(message);
        }

        #endregion

    }
}