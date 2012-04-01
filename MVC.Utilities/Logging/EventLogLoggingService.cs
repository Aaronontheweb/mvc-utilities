using System.Diagnostics;

namespace MVC.Utilities.Logging
{
    public class EventLogLoggingService : ILoggingService
    {
        private readonly string _eventLogSource;

        public EventLogLoggingService(string eventLogSource)
        {
            _eventLogSource = eventLogSource;
        }

        #region Implementation of ILoggingService

        public void LogMessage(string message)
        {
            EventLog.WriteEntry(_eventLogSource, message);
        }

        #endregion

    }
}