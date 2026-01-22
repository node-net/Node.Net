using System.Threading.Tasks;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test
{
    internal class PointExtensionTest
    {
        [Test]
        public async Task GetLength()
        {
            await Assert.That(new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) }.GetLength()).IsEqualTo(15.0);
        }

        [Test]
        public async Task Close()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            await Assert.That(points.IsClosed()).IsFalse();
            Point[] points2 = points.Close();
            await Assert.That(points2.IsClosed()).IsTrue();
            Point[] points3 = points2.Close();
            await Assert.That(points3.Length).IsEqualTo(5);
        }

        [Test]
        public async Task GetArea()
        {
            await Assert.That(new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) }.GetArea()).IsEqualTo(25.0);
        }

        [Test]
        public async Task GetPointAtDistance()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point a = points.GetPointAtDistance(3.0);
            await Assert.That(a.Y).IsEqualTo(3.0);
            a = points.GetPointAtDistance(33.0);
            await Assert.That(a.X).IsEqualTo(5);
        }

        [Test]
        public async Task Offset()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point[] offset = points.Offset(2.0);
            await Assert.That(offset.Length).IsEqualTo(6);
        }

        [Test]
        public async Task OffsetWithArcs()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point[] offset = points.OffsetWithArcs(2.0);
            await Assert.That(offset.Length).IsEqualTo(42);
        }

        [Test]
        public async Task GetA()
        {
            await Assert.That(new Point(0, 0).GetA(new Point(1, 1))).IsEqualTo(-1);
        }

        [Test]
        public async Task GetC()
        {
            await Assert.That(new Point(0, 0).GetC(new Point(1, 1))).IsEqualTo(0);
        }

        [Test]
        public async Task GetSlope()
        {
            await Assert.That(new Point(0, 0).GetSlope(new Point(1, 1))).IsEqualTo(1);
        }

        [Test]
        public async Task ParsePoints()
        {
            Point[] points = PointExtension.ParsePoints("0,0 1,0");
            await Assert.That(points.Length).IsEqualTo(2);

            points = PointExtension.ParsePoints("0,0,0 1,0,0");
            await Assert.That(points.Length).IsEqualTo(2);
        }

        [Test]
        public async Task GetString()
        {
            Point[] points = new Point[] { new Point(3, 4) };
            string str = points.GetString();
            await Assert.That(str).IsEqualTo("3,4");
        }

        [Test]
        public async Task GetCentroid()
        {
            Point[] points = new Point[] { new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0) };
            Point centroid = points.GetCentroid();
            await Assert.That(centroid.X).IsEqualTo(2.5);
        }


    }
}