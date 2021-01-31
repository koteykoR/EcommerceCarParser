using System.Collections.Generic;
using System.Text;

namespace EcommerceCarsParser.Extension
{
    public static class IEnumerableExtension
    {
        public static string GetString(this IEnumerable<char> enumerable)
        {
            var sb = new StringBuilder();

            foreach (var element in enumerable)
            {
                sb.Append(element);
            }

            return sb.ToString();
        }
    }
}
