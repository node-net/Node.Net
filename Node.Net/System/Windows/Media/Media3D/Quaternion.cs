#if !IS_WINDOWS
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents a rotation in 3-D space.
    /// </summary>
    public struct Quaternion : IFormattable
    {
        private double _x;
        private double _y;
        private double _z;
        private double _w;

        /// <summary>
        /// Gets or sets the X component of the Quaternion.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the Y component of the Quaternion.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Gets or sets the Z component of the Quaternion.
        /// </summary>
        public double Z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// Gets or sets the W component of the Quaternion.
        /// </summary>
        public double W
        {
            get => _w;
            set => _w = value;
        }

        /// <summary>
        /// Initializes a new instance of the Quaternion structure.
        /// </summary>
        /// <param name="x">The X value of the new Quaternion.</param>
        /// <param name="y">The Y value of the new Quaternion.</param>
        /// <param name="z">The Z value of the new Quaternion.</param>
        /// <param name="w">The W value of the new Quaternion.</param>
        public Quaternion(double x, double y, double z, double w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>
        /// Initializes a new instance of the Quaternion structure from the specified axis and angle.
        /// </summary>
        /// <param name="axisOfRotation">The axis of rotation.</param>
        /// <param name="angleInDegrees">The angle of rotation around the axis, in degrees.</param>
        public Quaternion(Vector3D axisOfRotation, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180.0);
            double halfAngle = angleInRadians * 0.5;
            double sinHalfAngle = Math.Sin(halfAngle);
            
            // Normalize the axis
            double length = axisOfRotation.Length;
            if (length == 0)
            {
                _x = 0;
                _y = 0;
                _z = 0;
                _w = 1;
            }
            else
            {
                _x = axisOfRotation.X / length * sinHalfAngle;
                _y = axisOfRotation.Y / length * sinHalfAngle;
                _z = axisOfRotation.Z / length * sinHalfAngle;
                _w = Math.Cos(halfAngle);
            }
        }

        /// <summary>
        /// Gets the length of the quaternion.
        /// </summary>
        public double Length => Math.Sqrt(LengthSquared);

        /// <summary>
        /// Gets the square of the length of the quaternion.
        /// </summary>
        public double LengthSquared => (_x * _x) + (_y * _y) + (_z * _z) + (_w * _w);

        /// <summary>
        /// Normalizes the quaternion.
        /// </summary>
        public void Normalize()
        {
            double length = Length;
            if (length == 0)
            {
                throw new InvalidOperationException("Cannot normalize a zero quaternion.");
            }
            _x /= length;
            _y /= length;
            _z /= length;
            _w /= length;
        }

        /// <summary>
        /// Gets the conjugate of the quaternion.
        /// </summary>
        public Quaternion Conjugate()
        {
            return new Quaternion(-_x, -_y, -_z, _w);
        }

        /// <summary>
        /// Gets the inverse of the quaternion.
        /// </summary>
        public Quaternion Invert()
        {
            double lengthSquared = LengthSquared;
            if (lengthSquared == 0)
            {
                throw new InvalidOperationException("Cannot invert a zero quaternion.");
            }
            Quaternion conjugate = Conjugate();
            return new Quaternion(
                conjugate._x / lengthSquared,
                conjugate._y / lengthSquared,
                conjugate._z / lengthSquared,
                conjugate._w / lengthSquared
            );
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            return new Quaternion(
                (left._w * right._x) + (left._x * right._w) + (left._y * right._z) - (left._z * right._y),
                (left._w * right._y) - (left._x * right._z) + (left._y * right._w) + (left._z * right._x),
                (left._w * right._z) + (left._x * right._y) - (left._y * right._x) + (left._z * right._w),
                (left._w * right._w) - (left._x * right._x) - (left._y * right._y) - (left._z * right._z)
            );
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return Multiply(left, right);
        }

        /// <summary>
        /// Compares two Quaternion structures for equality.
        /// </summary>
        public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2)
        {
            return quaternion1._x == quaternion2._x &&
                   quaternion1._y == quaternion2._y &&
                   quaternion1._z == quaternion2._z &&
                   quaternion1._w == quaternion2._w;
        }

        /// <summary>
        /// Compares two Quaternion structures for inequality.
        /// </summary>
        public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2)
        {
            return !(quaternion1 == quaternion2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Quaternion structure and whether the X, Y, Z, and W properties of the specified Object are equal to the X, Y, Z, and W properties of this Quaternion structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Quaternion quaternion)
            {
                return this == quaternion;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Quaternion structure.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y, _z, _w);
        }

        /// <summary>
        /// Creates a string representation of this Quaternion structure.
        /// </summary>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Creates a string representation of this Quaternion structure.
        /// </summary>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }
            formatProvider ??= System.Globalization.CultureInfo.CurrentCulture;
            return string.Format(formatProvider, "({0},{1},{2},{3})", 
                _x.ToString(format, formatProvider), 
                _y.ToString(format, formatProvider), 
                _z.ToString(format, formatProvider),
                _w.ToString(format, formatProvider));
        }
    }
}
#endif

