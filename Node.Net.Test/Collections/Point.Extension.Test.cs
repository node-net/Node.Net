using NUnit.Framework;
using System.Windows;

namespace Node.Net.Collections.Test
{
    [TestFixture]
    class PointExtensionText
    {
        [Test]
        public void PointExtension_PointArrayFromString()
        {
            Assert.AreEqual(0, PointExtension.GetPointArray("").Length);
            Assert.AreEqual(1, PointExtension.GetPointArray("5,10").Length);
            Assert.AreEqual(3, PointExtension.GetPointArray("0,5 50.5,10.1 100,1.23").Length);
        }

        [Test]
        public void PointExtension_ToString()
        {
            Point[] points = { new Point(0,1), new Point(2,3)};
            Assert.AreEqual("0,1 2,3", PointExtension.ToString(points));
        }
    }
}
