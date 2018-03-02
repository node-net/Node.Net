using NUnit.Framework;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Math;

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
            Assert.AreEqual(6, offset.Length, "offset.Length");
            Assert.AreEqual(-1.0, Round(offset[0].X, 2), "offset[0].X");
            Assert.AreEqual(-3.0, Round(offset[0].Y, 2), "offset[0].Y");
            Assert.AreEqual(1.0, Round(offset[1].X, 2), "offset[1].X");
            Assert.AreEqual(-3.0, Round(offset[1].Y, 2), "offset[1].Y");
            Assert.AreEqual(3.0, Round(offset[2].X, 2), "offset[2].X");
            Assert.AreEqual(-1.0, Round(offset[2].Y, 2), "offset[2].Y");

            points = PointExtension.ParsePoints("-5.5,-5.5 0,-5.5 5.5,-5.5 11,-5.5");
            offset = points.Offset(10.0);
            Assert.AreEqual(-5.5, Round(offset[0].X, 2), "offset[0].X");
            Assert.AreEqual(-15.5, Round(offset[0].Y, 2), "offset[0].Y");
            /*
			Assert.AreEqual(-3.0, offset[0].X, "offset[0].X");
			Assert.AreEqual(3.0, offset[2].X, "offset[2].X");

			points = PointExtension.ParsePoints("-10,-5 10,-5 10,5 -10,5");
			offset = points.Offset(2.0);
			Assert.AreEqual(-12, offset[0].X, "offset[0].X");
			Assert.AreEqual(-7, offset[0].Y, "offset[0].Y");

			points = PointExtension.ParsePoints("0,0 10,0 10,5 0,5");
			offset = points.Offset(2.0);
			Assert.AreEqual(-2, offset[0].X, "offset[0].X");
			Assert.AreEqual(-2, offset[0].Y, "offset[0].Y");
			Assert.AreEqual(12, offset[2].X, "offset[2].X");
			Assert.AreEqual(7, offset[2].Y, "offset[2].Y");
            */
        }

        [Test]
        public void OffsetWithArcs()
        {
            var points = PointExtension.ParsePoints("-1,-1 1,-1 1,1 -1,1");
            var offset = points.OffsetWithArcs(2.0);
            Assert.AreEqual(42, offset.Length, "offset.Length");
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

        [Test]
        public void GetConnectingArcPoints()
        {
            var arcPoints = PointExtension.GetConnectingArcPoints(
                new Point(100.0, 0.0), new Point(50.0, 10.0),
                new Point(-50.0, 10.0), new Point(-100.0, 0.0));
            Assert.AreEqual(5, arcPoints.Count, "arcPoints.Count case 1");
            Assert.AreEqual(33.45, Round(arcPoints[0].X, 2), "arcPoints[0].X case 1");
            Assert.AreEqual(16.76, Round(arcPoints[1].X, 2), "arcPoints[1].X case 1");
            Assert.AreEqual(0.0, Round(arcPoints[2].X, 2), "arcPoints[2].X case 1");
            Assert.AreEqual(-16.76, Round(arcPoints[3].X, 2), "arcPoints[3].X case 1");
            Assert.AreEqual(-33.45, Round(arcPoints[4].X, 2), "arcPoints[4].X case 1");

            arcPoints = PointExtension.GetConnectingArcPoints(
                new Point(-100.0, 0.0), new Point(-50.0, 10.0),
                new Point(50.0, 10.0), new Point(100.0, 0.0));
            Assert.AreEqual(5, arcPoints.Count, "arcPoints.Count case 2");
            Assert.AreEqual(-33.45, Round(arcPoints[0].X, 2), "arcPoints[0].X case 2");
            Assert.AreEqual(-16.76, Round(arcPoints[1].X, 2), "arcPoints[1].X case 2");
            Assert.AreEqual(0.0, Round(arcPoints[2].X, 2), "arcPoints[2].X case 2");
            Assert.AreEqual(16.76, Round(arcPoints[3].X, 2), "arcPoints[3].X case 2");
            Assert.AreEqual(33.45, Round(arcPoints[4].X, 2), "arcPoints[4].X case 2");
        }

        [Test]
        public void IsIntersection()
        {
            Point intersection = new Point();
            Assert.False(PointExtension.IsIntersection(new Point(0, 0), new Point(0, 100), new Point(10, 0), new Point(10, 100), out intersection));

            Assert.True(PointExtension.IsIntersection(new Point(0, 0), new Point(10, 10), new Point(10, 0), new Point(0, 10), out intersection));
            Assert.AreEqual(5.0, intersection.X, "intersection.X");
            Assert.AreEqual(5.0, intersection.X, "intersection.Y");

            Assert.True(PointExtension.IsIntersection(new Point(10, 0), new Point(10, 100), new Point(0, 15), new Point(20, 15), out intersection));
            Assert.AreEqual(10.0, intersection.X, "intersection.X case 1");
            Assert.AreEqual(15.0, intersection.Y, "intersection.Y case 1");

            Assert.True(PointExtension.IsIntersection(new Point(0, 15), new Point(20, 15), new Point(10, 0), new Point(10, 100), out intersection));
            Assert.AreEqual(10.0, intersection.X, "intersection.X case 2");
            Assert.AreEqual(15.0, intersection.Y, "intersection.Y case 2");
        }

        [Test, Explicit, Apartment(System.Threading.ApartmentState.STA)]
        public void OffsetWithArcs_Show()
        {
            var s = "-5.5,-5.5 0,-5.5 5.5,-5.5 11,-5.5 16.5,-5.5 21.9,-5.5 27.4,-5.5 32.9,-5.5 38.4,-5.5 43.9,-5.5 49.4,-5.5 54.9,-5.5 60.4,-5.5 61,-5.5 61,0 60.7,5.5 60.7,6 59.9,11.4 59.8,11.9 58.5,17.2 58.3,17.7 56.5,22.9 56.3,23.3 54,28.3 53.8,28.7 50.9,33.4 50.7,33.9 47.4,38.3 47.1,38.7 43.4,42.7 43.1,43.1 39,46.8 38.7,47.1 34.3,50.4 33.9,50.7 29.2,53.5 28.7,53.8 23.8,56.1 23.3,56.3 18.2,58.2 17.7,58.3 12.4,59.7 11.9,59.8 6.5,60.6 6,60.7 0.5,60.9 0,61 -5.5,61 -5.5,55.5 -5.5,50 -5.5,44.5 -5.5,39 -5.5,33.5 -5.5,28 -5.5,22.6 -5.5,17.1 -5.5,11.6 -5.5,6.1 -5.5,0.6 -5.5,-4.9 -5.5,-5.5";
            var points = PointExtension.ParsePoints(s);
            Assert.AreEqual(61, points.Length, "points.Length");
            var offsetPoints = points.OffsetWithArcs(45.0);
            Assert.AreEqual(202, offsetPoints.Length, "offsetPoints.Length");

            var canvas = new Canvas();
            var canvas2 = new Canvas();
            canvas2.RenderTransform = new ScaleTransform(1, -1);
            canvas.Children.Add(canvas2);
            Canvas.SetTop(canvas2, 700);
            Canvas.SetLeft(canvas2, 700);
            var original = new Polyline { Points = new PointCollection(points), Stroke = Brushes.Black, StrokeThickness = 3 };
            canvas2.Children.Add(original);
            var offset = new Polyline { Points = new PointCollection(offsetPoints), Stroke = Brushes.Blue, StrokeThickness = 3 };
            canvas2.Children.Add(offset);
            var window = new Window
            {
                Title = "Offset With Arcs",
                WindowState = WindowState.Maximized,
                Content = canvas
            }.ShowDialog();
        }
    }
}