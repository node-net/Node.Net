using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Node.Net.Data.Extension
{
    public class RegexExtension
    {
        public static string[] GetGroupMatches(Regex regex, string text, int group_index = 0)
        {
            List<string> results = new List<string>();
            MatchCollection matches = regex.Matches(text);
            for (int i = 0; i < matches.Count; ++i)
            {
                if (matches[i].Groups.Count > group_index + 1)
                {
                    results.Add(matches[i].Groups[group_index + 1].Value);
                }
            }
            /*
            Assert.AreEqual(2, matches.Count);
            GroupCollection groups0 = matches[0].Groups;
            GroupCollection groups1 = matches[1].Groups;
            Assert.AreEqual("rakefile", matches[0].Groups[1].Value);
            Assert.AreEqual("test", matches[1].Groups[1].Value);*/

            return results.ToArray();
        }
    }
}
