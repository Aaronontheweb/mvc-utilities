using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Utilities.Logging
{
    /// <summary>
    /// Interface for classes that support standard logging behaviors
    /// </summary>
    public interface ILoggingService
    {
        void LogMessage(string message);
        void LogMessage(string message, LogEntryType entryType);
        void LogException(Exception ex);
        void LogException(Exception ex, LogEntryType entryType);
    }

    public enum LogEntryType
    {
        Success, Failure, Error, Warning, Information
    }
}
