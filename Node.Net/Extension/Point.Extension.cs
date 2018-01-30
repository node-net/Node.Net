using System;
using System.Collections.Generic;

//using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static System.Math;

namespace Node.Net
{
	public static class PointExtension
	{
		public static Point[] GetPointArray(string value)
		{
			var points = new List<Point>();
			var words = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (var word in words)
			{
				if (word.Contains(','))
				{
					var parts = word.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length == 2)
					{
						points.Add(new Point(Convert.ToDouble(parts[0]), Convert.ToDouble(parts[1])));
					}
				}
			}
			return points.ToArray();
		}

		public static string GetString(this Point[] points)
		{
			var builder = new StringBuilder();
			foreach (var point in points)
			{
				if (builder.Length > 0) builder.Append(' ');
				builder.Append($"{point.X},{point.Y}");
			}
			return builder.ToString();
		}

		public static Point[] Close(this Point[] points, double tolerance = 0.0001)
		{
			if (points.Length < 2) return points;
			var delta = Point.Subtract(points[points.Length - 1], points[0]);
			if (delta.Length > tolerance)
			{
				var result = new List<Point>(points);
				result.Add(points[0]);
				return result.ToArray();
			}
			return points;
		}

		public static double GetLength(this Point[] points)
		{
			if (points.Length < 2) return 0.0;
			var length = 0.0;
			for (int i = 1; i < points.Length; ++i)
			{
				length += Point.Subtract(points[i], points[i - 1]).Length;
			}
			return length;
		}

		public static Point GetPointAtDistance(this Point[] points, double distance)
		{
			if (points.Length > 1 && distance >= 0.0)
			{
				var totalLength = points.GetLength();
				if (distance < totalLength)
				{
					var lastLength = 0.0;
					var lengthToI = 0.0;
					for (int i = 1; i < points.Length; ++i)
					{
						var delta = Point.Subtract(points[i], points[i - 1]);
						lengthToI += delta.Length;
						if (lengthToI == distance) return points[i];
						if (lengthToI > distance)
						{
							return points[i - 1] + delta * ((distance - lastLength) / delta.Length);
						}
						lastLength = lengthToI;
					}
				}
			}
			if (points.Length > 0) return points[points.Length - 1];
			return new Point(0, 0);
		}

		public static Point[] GetPointsAtInterval(this Point[] points, double interval)
		{
			var results = new List<Point>();
			if (points.Length > 1)
			{
				var totalLength = points.GetLength();
				results.Add(points[0]);
				for (double distance = interval; distance <= totalLength; distance += interval)
				{
					results.Add(points.GetPointAtDistance(distance));
				}
			}
			return results.ToArray();
		}

		public static double GetArea(this Point[] points)
		{
			if (points.Length > 2)
			{
				Point[] pts = new Point[points.Length + 1];
				points.CopyTo(pts, 0);
				pts[points.Length] = points[0];

				double area = 0.0;
				for (int i = 0; i < points.Length; i++)
				{
					var segment_area = (pts[i + 1].X - pts[i].X) *
						(pts[i + 1].Y + pts[i].Y) / 2;
					area += segment_area;
				}

				return Abs(area);
			}
			return 0.0;
		}

		public static Point[] ParsePoints(string value)
		{
			var results = new List<Point>();
			var words = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (var word in words)
			{
				var svalues = word.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (svalues.Length == 2)
				{
					results.Add(new Point(Convert.ToSingle(svalues[0]), Convert.ToSingle(svalues[1])));
				}
			}
			return results.ToArray();
		}

		public static Point GetCentroid(this Point[] points, double tolerance = 0.0001)
		{
			var pts = points.Close(tolerance);
			int num_points = pts.Length - 1;

			var X = 0.0;
			var Y = 0.0;
			double second_factor;
			for (int i = 0; i < num_points; i++)
			{
				second_factor =
					pts[i].X * pts[i + 1].Y -
					pts[i + 1].X * pts[i].Y;
				X += (pts[i].X + pts[i + 1].X) * second_factor;
				Y += (pts[i].Y + pts[i + 1].Y) * second_factor;
			}

			var polygon_area = pts.GetArea();
			X /= (6 * polygon_area);
			Y /= (6 * polygon_area);

			if (X < 0)
			{
				X = -X;
				Y = -Y;
			}

			return new Point(X, Y);
		}

		public static Point GetDimensions(this Point[] points)
		{
			if (points.Length > 0)
			{
				var min = points[0];
				var max = points[0];
				foreach (var point in points)
				{
					if (point.X < min.X) min.X = point.X;
					if (point.Y < min.Y) min.Y = point.Y;
					if (point.X > max.X) max.X = point.X;
					if (point.Y > max.Y) max.Y = point.Y;
				}
				return new Point(max.X - min.X, max.Y - min.Y);
			}
			return new Point(0, 0);
		}

		public static Point[] Offset(this Point[] points, double distance)
		{
			var dims = points.GetDimensions();
			var scale = (dims.X / 2.0 + distance) / (dims.X / 2.0);
			var scaleTransform = new ScaleTransform(scale, scale);

			var result = new List<Point>();
			var pointFs = new List<System.Drawing.PointF>();
			foreach (var point in points)
			{
				result.Add(scaleTransform.Transform(point));
			}
			return result.ToArray();
		}

		public static Point[] Reverse(this Point[] points)
		{
			var list = new List<Point>(points);
			list.Reverse();
			return list.ToArray();
		}

		public static List<Point[]> Subdivide(this Point[] points, Point origin, double deltaX, double deltaY)
		{
			return null;
		}
	}
}