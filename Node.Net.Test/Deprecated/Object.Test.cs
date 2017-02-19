using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Deprecated
{
    [TestFixture]
    class ObjectTest
    {
        #region GetKey
        [TestCase]
        public void ObjectExtension_GetKey()
        {
            Assert.AreEqual(null, 3.GetKey());
            var kvp = new KeyValuePair<string, int>("a", 3);
            Assert.AreEqual("a", kvp.GetKey());// ObjectExtension.GetKey(kvp));
            //Assert.AreEqual("a", ObjectExtension.GetKey(new KeyValuePair<string, object>("a", null)));
            //Assert.AreEqual("a", ObjectExtension.GetKey(new DictionaryEntry("a", null)));
            //Assert.AreEqual("a", ObjectExtension.GetKey(new KeyValuePair<string, string>("a", "A")));
        }
        #endregion
        #region GetValue
        [TestCase]
        public void ObjectExtension_GetValue()
        {
            Assert.AreEqual(3, 3.GetValue());
            var kvp = new KeyValuePair<string, int>("a", 3);
            //Assert.AreEqual(3, ObjectExtension.GetValue(kvp));
            //Assert.AreEqual("A", ObjectExtension.GetValue(new KeyValuePair<string, string>("a", "A")));
        }
        #endregion
    }
}
