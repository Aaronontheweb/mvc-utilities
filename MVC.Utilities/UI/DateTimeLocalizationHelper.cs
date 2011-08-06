using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC.Utilities.UI
{
    /// <summary>
    /// Static helper class used to represent times as relative times (ghetto localization 4tw)
    /// </summary>
    public static class DateTimeLocalizationHelper
    {
        /// <summary>
        /// Extension method for HTML Helpers in ASP.NET MVC
        /// </summary>
        /// <param name="html">An HTML Helper instance</param>
        /// <param name="date">A datetime to displayed in relative terms</param>
        /// <returns>An html-encoded string with the date displayed as relative</returns>
        public static MvcHtmlString ToRelativeDateTime(this HtmlHelper html, DateTime date)
        {
            return MvcHtmlString.Create(ToRelativeDateTime(date));
        }

        /// <summary>
        /// Converts a datetime into "x [units] ago" or "in x [units]"
        /// </summary>
        /// <param name="date">A datetime to displayed in relative terms</param>
        /// <returns>An html-encoded string with the date displayed as relative</returns>
        public static string ToRelativeDateTime(DateTime date)
        {
            var offset = DateTime.UtcNow - date;
            var timeString = "{0} {1} ago";

            //In case [date] is a future date
            if (offset.TotalSeconds < 0)
            {
                timeString = "in {0} {1}";
            }

            //If the offset can be expressed in days
            if (Math.Abs(offset.Days) > 0)
            {
                //Using offset.TotalDays so we can get 9 days ago instead of (1 week ago)
                var totalDays = (int)Math.Round(Math.Abs(offset.TotalDays), MidpointRounding.AwayFromZero);
                return string.Format(timeString, totalDays, totalDays == 1 ? "day" : "days");
            }

            var hours = Math.Abs(offset.Hours);
            //If the offset can be expressed in hours
            if (hours > 0)
            {
                return string.Format(timeString, hours, hours == 1 ? "hour" : "hours");
            }

            var minutes = Math.Abs(offset.Minutes);
            //If the offset can be expressed in minutes)
            if (minutes > 0)
            {
                return string.Format(timeString, minutes, minutes == 1 ? "minute" : "minutes");
            }

            var seconds = Math.Abs(offset.Seconds);
            //Otherwise, express it in seconds)
            return string.Format(timeString, seconds, seconds == 1 ? "second" : "seconds");
        }
    }
}
