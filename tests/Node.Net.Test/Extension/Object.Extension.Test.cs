using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test.Extension
{
    public class Widget
    {
        public object Parent { get; set; }
    }

    public class ObjectExtensionTest
    {
        [Test]
        public async Task GetName_SetName()
        {
            DateTime dateTime = DateTime.Now;
            await Assert.That(dateTime.GetName()).IsEqualTo("");
            dateTime.SetName("Now");
            //Assert.AreEqual("Now", dateTime.GetName());
            await Task.CompletedTask;
        }

        [Test]
        public async Task GetParent_SetParent()
        {
            Dictionary<string, dynamic> bar = new Dictionary<string, dynamic> { { "Name", "bar" } };
            bar.SetParent(null);
            Dictionary<string, dynamic> foo = new Dictionary<string, dynamic>
            {
                {"Name","foo" },
                {"bar",bar }
            };
            bar.SetParent(foo);
            await Assert.That(ReferenceEquals(foo, bar.GetParent())).IsTrue();

            Widget widget = new Widget();
            await Assert.That(widget.GetParent()).IsNull();
            widget.SetParent(null);
            await Assert.That(widget.GetParent()).IsNull();

            bar.ClearMetaData();
        }

        [Test]
        public async Task GetName_From_Dictionary()
        {
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>
            {
                {"Name", "foo" }
            };
            await Assert.That(dict.GetName()).IsEqualTo("foo");
            var obj = dict;
            await Assert.That(obj.GetName()).IsEqualTo("foo");
        }
    }
}