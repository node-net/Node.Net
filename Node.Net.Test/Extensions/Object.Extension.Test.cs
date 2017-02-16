using NUnit.Framework;
using System.Windows;

namespace Node.Net.Extensions
{
    class Document
    {
        public string FileName { get; set; }
    }
    [TestFixture]
    class ObjectExtensionTest
    {
        [Test]
        public void ObjectExtension_SetProperty()
        {
            var point = new Point();
            point.SetPropertyValue("FileName", "?");
            Assert.AreEqual(null, point.GetPropertyValue<string>("FileName"));

            var doc = new Document();
            doc.SetPropertyValue("FileName", "?");
            Assert.AreEqual("?", doc.GetPropertyValue<string>("FileName"));
            Assert.AreEqual("?", doc.GetPropertyValue("FileName").ToString());

        }
    }
}
