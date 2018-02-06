using NUnit.Framework;
using System.Windows;

namespace Node.Net.Tests
{
	[TestFixture, Category(nameof(PointExtension))]
	internal class PointExtensionTest
	{
		[Test]
		public void GetPointsAtInterval()
		{
			var outline = new Point[] { new Point(0, 0), new Point(10, 0), new Point(10, 10), new Point(0, 10) };
			outline = outline.Close();
			Assert.AreEqual(5, outline.Length, "outline.Length");
			Assert.AreEqual(40.0, outline.GetLength(), "outline.GetLength()");
			var points = outline.GetPointsAtInterval(5.0);
			Assert.AreEqual(9, points.Length, "points.Length");
		}

		[Test]
		public void GetString()
		{
			var points = new Point[] { new Point(0, 0), new Point(10, 0), new Point(10, 10), new Point(0, 10) };
			Assert.AreEqual("0,0 10,0 10,10 0,10", points.GetString());
		}

		[Test]
		public void ParsePoints()
		{
			var points = PointExtension.ParsePoints("0,0 10,0 10,5 0,5");
			Assert.AreEqual(4, points.Length);
			Assert.AreEqual(0.0, points[0].X);
			Assert.AreEqual(0.0, points[0].Y);
			Assert.AreEqual(10.0, points[1].X);
			Assert.AreEqual(0.0, points[1].Y);
			Assert.AreEqual(10.0, points[2].X);
			Assert.AreEqual(5.0, points[2].Y);
			Assert.AreEqual(0.0, points[3].X);
			Assert.AreEqual(5.0, points[3].Y);
		}

		[Test]
		public void GetArea()
		{
			var points = PointExtension.ParsePoints("0,0 100,0 100,50 0,50");
			Assert.AreEqual(5000.0, points.GetArea());
		}

		[Test]
		public void GetLength()
		{
			var points = PointExtension.ParsePoints("0,0 100,0 100,50 0,50 0,0");
			Assert.AreEqual(300.0, points.GetLength());
		}

		[Test]
		public void GetCentroid()
		{
			var points = PointExtension.ParsePoints("-50,-25 50,-25 50,25 -50,25 -50,-25");
			var centroid = points.GetCentroid();
			Assert.AreEqual(0.0, centroid.X, "centroid.X");
			Assert.AreEqual(0.0, centroid.Y, "centroid.Y");
		}

		[Test]
		public void Offset()
		{
			var points = PointExtension.ParsePoints("-1,-1 1,-1 1,1 -1,1");
			var offset = points.Offset(2.0);
			Assert.AreEqual(-3.0, offset[0].X, "offset[0].X");
			Assert.AreEqual(3.0, offset[2].X, "offset[2].X");
		}

		[Test]
		public void Contains()
		{
			var points = PointExtension.ParsePoints("-20,-10 20,-10 20,10 -20,10");
			Assert.True(points.Contains(new Point(-20, -10)), "Contains(-20,-10)");
			Assert.True(points.Contains(new Point(-20, 10)), "Contains(-20,10)");
			Assert.True(points.Contains(new Point(-10, 10)), "Contains(-10,10)");
		}

		[Test]
		public void IsPointOnLine()
		{
			Assert.False(PointExtension.IsPointOnLine(new Point(-20, -10), new Point(20, -10), new Point(0, -11)), "a");
			Assert.True(PointExtension.IsPointOnLine(new Point(-20, -10), new Point(20, -10), new Point(0, -10)), "b");
			Assert.True(PointExtension.IsPointOnLine(new Point(20, 10), new Point(-20, 10), new Point(0, 10)), "c");
			Assert.True(PointExtension.IsPointOnLine(new Point(20, -10), new Point(20, 10), new Point(20, 0)), "d");
		}

		[Test]
		public void IsPointOnPolyline()
		{
			var points = PointExtension.ParsePoints("-20,-10 20,-10 20,10 -20,10");
			Assert.True(points.IsPointOnPolyline(new Point(0, 10)));
			Assert.True(points.IsPointOnPolyline(new Point(20, 0)));
		}

		[Test]
		public void Close()
		{
			var points = PointExtension.ParsePoints("-20,-10 20,-10 20,10 -20,10");
			Assert.AreEqual(4, points.Length);

			var closed = points.Close();
			Assert.AreEqual(5, closed.Length);

			var open = closed.Open();
			Assert.AreEqual(4, open.Length);
		}
	}
}