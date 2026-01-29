#if !IS_WINDOWS && USE_POLYFILL
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents a rotation in 3-D space.
    /// </summary>
    public struct Quaternion
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
        public Quaternion()
        {
            // Windows Quaternion default constructor creates identity quaternion (0,0,0,1)
            _x = 0;
            _y = 0;
            _z = 0;
            _w = 1;
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
            // Windows WPF negates the angle
            double angleInRadians = -angleInDegrees * (Math.PI / 180.0);
            double halfAngle = angleInRadians * 0.5;
            double sinHalfAngle = Math.Sin(halfAngle);
            
            // Normalize the axis
            double length = axisOfRotation.Length;
            // Windows Quaternion throws InvalidOperationException when axis is zero
            if (length == 0)
            {
                throw new InvalidOperationException("Zero axis of rotation specified.");
            }
            
            _x = axisOfRotation.X / length * sinHalfAngle;
            _y = axisOfRotation.Y / length * sinHalfAngle;
            _z = axisOfRotation.Z / length * sinHalfAngle;
            _w = Math.Cos(halfAngle);
        }

        // Note: Windows Quaternion doesn't expose Length or LengthSquared as properties
        // These are internal helpers for our implementation
        private double Length => Math.Sqrt(LengthSquared);
        private double LengthSquared => (_x * _x) + (_y * _y) + (_z * _z) + (_w * _w);

        /// <summary>
        /// Normalizes the quaternion.
        /// </summary>
        public void Normalize()
        {
            double length = Length;
            // Windows Quaternion behavior for zero quaternion - likely sets to NaN like Vector3D
            if (length == 0)
            {
                _x = double.NaN;
                _y = double.NaN;
                _z = double.NaN;
                _w = double.NaN;
            }
            else
            {
                _x /= length;
                _y /= length;
                _z /= length;
                _w /= length;
            }
        }

        /// <summary>
        /// Replaces this quaternion with its conjugate.
        /// </summary>
        public void Conjugate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
        }

        /// <summary>
        /// Replaces this quaternion with its inverse.
        /// </summary>
        public void Invert()
        {
            double lengthSquared = LengthSquared;
            // Windows Quaternion.Invert() sets components to NaN when length is zero, doesn't throw
            if (lengthSquared == 0)
            {
                _x = double.NaN;
                _y = double.NaN;
                _z = double.NaN;
                _w = double.NaN;
                return;
            }
            Conjugate();
            _x /= lengthSquared;
            _y /= lengthSquared;
            _z /= lengthSquared;
            _w /= lengthSquared;
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
#if NETSTANDARD2_0
            return ((((_x.GetHashCode() * 397) ^ _y.GetHashCode()) * 397) ^ _z.GetHashCode()) * 397 ^ _w.GetHashCode();
#else
            return HashCode.Combine(_x, _y, _z, _w);
#endif
        }

        /// <summary>
        /// Creates a string representation of this Quaternion structure.
        /// </summary>
        public override string ToString()
        {
            // Windows Quaternion doesn't implement IFormattable, only ToString()
            return string.Format("({0},{1},{2},{3})", _x, _y, _z, _w);
        }
    }
}
#endif

