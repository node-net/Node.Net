using NUnit.Framework;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test
{
    [TestFixture]
    internal class PointExtensionTest
    {
        [Test]
        public void GetLength()
        {
            Assert.That(new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) }.GetLength(),Is.EqualTo(15.0));
        }

        [Test]
        public void Close()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Assert.That(points.IsClosed(),Is.False);
            Point[] points2 = points.Close();
            Assert.That(points2.IsClosed(), Is.True);
            Point[] points3 = points2.Close();
            Assert.That(points3.Length, Is.EqualTo(5));
        }

        [Test]
        public void GetArea()
        {
            Assert.That(new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) }.GetArea(), Is.EqualTo(25.0));
        }

        [Test]
        public void GetPointAtDistance()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point a = points.GetPointAtDistance(3.0);
            Assert.That(a.Y, Is.EqualTo(3.0));
            a = points.GetPointAtDistance(33.0);
            Assert.That(a.X, Is.EqualTo(5));
        }

        [Test]
        public void Offset()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point[] offset = points.Offset(2.0);
            Assert.That(offset.Length, Is.EqualTo(6));
        }

        [Test]
        public void OffsetWithArcs()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point[] offset = points.OffsetWithArcs(2.0);
            Assert.That(offset.Length, Is.EqualTo(42));
        }

        [Test]
        public void GetA()
        {
            Assert.That(new Point(0, 0).GetA(new Point(1, 1)), Is.EqualTo(-1));
        }

        [Test]
        public void GetC()
        {
            Assert.That(new Point(0, 0).GetC(new Point(1, 1)), Is.EqualTo(0));
        }

        [Test]
        public void GetSlope()
        {
            Assert.That(new Point(0, 0).GetSlope(new Point(1, 1)), Is.EqualTo(1));
        }

        [Test]
        public void ParsePoints()
        {
            Point[] points = PointExtension.ParsePoints("0,0 1,0");
            Assert.That(points.Length, Is.EqualTo(2));

            points = PointExtension.ParsePoints("0,0,0 1,0,0");
            Assert.That(points.Length, Is.EqualTo(2));
        }

        [Test]
        public void GetString()
        {
            Point[] points = new Point[] { new Point(3, 4) };
            string str = points.GetString();
            Assert.That(str, Is.EqualTo("3,4"));
        }

        [Test]
        public void GetCentroid()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point centroid = points.GetCentroid();
            Assert.That(centroid.X, Is.EqualTo(2.5));
        }


    }
}