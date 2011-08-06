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
    }
}
