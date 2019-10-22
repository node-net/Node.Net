using NUnit.Framework;
using System.Collections.Generic;
namespace Node.Net.Json
{
    [TestFixture,Category("Json"),Category("Children")]
    class Children_Test
    {
        [TestCase]
        public void Children_String_Usage()
        {
            string s = "abcdefg";
            Children children = new Children(s);
            Assert.AreEqual(0, children.Count);
        }

        [TestCase]
        public void Children_IEnumerable_Usage()
        {
            List<string> names = new List<string>();
            names.Add("a");
            names.Add("b");
            Children children = new Children(names);
            Assert.AreEqual(2, children.Count);
        }

        [TestCase]
        public void Children_IDictionary_Usage()
        {
            Dictionary<string, string> nameMap = new Dictionary<string, string>();
            nameMap.Add("a", "A");
            nameMap.Add("b", "B");
            Children children = new Children(nameMap);
            Assert.AreEqual(2, children.Count);
        }
    }
}
