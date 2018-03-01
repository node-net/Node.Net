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

        public static string GetString(this Point[] points, int decimals = 1)
        {
            var builder = new StringBuilder();
            foreach (var point in points)
            {
                if (builder.Length > 0) builder.Append(' ');
                builder.Append($"{Round(point.X, decimals)},{Round(point.Y, decimals)}");
            }
            return builder.ToString();
        }

        public static bool IsClosed(this Point[] points, double tolerance = 0.0001)
        {
            if (points.Length < 3) return false;
            var delta = Point.Subtract(points[points.Length - 1], points[0]);
            if (delta.Length < tolerance) return true;
            return false;
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

        public static Point[] Open(this Point[] points, double tolerance = 0.0001)
        {
            if (points.Length < 2) return points;
            var delta = Point.Subtract(points[points.Length - 1], points[0]);
            if (delta.Length < tolerance)
            {
                var result = new List<Point>();
                for (int i = 0; i < points.Length - 1; ++i)
                {
                    result.Add(points[i]);
                }
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

        public static Point[] Move(this Point[] points, Point offset)
        {
            var results = new List<Point>();
            foreach (var point in points)
            {
                results.Add(new Point(point.X + offset.X, point.Y + offset.Y));
            }
            return results.ToArray();
        }

        public static Point[] Offset(this Point[] points, double distance)
        {
            var offsetPointList = new List<Point>();
            var segmentCount = points.Length - 1;
            for (int si = 0; si < segmentCount; ++si)
            {
                var pointA = points[si];
                var pointB = points[si + 1];
                Vector tangent = (pointB - pointA);
                tangent.Normalize();
                var matrix = new System.Windows.Media.Matrix();
                matrix.Rotate(-90.0);
                var normal = Vector.Multiply(tangent, matrix);
                var offset = new Vector(normal.X * distance, normal.Y * distance);

                offsetPointList.Add(new Point(pointA.X + offset.X, pointA.Y + offset.Y));
                offsetPointList.Add(new Point(pointB.X + offset.X, pointB.Y + offset.Y));
            }

            var offsetPoints = offsetPointList.ToArray();
            if (points.IsClosed()) offsetPoints = offsetPoints.Close();
            return offsetPoints;
        }

        public static Point[] OffsetWithArcs(this Point[] points,double distance)
        {
            var offsetPointList = new List<Point>();
            var segmentCount = points.Length - 1;
            Point offsetPointA = new Point();
            Point offsetPointB = new Point();
            Point offsetPointC = new Point();
            Point offsetPointD = new Point();

            Point originalOffsetA = new Point();
            Point originalOffsetB = new Point();
            for (int si = 0; si < segmentCount; ++si)
            {
                var pointA = points[si];
                var pointB = points[si + 1];
                Vector tangent = (pointB - pointA);
                tangent.Normalize();
                var matrix = new System.Windows.Media.Matrix();
                matrix.Rotate(-90.0);
                var normal = Vector.Multiply(tangent, matrix);
                var offset = new Vector(normal.X * distance, normal.Y * distance);

                offsetPointC = new Point(pointA.X + offset.X, pointA.Y + offset.Y);
                offsetPointD = new Point(pointB.X + offset.X, pointB.Y + offset.Y);

                if (si > 0)
                {
                    var arc_points = GetConnectingArcPoints(offsetPointA, offsetPointB, offsetPointC, offsetPointD);
                    if(arc_points.Count > 0)
                    {
                        foreach (var arc_point in arc_points) offsetPointList.Add(arc_point);
                    }
                }

                offsetPointList.Add(offsetPointC);
                offsetPointList.Add(offsetPointD);

                offsetPointA = offsetPointC;
                offsetPointB = offsetPointD;

                if(si == 0)
                {
                    originalOffsetA = offsetPointA;
                    originalOffsetB = offsetPointB;
                }
            }

            var offsetPoints = offsetPointList.ToArray();
            if (points.IsClosed())
            {
                var arc_points = GetConnectingArcPoints(offsetPointC, offsetPointD, originalOffsetA, originalOffsetB);
                if (arc_points.Count > 0)
                {
                    foreach (var arc_point in arc_points) offsetPointList.Add(arc_point);
                }

                offsetPoints = offsetPoints.Close();
            }
            return offsetPoints;
        }

        public static Point[] Offset_Old(this Point[] points, double distance)
        {
            var centroid = points.GetCentroid();

            var centeredPoints = points.Move(new Point(-centroid.X, -centroid.Y));

            var dims = points.GetDimensions();
            var scaleX = (dims.X / 2.0 + distance) / (dims.X / 2.0);
            var scaleY = (dims.Y / 2.0 + distance) / (dims.Y / 2.0);
            var scaleTransform = new ScaleTransform(scaleX, scaleY);

            var resultA = new List<Point>();
            var pointFs = new List<System.Drawing.PointF>();
            foreach (var point in centeredPoints)
            {
                resultA.Add(scaleTransform.Transform(point));
            }

            return resultA.ToArray().Move(centroid);
        }

        public static Point[] Scale(this Point[] points, double scale)
        {
            var dims = points.GetDimensions();
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

        public static bool Contains(this Point[] polygon, Point point, double epsilon = 0.00001)
        {
            if (polygon == null) return false;
            int polygonLength = polygon.Length, i = 0;
            bool inside = false;
            double pointX = point.X, pointY = point.Y;
            double startX, startY, endX, endY;
            Point endPoint = polygon[polygonLength - 1];
            endX = endPoint.X;
            endY = endPoint.Y;
            while (i < polygonLength)
            {
                startX = endX; startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.X; endY = endPoint.Y;
                inside ^= (endY >= pointY ^ startY >= pointY) /* ? pointY inside [startY;endY] segment ? */
                          && /* if so, test if it is under the segment */
                          ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
                //if (Abs(point.X - startX) < tolerance && Abs(point.Y - startY) < tolerance) return true;
            }

            if (!inside)
            {
                // test if point is on outline
                var closed = polygon.Close(epsilon);
                if (closed.IsPointOnPolyline(point)) return true;
            }
            return inside;
        }

        public static bool IsPointOnPolyline(this Point[] points, Point point, double epsilon = 0.00001)
        {
            for (int i = 1; i < points.Length; ++i)
            {
                var pointA = points[i - 1];
                var pointB = points[i];
                if (IsPointOnLine(pointA, pointB, point, epsilon)) return true;
            }
            return false;
        }

        public static bool IsPointOnLine(Point pointA, Point pointB, Point testPoint, double epsilon = 0.00001)
        {
            if (Abs(pointB.X - pointA.X) < epsilon)
            {
                // vertical line
                if (Abs(testPoint.X - pointA.X) < epsilon)
                {
                    if (Abs(testPoint.Y - pointA.Y) < epsilon) return true;
                    if (Abs(testPoint.Y - pointB.Y) < epsilon) return true;
                    if (testPoint.Y < pointB.Y && testPoint.Y > pointA.Y) return true;
                    if (testPoint.Y > pointB.Y && testPoint.Y < pointA.Y) return true;
                }
            }
            else
            {
                var a = (pointB.Y - pointA.Y) / (pointB.X - pointA.X);
                var b = pointA.Y - a * pointA.X;
                if (Abs(testPoint.Y - (a * testPoint.X + b)) < epsilon) return true;
            }
            return false;
        }

        public static List<Point[]> Subdivide(this Point[] points, Point origin, double deltaX, double deltaY)
        {
            return null;
        }

        public static List<Point> GetConnectingArcPoints(Point pointA, Point pointB, Point pointC, Point pointD)
        {
            var results = new List<Point>();
            Point intersection = new Point();
            if (!IsIntersection(pointA, pointB, pointC, pointD, out intersection)) return results;

            Vector tangent1 = (pointB - pointA);
            tangent1.Normalize();
            var matrix = new System.Windows.Media.Matrix();
            matrix.Rotate(90.0);
            var negativenormal1 = Vector.Multiply(tangent1, matrix);
            Vector tangent2 = (pointD - pointC);
            tangent2.Normalize();
            var negativenormal2 = Vector.Multiply(tangent2, matrix);

            Point arc_origin = new Point();
            if(IsIntersection(pointB,new Point(pointB.X + negativenormal1.X,pointB.Y + negativenormal1.Y),
                              pointC,new Point(pointC.X + negativenormal2.X,pointC.Y + negativenormal2.Y),out arc_origin))
            {
                var angle = Vector.AngleBetween(pointB - arc_origin, pointC - arc_origin);
                if(Abs(angle) > 5.0)
                {
                    var theta_div = Round(Abs(angle) / 5.0, 0) + 1;
                    if(theta_div > 1)
                    {
                        var delta = angle / ((double)theta_div);
                        for(double i =1; i < theta_div;++i)
                        {
                            var rotationMatrix = new System.Windows.Media.Matrix();
                            rotationMatrix.Rotate(delta * i);
                            var arc_vector = pointB - arc_origin;
                            arc_vector = Vector.Multiply(arc_vector, rotationMatrix);
                            results.Add(new Point(arc_origin.X + arc_vector.X,arc_origin.Y + arc_vector.Y));
                        }
                    }
                }
            }
            
            return results;
        }
        public static double GetSlope(this Point pointA,Point pointB)
        {
            return (pointB.Y - pointA.Y) / (pointB.X - pointA.X);
        }
        public static double GetA(this Point pointA,Point pointB) { return -1.0 * GetSlope(pointA, pointB); }
        public static double GetC(this Point pointA,Point pointB) { return pointA.Y - GetSlope(pointA, pointB) * pointA.X; }
        public static bool IsVertical(this Point pointA,Point pointB,double tolerance = 0.0001)
        {
            return Abs(pointB.X-pointA.X) < tolerance;
        }
        public static bool IsIntersection(Point pointA,Point pointB,Point pointC,Point pointD,out Point intersectionPoint)
        {
            intersectionPoint = new Point();
            if (pointA.IsVertical(pointB) && pointC.IsVertical(pointD)) return false;
            if(pointA.IsVertical(pointB))
            {
                var y = (pointA.X - pointC.X) * (pointD.Y - pointC.Y) / ((pointD.X - pointC.X)) + pointC.Y;
                intersectionPoint = new Point(pointA.X, y);
                return true;
            }
            if(pointC.IsVertical(pointD))
            {
                var y = (pointC.X - pointA.X) * (pointB.Y - pointA.Y) / ((pointB.X - pointA.X)) + pointA.Y;
                intersectionPoint = new Point(pointC.X, y);
                return true;
            }
            var A1 = pointA.GetA(pointB);
            var A2 = pointC.GetA(pointD);
            var B1 = 1.0;
            var B2 = 1.0;
            var C1 = pointA.GetC(pointB);
            var C2 = pointC.GetC(pointD);
            double delta = A1 * B2 - A2 * B1;
            bool hasIntersection = Abs(delta - 0) > 0.0001f;
            if (hasIntersection)
            {
                double x = (B2 * C1 - B1 * C2) / delta;
                double y = (A1 * C2 - A2 * C1) / delta;
                intersectionPoint = new Point(x, y);
            }
            return hasIntersection;
        }
    }
}