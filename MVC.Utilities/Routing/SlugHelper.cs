using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MVC.Utilities.Routing
{
    /// <summary>
    /// Helper class for generating URL / SEO -friendly slugs for user-generated content
    /// </summary>
    public static class SlugHelper
    {
        /// <summary>
        /// Transforms a DateTime into a url-friendly hash
        /// </summary>
        /// <param name="time">A DateTime</param>
        /// <returns>A string representation of the DateTime as a timestamp</returns>
        public static string GenerateSlug(DateTime time)
        {
            return time.Ticks.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Transforms the string into a URL-friendly slug
        /// </summary>
        /// <param name="name">The original string</param>
        /// <returns>A string containing a url-friendly slug</returns>
        public static string GenerateSlug(string name)
        {
            var sb = new StringBuilder();
            string lower = name.ToLower();
            foreach (char c in lower)
            {
                if (c == ' ' || c == '.' || c == '=' || c == '-')
                    sb.Append('-');
                else if ((c <= 'z' && c >= 'a') || (c <= '9' && c >= '0'))
                    sb.Append(c);
            }

            return sb.ToString().Trim('-');
        }

        /// <summary>
        /// Transforms the string into a URL-friendly slug
        /// </summary>
        /// <param name="name">The original string</param>
        /// <returns>A string containing a url-friendly slug</returns>
        public static string ToSlug(this string name)
        {
            return GenerateSlug(name);
        }

        /// <summary>
        /// Transforms the date into a URL-friendly string representation of a timestamp
        /// </summary>
        /// <param name="time">The original date</param>
        /// <returns>A string representation of the date as a timestamp</returns>
        public static string ToSlug(this DateTime time)
        {
            return GenerateSlug(time);
        }
    }
}
