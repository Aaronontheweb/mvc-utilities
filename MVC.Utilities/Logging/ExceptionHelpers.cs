using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Utilities.Logging
{
    /// <summary>
    /// Helper class for condensing exceptions down into single strings
    /// </summary>
    public static class ExceptionHelpers
    {
        /// <summary>
        /// Condenses an exception down into a string that is of an appropriate size for System.Trace
        /// </summary>
        /// <param name="ex">The exception to trace</param>
        /// <returns>A string containing the Exception message, Source, and Trace</returns>
        public static string ToTraceString(this Exception ex)
        {
            return string.Format("Exception: {0}\r\n Source:{1}\r\n Trace:{2}", ex.Message, ex.Source, ex.StackTrace);
        }
    }
}
