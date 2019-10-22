using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Node.Net.Deprecated.Data
{
    public class RegexExtension
    {
        public static string[] GetGroupMatches(Regex regex, string text, int group_index = 0)
        {
            var results = new List<string>();
            var matches = regex.Matches(text);
            for (int i = 0; i < matches.Count; ++i)
            {
                if (matches[i].Groups.Count > group_index + 1)
                {
                    results.Add(matches[i].Groups[group_index + 1].Value);
                }
            }
            return results.ToArray();
        }
    }
}
