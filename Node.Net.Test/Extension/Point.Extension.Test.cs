using NUnit.Framework;
using System.Windows;

namespace Node.Net.Test
{
    [TestFixture]
    internal class PointExtensionTest
    {
        [Test]
        public void GetLength()
        {
            Assert.AreEqual(15.0, new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) }.GetLength());
        }

        [Test]
        public void Close()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Assert.False(points.IsClosed());
            Point[] points2 = points.Close();
            Assert.True(points2.IsClosed());
            Point[] points3 = points2.Close();
            Assert.AreEqual(5, points3.Length);
        }

        [Test]
        public void GetArea()
        {
            Assert.AreEqual(25.0, new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) }.GetArea());
        }

        [Test]
        public void GetPointAtDistance()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point a = points.GetPointAtDistance(3.0);
            Assert.AreEqual(3.0, a.Y);
            a = points.GetPointAtDistance(33.0);
            Assert.AreEqual(5, a.X);
        }

        [Test]
        public void Offset()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point[] offset = points.Offset(2.0);
            Assert.AreEqual(6, offset.Length);
        }

        [Test]
        public void OffsetWithArcs()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point[] offset = points.OffsetWithArcs(2.0);
            Assert.AreEqual(42, offset.Length);
        }

        [Test]
        public void GetA()
        {
            Assert.AreEqual(-1, new Point(0, 0).GetA(new Point(1, 1)));
        }

        [Test]
        public void GetC()
        {
            Assert.AreEqual(0, new Point(0, 0).GetC(new Point(1, 1)));
        }

        [Test]
        public void GetSlope()
        {
            Assert.AreEqual(1, new Point(0, 0).GetSlope(new Point(1, 1)));
        }

        [Test]
        public void ParsePoints()
        {
            Point[] points = PointExtension.ParsePoints("0,0 1,0");
            Assert.AreEqual(2, points.Length);

            points = PointExtension.ParsePoints("0,0,0 1,0,0");
            Assert.AreEqual(2, points.Length);
        }

        [Test]
        public void GetString()
        {
            Point[] points = new Point[] { new Point(3, 4) };
            string str = points.GetString();
            Assert.AreEqual("3,4", str);
        }

        [Test]
        public void GetCentroid()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point centroid = points.GetCentroid();
            Assert.AreEqual(2.5, centroid.X);
        }


    }
}