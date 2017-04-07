using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
    }
}
