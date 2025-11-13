using System;

namespace System.Windows.Media.Media3D
{
#if !IS_WINDOWS
    /// <summary>
    /// Represents a displacement in 3-D space.
    /// </summary>
    public struct Vector3D
    {
        private double _x;
        private double _y;
        private double _z;

        /// <summary>
        /// Gets or sets the X component of this Vector3D structure.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the Y component of this Vector3D structure.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Gets or sets the Z component of this Vector3D structure.
        /// </summary>
        public double Z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// Initializes a new instance of the Vector3D structure.
        /// </summary>
        /// <param name="x">The X component of the new Vector3D structure.</param>
        /// <param name="y">The Y component of the new Vector3D structure.</param>
        /// <param name="z">The Z component of the new Vector3D structure.</param>
        public Vector3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Gets the length of this Vector3D.
        /// </summary>
        public double Length => Math.Sqrt(LengthSquared);

        /// <summary>
        /// Gets the square of the length of this Vector3D.
        /// </summary>
        public double LengthSquared => (_x * _x) + (_y * _y) + (_z * _z);

        /// <summary>
        /// Normalizes the specified Vector3D structure.
        /// </summary>
        public void Normalize()
        {
            double length = Length;
            // Windows Vector3D sets components to NaN when normalizing zero vector, doesn't throw
            if (length == 0)
            {
                _x = double.NaN;
                _y = double.NaN;
                _z = double.NaN;
            }
            else
            {
                _x /= length;
                _y /= length;
                _z /= length;
            }
        }

        /// <summary>
        /// Negates the specified Vector3D structure.
        /// </summary>
        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
        }

        /// <summary>
        /// Adds two Vector3D structures and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D Add(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1._x + vector2._x, vector1._y + vector2._y, vector1._z + vector2._z);
        }

        /// <summary>
        /// Subtracts a Vector3D structure from a Vector3D structure.
        /// </summary>
        public static Vector3D Subtract(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1._x - vector2._x, vector1._y - vector2._y, vector1._z - vector2._z);
        }

        /// <summary>
        /// Multiplies the specified Vector3D structure by the specified scalar and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D Multiply(Vector3D vector, double scalar)
        {
            return new Vector3D(vector._x * scalar, vector._y * scalar, vector._z * scalar);
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified Vector3D structure and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D Multiply(double scalar, Vector3D vector)
        {
            return Multiply(vector, scalar);
        }

        /// <summary>
        /// Divides the specified Vector3D structure by the specified scalar and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D Divide(Vector3D vector, double scalar)
        {
            // Windows Vector3D returns Infinity/NaN on divide by zero, doesn't throw
            return new Vector3D(vector._x / scalar, vector._y / scalar, vector._z / scalar);
        }

        /// <summary>
        /// Calculates the dot product of two Vector3D structures.
        /// </summary>
        public static double DotProduct(Vector3D vector1, Vector3D vector2)
        {
            return (vector1._x * vector2._x) + (vector1._y * vector2._y) + (vector1._z * vector2._z);
        }

        /// <summary>
        /// Calculates the cross product of two Vector3D structures.
        /// </summary>
        public static Vector3D CrossProduct(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(
                (vector1._y * vector2._z) - (vector1._z * vector2._y),
                (vector1._z * vector2._x) - (vector1._x * vector2._z),
                (vector1._x * vector2._y) - (vector1._y * vector2._x)
            );
        }

        /// <summary>
        /// Retrieves the angle required to rotate the first specified Vector3D structure into the second specified Vector3D structure.
        /// </summary>
        public static double AngleBetween(Vector3D vector1, Vector3D vector2)
        {
            double length1 = vector1.Length;
            double length2 = vector2.Length;
            
            // Windows Vector3D returns NaN when one vector is zero
            if (length1 == 0 || length2 == 0)
            {
                return double.NaN;
            }
            
            double dotProduct = DotProduct(vector1, vector2) / (length1 * length2);
            // Clamp to [-1, 1] to avoid numerical errors
            dotProduct = Math.Max(-1.0, Math.Min(1.0, dotProduct));
            return Math.Acos(dotProduct) * (180.0 / Math.PI);
        }

        /// <summary>
        /// Adds two Vector3D structures and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
        {
            return Add(vector1, vector2);
        }

        /// <summary>
        /// Subtracts a Vector3D structure from a Vector3D structure.
        /// </summary>
        public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
        {
            return Subtract(vector1, vector2);
        }

        /// <summary>
        /// Multiplies the specified Vector3D structure by the specified scalar and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D operator *(Vector3D vector, double scalar)
        {
            return Multiply(vector, scalar);
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified Vector3D structure and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D operator *(double scalar, Vector3D vector)
        {
            return Multiply(scalar, vector);
        }

        /// <summary>
        /// Divides the specified Vector3D structure by the specified scalar and returns the result as a Vector3D.
        /// </summary>
        public static Vector3D operator /(Vector3D vector, double scalar)
        {
            return Divide(vector, scalar);
        }

        /// <summary>
        /// Negates the specified Vector3D structure.
        /// </summary>
        public static Vector3D operator -(Vector3D vector)
        {
            return new Vector3D(-vector._x, -vector._y, -vector._z);
        }

        /// <summary>
        /// Compares two Vector3D structures for equality.
        /// </summary>
        public static bool operator ==(Vector3D vector1, Vector3D vector2)
        {
            return vector1._x == vector2._x && vector1._y == vector2._y && vector1._z == vector2._z;
        }

        /// <summary>
        /// Compares two Vector3D structures for inequality.
        /// </summary>
        public static bool operator !=(Vector3D vector1, Vector3D vector2)
        {
            return !(vector1 == vector2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Vector3D structure and whether the X, Y, and Z properties of the specified Object are equal to the X, Y, and Z properties of this Vector3D structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Vector3D vector)
            {
                return this == vector;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Vector3D structure.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y, _z);
        }

        /// <summary>
        /// Creates a string representation of this Vector3D structure.
        /// </summary>
        public override string ToString()
        {
            // Windows Vector3D doesn't implement IFormattable, only ToString()
            return string.Format("({0},{1},{2})", _x, _y, _z);
        }

        /// <summary>
        /// Strips parentheses from a Vector3D string representation to support both "x,y,z" and "(x,y,z)" formats.
        /// </summary>
        private static string StripParentheses(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            string trimmedValue = value.Trim();
            if (trimmedValue.StartsWith("(") && trimmedValue.EndsWith(")"))
            {
                return trimmedValue.Substring(1, trimmedValue.Length - 2).Trim();
            }

            return trimmedValue;
        }

        /// <summary>
        /// Parses a string representation of a Vector3D (e.g., "1,0,0" or "(1,0,0)").
        /// Supports both formats with and without parentheses.
        /// </summary>
        public static Vector3D Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            }

            // Strip parentheses if present to support both "x,y,z" and "(x,y,z)" formats
            string trimmedValue = StripParentheses(value);

            string[] parts = trimmedValue.Split(',');
            if (parts.Length != 3)
            {
                throw new FormatException($"Invalid Vector3D format. Expected 'x,y,z' or '(x,y,z)' but got '{value}'.");
            }

            if (double.TryParse(parts[0].Trim(), out double x) &&
                double.TryParse(parts[1].Trim(), out double y) &&
                double.TryParse(parts[2].Trim(), out double z))
            {
                return new Vector3D(x, y, z);
            }

            throw new FormatException($"Invalid Vector3D format. Could not parse '{value}'.");
        }
    }
#endif
}

