using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
					area +=
						(pts[i + 1].X - pts[i].X) *
						(pts[i + 1].Y + pts[i].Y) / 2;
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
	}
}