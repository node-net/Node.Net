#if !IS_WINDOWS
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents an x-, y-, and z-coordinate point in 3-D space.
    /// </summary>
    public struct Point3D : IFormattable
    {
        private double _x;
        private double _y;
        private double _z;

        /// <summary>
        /// Gets or sets the X coordinate of this Point3D structure.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the Y coordinate of this Point3D structure.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Gets or sets the Z coordinate of this Point3D structure.
        /// </summary>
        public double Z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// Initializes a new instance of the Point3D structure.
        /// </summary>
        /// <param name="x">The X coordinate of the new Point3D structure.</param>
        /// <param name="y">The Y coordinate of the new Point3D structure.</param>
        /// <param name="z">The Z coordinate of the new Point3D structure.</param>
        public Point3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Offsets the Point3D structure by the specified Vector3D structure.
        /// </summary>
        /// <param name="offsetVector">The Vector3D structure that offsets this Point3D structure.</param>
        public void Offset(Vector3D offsetVector)
        {
            _x += offsetVector.X;
            _y += offsetVector.Y;
            _z += offsetVector.Z;
        }

        /// <summary>
        /// Offsets the Point3D structure by the specified amounts.
        /// </summary>
        /// <param name="offsetX">The amount to offset the X coordinate.</param>
        /// <param name="offsetY">The amount to offset the Y coordinate.</param>
        /// <param name="offsetZ">The amount to offset the Z coordinate.</param>
        public void Offset(double offsetX, double offsetY, double offsetZ)
        {
            _x += offsetX;
            _y += offsetY;
            _z += offsetZ;
        }

        /// <summary>
        /// Adds a Point3D structure to a Vector3D and returns the result as a Point3D structure.
        /// </summary>
        public static Point3D Add(Point3D point, Vector3D vector)
        {
            return new Point3D(point._x + vector.X, point._y + vector.Y, point._z + vector.Z);
        }

        /// <summary>
        /// Subtracts a Vector3D from a Point3D and returns the result as a Point3D structure.
        /// </summary>
        public static Point3D Subtract(Point3D point, Vector3D vector)
        {
            return new Point3D(point._x - vector.X, point._y - vector.Y, point._z - vector.Z);
        }

        /// <summary>
        /// Subtracts a Point3D structure from a Point3D structure and returns the result as a Vector3D structure.
        /// </summary>
        public static Vector3D Subtract(Point3D point1, Point3D point2)
        {
            return new Vector3D(point1._x - point2._x, point1._y - point2._y, point1._z - point2._z);
        }

        /// <summary>
        /// Transforms the specified Point3D structure by the specified Matrix3D structure.
        /// </summary>
        public static Point3D Multiply(Point3D point, Matrix3D matrix)
        {
            return matrix.Transform(point);
        }

        /// <summary>
        /// Adds a Point3D structure to a Vector3D and returns the result as a Point3D structure.
        /// </summary>
        public static Point3D operator +(Point3D point, Vector3D vector)
        {
            return Add(point, vector);
        }

        /// <summary>
        /// Subtracts a Vector3D from a Point3D and returns the result as a Point3D structure.
        /// </summary>
        public static Point3D operator -(Point3D point, Vector3D vector)
        {
            return Subtract(point, vector);
        }

        /// <summary>
        /// Subtracts a Point3D structure from a Point3D structure and returns the result as a Vector3D structure.
        /// </summary>
        public static Vector3D operator -(Point3D point1, Point3D point2)
        {
            return Subtract(point1, point2);
        }

        /// <summary>
        /// Transforms the specified Point3D structure by the specified Matrix3D structure.
        /// </summary>
        public static Point3D operator *(Point3D point, Matrix3D matrix)
        {
            return Multiply(point, matrix);
        }

        /// <summary>
        /// Compares two Point3D structures for equality.
        /// </summary>
        public static bool operator ==(Point3D point1, Point3D point2)
        {
            return point1._x == point2._x && point1._y == point2._y && point1._z == point2._z;
        }

        /// <summary>
        /// Compares two Point3D structures for inequality.
        /// </summary>
        public static bool operator !=(Point3D point1, Point3D point2)
        {
            return !(point1 == point2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Point3D structure and whether the X, Y, and Z properties of the specified Object are equal to the X, Y, and Z properties of this Point3D structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Point3D point)
            {
                return this == point;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Point3D structure.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y, _z);
        }

        /// <summary>
        /// Creates a string representation of this Point3D structure.
        /// </summary>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Creates a string representation of this Point3D structure.
        /// </summary>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }
            formatProvider ??= System.Globalization.CultureInfo.CurrentCulture;
            return string.Format(formatProvider, "({0},{1},{2})", 
                _x.ToString(format, formatProvider), 
                _y.ToString(format, formatProvider), 
                _z.ToString(format, formatProvider));
        }
    }
}
#endif

