using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    internal class ObjectTest
    {
        [Test]
        public void SetParent()
        {
            var a = new Dictionary<string, object>();
            var b = new Dictionary<string, object>();
            a.SetParent(b);
            Assert.AreSame(b, a.GetParent(), "a.GetParent");
            a.SetParent(null);
            Assert.IsNull(a.GetParent(), "a.GetParent");
        }

        [Test]
        public void GetPropertyValue()
        {
            var a = new Dictionary<string, object>();
            Assert.AreEqual(0, a.GetPropertyValue<int>("Count"));
        }

        [Test]
        public void SetPropertyValue()
        {
            var foo = new Foo();
            foo.SetPropertyValue("Name", "foo");
            Assert.AreEqual("foo", foo.Name);
        }
    }
}
