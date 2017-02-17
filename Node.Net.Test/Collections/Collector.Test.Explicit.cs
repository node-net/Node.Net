using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections.Test
{
    [TestFixture]
    class CollectorTestExplicit : Fixture
    {
        /*
        [Test, Explicit]
        [TestCase("Translations.json", "", false, 9)]
        [TestCase("Translations.json","e",false,4)]
        public void Collector_CollectKeys_Pattern(string name, string key_pattern, bool exact_match, int count)
        {
            var keys = new Collector { KeyFilter = new Filters.StringFilter { Pattern = key_pattern }.Include }.CollectKeys(Read(name) as IDictionary);
            Assert.AreEqual(count, keys.Length);
        }

        [Test, Explicit]
        [TestCase("Translations.json", "", false, 10)]
        public void Collector_DeepCollectKeys(string name,string key_pattern,bool exact_match,int count)
        {
            var keys = new Collector().DeepCollectKeys(Read(name) as IDictionary);
            Assert.AreEqual(count, keys.Length);
        }*/
    
    }
}
