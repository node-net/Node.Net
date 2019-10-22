using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Node.Net.Deprecated.Collections
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
        public static string ToString(Point[] points)
        {
            var builder = new StringBuilder();
            foreach (var point in points)
            {
                if (builder.Length > 0) builder.Append(' ');
                builder.Append($"{point.X},{point.Y}");
            }
            return builder.ToString();
        }
    }
}
