using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Node.Net.Test.Extension
{
    public class Widget
    {
        public object Parent { get; set; }
    }

    [TestFixture, Category("Object.Extension")]
    public class ObjectExtensionTest
    {
        [Test]
        public void GetName_SetName()
        {
            DateTime dateTime = DateTime.Now;
            Assert.AreEqual("", dateTime.GetName());
            dateTime.SetName("Now");
            //Assert.AreEqual("Now", dateTime.GetName());
        }

        [Test]
        public void GetParent_SetParent()
        {
            Dictionary<string, dynamic> bar = new Dictionary<string, dynamic> { { "Name", "bar" } };
            bar.SetParent(null);
            Dictionary<string, dynamic> foo = new Dictionary<string, dynamic>
            {
                {"Name","foo" },
                {"bar",bar }
            };
            Assert.IsNull(bar.GetParent());
            bar.SetParent(foo);
            Assert.AreSame(foo, bar.GetParent());

            Widget widget = new Widget();
            Assert.IsNull(widget.GetParent());
            widget.SetParent(null);
            Assert.IsNull(widget.GetParent());

            bar.ClearMetaData();
        }
    }
}