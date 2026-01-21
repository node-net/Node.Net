#if !IS_WINDOWS
using System;

namespace System.Windows
{
    /// <summary>
    /// Represents a displacement in 2-D space.
    /// </summary>
    public struct Vector
    {
        private double _x;
        private double _y;

        /// <summary>
        /// Gets or sets the X component of this vector.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the Y component of this vector.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Initializes a new instance of the Vector structure.
        /// </summary>
        /// <param name="x">The X component of the new Vector structure.</param>
        /// <param name="y">The Y component of the new Vector structure.</param>
        public Vector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Gets the length of this vector.
        /// </summary>
        public double Length => Math.Sqrt(LengthSquared);

        /// <summary>
        /// Gets the square of the length of this vector.
        /// </summary>
        public double LengthSquared => (_x * _x) + (_y * _y);

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            double length = Length;
            // Windows Vector sets components to NaN when normalizing zero vector, doesn't throw
            if (length == 0)
            {
                _x = double.NaN;
                _y = double.NaN;
            }
            else
            {
                _x /= length;
                _y /= length;
            }
        }

        /// <summary>
        /// Negates this vector.
        /// </summary>
        public void Negate()
        {
            _x = -_x;
            _y = -_y;
        }

        /// <summary>
        /// Adds two vectors and returns the result as a Vector.
        /// </summary>
        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        /// <summary>
        /// Subtracts the specified vector from another specified vector.
        /// </summary>
        public static Vector Subtract(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        /// <summary>
        /// Multiplies the specified vector by the specified scalar and returns the result as a Vector.
        /// </summary>
        public static Vector Multiply(Vector vector, double scalar)
        {
            return new Vector(vector._x * scalar, vector._y * scalar);
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified vector and returns the result as a Vector.
        /// </summary>
        public static Vector Multiply(double scalar, Vector vector)
        {
            return Multiply(vector, scalar);
        }

        /// <summary>
        /// Multiplies the specified Vector by the specified Matrix and returns the result.
        /// </summary>
        public static Vector Multiply(Vector vector, System.Windows.Media.Matrix matrix)
        {
            // Vector transformation: [x y] * [m11 m12; m21 m22] = [x*m11+y*m21, x*m12+y*m22]
            double x = vector.X * matrix.M11 + vector.Y * matrix.M21;
            double y = vector.X * matrix.M12 + vector.Y * matrix.M22;
            return new Vector(x, y);
        }

        /// <summary>
        /// Divides the specified vector by the specified scalar and returns the result as a Vector.
        /// </summary>
        public static Vector Divide(Vector vector, double scalar)
        {
            // Windows Vector returns Infinity/NaN on divide by zero, doesn't throw
            return new Vector(vector._x / scalar, vector._y / scalar);
        }

        // Note: Windows Vector doesn't have DotProduct static method
        // This method is removed to match Windows API exactly

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        public static double CrossProduct(Vector vector1, Vector vector2)
        {
            return (vector1._x * vector2._y) - (vector1._y * vector2._x);
        }

        /// <summary>
        /// Retrieves the angle, expressed in degrees, between the two specified vectors.
        /// </summary>
        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            double length1 = vector1.Length;
            double length2 = vector2.Length;

            // Windows Vector returns 0.0 when one vector is zero, not NaN
            if (length1 == 0 || length2 == 0)
            {
                return 0.0;
            }

            double dotProduct = (vector1._x * vector2._x + vector1._y * vector2._y) / (length1 * length2);
            // Clamp to [-1, 1] to avoid numerical errors
            dotProduct = Math.Max(-1.0, Math.Min(1.0, dotProduct));
            return Math.Acos(dotProduct) * (180.0 / Math.PI);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return Add(vector1, vector2);
        }

        /// <summary>
        /// Subtracts one vector from another.
        /// </summary>
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return Subtract(vector1, vector2);
        }

        /// <summary>
        /// Multiplies the specified vector by the specified scalar.
        /// </summary>
        public static Vector operator *(Vector vector, double scalar)
        {
            return Multiply(vector, scalar);
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified vector.
        /// </summary>
        public static Vector operator *(double scalar, Vector vector)
        {
            return Multiply(scalar, vector);
        }

        /// <summary>
        /// Divides the specified vector by the specified scalar.
        /// </summary>
        public static Vector operator /(Vector vector, double scalar)
        {
            return Divide(vector, scalar);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        public static Vector operator -(Vector vector)
        {
            return new Vector(-vector._x, -vector._y);
        }

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        public static bool operator ==(Vector vector1, Vector vector2)
        {
            return vector1._x == vector2._x && vector1._y == vector2._y;
        }

        /// <summary>
        /// Compares two vectors for inequality.
        /// </summary>
        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Vector structure and whether the X and Y properties of the specified Object are equal to the X and Y properties of this Vector structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Vector vector)
            {
                return this == vector;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Vector structure.
        /// </summary>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            return ((_x.GetHashCode() * 397) ^ _y.GetHashCode());
#else
            return HashCode.Combine(_x, _y);
#endif
        }

        /// <summary>
        /// Creates a string representation of this Vector structure.
        /// </summary>
        public override string ToString()
        {
            // Windows Vector doesn't implement IFormattable, only ToString()
            return string.Format("({0},{1})", _x, _y);
        }
    }
}
#endif

