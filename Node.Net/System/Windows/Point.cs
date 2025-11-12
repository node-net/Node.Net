#if !IS_WINDOWS
using System;

namespace System.Windows
{
    /// <summary>
    /// Represents an x- and y-coordinate pair in two-dimensional space.
    /// </summary>
    public struct Point
    {
        private double _x;
        private double _y;

        /// <summary>
        /// Gets or sets the X-coordinate value of this Point structure.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the Y-coordinate value of this Point structure.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Initializes a new instance of the Point structure.
        /// </summary>
        /// <param name="x">The X-coordinate value of the new Point structure.</param>
        /// <param name="y">The Y-coordinate value of the new Point structure.</param>
        public Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Offsets this point by the specified amounts.
        /// </summary>
        /// <param name="offsetX">The amount to offset the X coordinate.</param>
        /// <param name="offsetY">The amount to offset the Y coordinate.</param>
        public void Offset(double offsetX, double offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        /// <summary>
        /// Adds a Point structure to a Vector and returns the result as a Point structure.
        /// </summary>
        public static Point Add(Point point, Vector vector)
        {
            return new Point(point._x + vector.X, point._y + vector.Y);
        }

        /// <summary>
        /// Subtracts the specified Vector from the specified Point and returns the resulting Point.
        /// </summary>
        public static Point Subtract(Point point, Vector vector)
        {
            return new Point(point._x - vector.X, point._y - vector.Y);
        }

        /// <summary>
        /// Subtracts the specified Point from another specified Point and returns the difference as a Vector.
        /// </summary>
        public static Vector Subtract(Point point1, Point point2)
        {
            return new Vector(point1._x - point2._x, point1._y - point2._y);
        }

        /// <summary>
        /// Multiplies the specified Point structure by the specified Matrix and returns the result.
        /// </summary>
        public static Point Multiply(Point point, System.Windows.Media.Matrix matrix)
        {
            // Matrix multiplication: [x y 1] * [m11 m12 0; m21 m22 0; offsetX offsetY 1]
            // For Point * Matrix, we use: [x y 1] * matrix = [x*m11+y*m21+offsetX, x*m12+y*m22+offsetY]
            double x = point._x * matrix.M11 + point._y * matrix.M21 + matrix.OffsetX;
            double y = point._x * matrix.M12 + point._y * matrix.M22 + matrix.OffsetY;
            return new Point(x, y);
        }

        // Note: Windows Point doesn't have Divide static method
        // This method is removed to match Windows API exactly

        /// <summary>
        /// Adds a Point structure to a Vector and returns the result as a Point structure.
        /// </summary>
        public static Point operator +(Point point, Vector vector)
        {
            return Add(point, vector);
        }

        /// <summary>
        /// Subtracts the specified Vector from the specified Point and returns the resulting Point.
        /// </summary>
        public static Point operator -(Point point, Vector vector)
        {
            return Subtract(point, vector);
        }

        /// <summary>
        /// Subtracts the specified Point from another specified Point and returns the difference as a Vector.
        /// </summary>
        public static Vector operator -(Point point1, Point point2)
        {
            return Subtract(point1, point2);
        }

        /// <summary>
        /// Multiplies the specified Point structure by the specified Matrix and returns the result.
        /// </summary>
        public static Point operator *(Point point, System.Windows.Media.Matrix matrix)
        {
            return Multiply(point, matrix);
        }

        // Note: Windows Point doesn't have division operator
        // This operator is removed to match Windows API exactly

        /// <summary>
        /// Compares two Point structures for equality.
        /// </summary>
        public static bool operator ==(Point point1, Point point2)
        {
            return point1._x == point2._x && point1._y == point2._y;
        }

        /// <summary>
        /// Compares two Point structures for inequality.
        /// </summary>
        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Point structure and whether the X and Y properties of the specified Object are equal to the X and Y properties of this Point structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Point point)
            {
                return this == point;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Point structure.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y);
        }

        /// <summary>
        /// Creates a string representation of this Point structure.
        /// </summary>
        public override string ToString()
        {
            // Windows Point doesn't implement IFormattable, only ToString()
            return string.Format("({0},{1})", _x, _y);
        }
    }
}
#endif

