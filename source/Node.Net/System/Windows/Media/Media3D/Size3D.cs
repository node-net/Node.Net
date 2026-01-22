#if !IS_WINDOWS
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents the size of a 3-D object.
    /// </summary>
    public struct Size3D
    {
        private double _x;
        private double _y;
        private double _z;

        /// <summary>
        /// Gets or sets the X component of this Size3D structure.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the Y component of this Size3D structure.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Gets or sets the Z component of this Size3D structure.
        /// </summary>
        public double Z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// Initializes a new instance of the Size3D structure.
        /// </summary>
        /// <param name="x">The X component of the new Size3D structure.</param>
        /// <param name="y">The Y component of the new Size3D structure.</param>
        /// <param name="z">The Z component of the new Size3D structure.</param>
        public Size3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Compares two Size3D structures for equality.
        /// </summary>
        public static bool operator ==(Size3D size1, Size3D size2)
        {
            return size1._x == size2._x && size1._y == size2._y && size1._z == size2._z;
        }

        /// <summary>
        /// Compares two Size3D structures for inequality.
        /// </summary>
        public static bool operator !=(Size3D size1, Size3D size2)
        {
            return !(size1 == size2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Size3D structure and whether the X, Y, and Z properties of the specified Object are equal to the X, Y, and Z properties of this Size3D structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Size3D size)
            {
                return this == size;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Size3D structure.
        /// </summary>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            return (((_x.GetHashCode() * 397) ^ _y.GetHashCode()) * 397) ^ _z.GetHashCode();
#else
            return HashCode.Combine(_x, _y, _z);
#endif
        }

        /// <summary>
        /// Creates a string representation of this Size3D structure.
        /// </summary>
        public override string ToString()
        {
            return string.Format("({0},{1},{2})", _x, _y, _z);
        }
    }
}
#endif

