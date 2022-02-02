using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Helpers
{
    public static class StringExtensions
    {
        public static bool MatchesUrl(this string url, string matchUrl)
        {
            var leftString = url.ToLower().TrimEnd(new[] { '/', '\\' });
            var testString = matchUrl.ToLower().TrimEnd(new[] { '/', '\\' });

            return leftString == testString;

        }

        public static bool EqualsCaseInsensitive(this string a, string b)
            => String.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
    }
}
