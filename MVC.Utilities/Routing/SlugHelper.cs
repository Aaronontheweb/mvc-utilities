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
            foreach (var c in name.Where(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' '))
            {
                sb.Append(c);
            }

            return sb.ToString().Replace(" ", "-").ToLower();
        }
    }
}
