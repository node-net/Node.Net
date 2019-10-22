using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    [TestFixture]
    class ObjectExtensionTest
    {
        [Test]
        public void ObjectExtension_Get_Set_FullName()
        {
            var point = new Point(0, 0);
            Assert.AreEqual("", point.GetFullName());
            point.SetFullName("Scope/Origin");
            Assert.AreEqual("Scope/Origin", point.GetFullName());
            Assert.AreEqual("Origin", point.GetName());

            var dictionary = new Dictionary<string, dynamic>();
            dictionary["a"] = new Dictionary<string, dynamic>();
            var c = new Dictionary<string, dynamic>();
            dictionary["a"]["b"] = c;
            Assert.AreEqual("", c.GetFullName());
            dictionary.DeepUpdateParents();
            Assert.AreEqual("a/b", c.GetFullName());
            Assert.AreEqual("b", c.GetName());
        }

        [Test,Apartment(ApartmentState.STA),Explicit]
        public void GetFileName()
        {
            var item = Factory.Default.Create<object>("XAML Files(.xaml)|*.xaml|All Files(*.*)|*.*");
            var filename = item.GetFileName();
            Assert.True(File.Exists(filename));
        }

        [Test]
        public void GetSetName()
        {
            var material = new DiffuseMaterial { Brush = Brushes.Red };
            material.SetFullName("Red");
            Assert.AreEqual("Red", material.GetName());
        }
    }
}
