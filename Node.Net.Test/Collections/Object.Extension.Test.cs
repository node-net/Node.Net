using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections.Test
{
    [TestFixture]
    class ObjectExtensionTest
    {
        #region GetKey
        [TestCase]
        public void ObjectExtension_GetKey()
        {
            Assert.AreEqual(null, ObjectExtension.GetKey(3));
            var
                kvp = new KeyValuePair<string, int>("a", 3);
            Assert.AreEqual("a", ObjectExtension.GetKey(kvp));
            Assert.AreEqual("a", ObjectExtension.GetKey(new KeyValuePair<string, object>("a", null)));
            Assert.AreEqual("a", ObjectExtension.GetKey(new DictionaryEntry("a", null)));
            Assert.AreEqual("a", ObjectExtension.GetKey(new KeyValuePair<string, string>("a", "A")));
        }
        #endregion
        #region GetValue
        [TestCase]
        public void ObjectExtension_GetValue()
        {
            Assert.AreEqual(3, ObjectExtension.GetValue(3));
            var
               kvp = new KeyValuePair<string, int>("a", 3);
            Assert.AreEqual(3, ObjectExtension.GetValue(kvp));
            Assert.AreEqual("A", ObjectExtension.GetValue(new KeyValuePair<string, string>("a", "A")));
        }
        #endregion

        [Test]
        public void Object_GetSetParent_NonIntrusive()
        {
            var child = new Dictionary<string, dynamic>();
            var parent = new Dictionary<string, dynamic>();
            Node.Net.Collections.ObjectExtension.SetParent(child, parent);
            Assert.AreSame(parent, Node.Net.Collections.ObjectExtension.GetParent(child));
        }

        [Test]
        public void ObjectExtension_Ancestors()
        {
            var a = new Dictionary<string, dynamic>();
            a["name"] = nameof(a);
            var b = new Dictionary<string, dynamic>();
            b["name"] = nameof(b);
            var c = new Dictionary<string, dynamic>();
            c["name"] = nameof(c);
            a[nameof(b)] = b;
            b[nameof(c)] = c;

            //var results = IDictionaryExtension.DeepCollect<IDictionary>(a);
            //Assert.AreEqual(2, results.Count);
            IDictionaryExtension.DeepUpdateParents(a);
            Assert.AreEqual(a, ObjectExtension.GetParent(b));
            Assert.AreEqual(b, ObjectExtension.GetNearestAncestor<IDictionary>(c));
            Assert.AreEqual(a, ObjectExtension.GetFurthestAncestor<IDictionary>(c));
            Assert.AreEqual(a, ObjectExtension.GetRootAncestor(c));
        }

        [Test]
        public void Object_GetKey()
        {
            var child = new Dictionary<string, dynamic>();
            var parent = new Dictionary<string, dynamic>();
            parent["child"] = child;
            IDictionaryExtension.DeepUpdateParents(parent);
            //Node.Net.Collections.IDictionaryExtension.DeepCollect<IDictionary>(parent);
            Assert.AreEqual("child", ObjectExtension.GetKey(child));
        }

        [Test]
        public void Object_GetFullKey()
        {
            var dictionary = new Dictionary<string, dynamic>();
            var things = new Dictionary<string, dynamic>();
            var thing1 = new Dictionary<string, dynamic>();
            var thing2 = new Dictionary<string, dynamic>();
            things["thing-1"] = thing1;
            things["thing-2"] = thing2;
            dictionary["things"] = things;
            IDictionaryExtension.DeepUpdateParents(dictionary);
            //var results = IDictionaryExtension.DeepCollect<IDictionary>(dictionary);
            Assert.AreEqual("things/thing-2", ObjectExtension.GetFullKey(thing2));
        }
    }
}
