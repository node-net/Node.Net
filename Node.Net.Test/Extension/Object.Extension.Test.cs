﻿using NUnit.Framework;
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
            Assert.That(dateTime.GetName(),Is.EqualTo(""));
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
            bar.SetParent(foo);
            Assert.That(foo, Is.SameAs(bar.GetParent()));

            Widget widget = new Widget();
            Assert.That(widget.GetParent(),Is.Null);
            widget.SetParent(null);
            Assert.That(widget.GetParent(), Is.Null);

            bar.ClearMetaData();
        }
    }
}