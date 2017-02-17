using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections.Test
{
    [TestFixture]
    class DictionaryTest
    {
        [Test]
        public void Dictionary_Construction()
        {
            var d = new Dictionary();
            Assert.False(d.ContainsKey("Type"));
            var widget = new Dictionary("Widget");
            Assert.True(widget.ContainsKey("Type"));
            Assert.AreEqual("Widget", widget["Type"]);
        }
        [Test]
        public void Dictionary_Type()
        {
            var widget = new Dictionary("Widget");
            Assert.AreEqual("Widget", widget.Type);

            var d = new Dictionary();
            Assert.AreEqual("", d.Type);
        }
        [Test]
        public void Dictionary_Key()
        {
            var widget = new Dictionary("Widget");
            var foo = new Dictionary("Foo");
            widget["foo"] = foo;
            var bar = new Dictionary("Bar");
            foo["bar"] = bar;

            widget.UpdateParentReferences();
            Assert.AreEqual("", widget.Key, "widget.Key");
            Assert.AreEqual("foo", foo.Key,"foo.Key");
            Assert.AreEqual("bar", bar.Key, "bar.Key");
        }
        [Test]
        public void Dictionary_FullKey()
        {
            var widget = new Dictionary("Widget");
            var foo = new Dictionary("Foo");
            widget["foo"] = foo;
            var bar = new Dictionary("Bar");
            foo["bar"] = bar;

            widget.UpdateParentReferences();
            Assert.AreEqual("", widget.FullKey, "widget.FullKey");
            Assert.AreEqual("foo", foo.FullKey, "foo.FullKey");
            Assert.AreEqual("foo/bar", bar.FullKey, "bar.FullKey");
        }
    }
}
