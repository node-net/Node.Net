using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net
{
	/// <summary>
	/// Extension methods for System.Windows.Media.Media3D.Point3D
	/// </summary>
	public static class Point3DExtension
	{
		/// <summary>
		/// Parse Point3D[] from a string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Point3D[] ParsePoints(string value)
		{
			var results = new List<Point3D>();
			var words = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (var word in words)
			{
				var svalues = word.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (svalues.Length == 2)
				{
					results.Add(new Point3D(Convert.ToSingle(svalues[0]), Convert.ToSingle(svalues[1]), 0.0));
				}
				if (svalues.Length == 3)
				{
					results.Add(new Point3D(Convert.ToSingle(svalues[0]), Convert.ToSingle(svalues[1]), Convert.ToSingle(svalues[2])));
				}
			}
			return results.ToArray();
		}
		/// <summary>
		/// Get Point[] from Point3D[]
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static Point[] Get2DPoints(this Point3D[] points)
		{
			var results = new List<Point>();
			foreach (var point in points)
			{
				results.Add(new Point(point.X, point.Y));
			}
			return results.ToArray();
		}
		/// <summary>
		/// Transform Point3D[] with a Matrix3D
		/// </summary>
		/// <param name="points"></param>
		/// <param name="matrix"></param>
		/// <returns></returns>
		public static Point3D[] Transform(this Point3D[] points, Matrix3D matrix)
		{
			var result = new List<Point3D>();
			foreach (var point in points)
			{
				result.Add(matrix.Transform(point));
			}
			return result.ToArray();
		}
	}
}
