using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    class WeakReferenceTest
    {
        class WeakReferenceComparer : IEqualityComparer<WeakReference>
        {
            public bool Equals(WeakReference a,WeakReference b)
            {
                return a.Target.Equals(b.Target);
            }

            public int GetHashCode(WeakReference w)
            {
                return w.Target.GetHashCode();
            }
        }

        static WeakReferenceComparer MyComparer = new WeakReferenceComparer();

        [Test]
        public void DictionaryKey()
        {
            var vint = 1;
            var vstring = "abc";
            var vdata = new Dictionary<string, dynamic>();

            var wrDictionary = new Dictionary<WeakReference, IDictionary>(MyComparer);
            wrDictionary.Add(new WeakReference(vint), new Dictionary<string, dynamic> { { "Type", "int" } });
            wrDictionary.Add(new WeakReference(vstring), new Dictionary<string, dynamic> { { "Type", "string" } });
            wrDictionary.Add(new WeakReference(vdata), new Dictionary<string, dynamic> { { "Type", "IDictionary" } });

            Assert.True(wrDictionary.ContainsKey(new WeakReference(vdata)));
            var md = wrDictionary[new WeakReference(vdata)];
            Assert.AreEqual(md["Type"].ToString(), "IDictionary");
        }
    }
}
