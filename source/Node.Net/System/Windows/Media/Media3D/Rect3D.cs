#if !IS_WINDOWS
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents a 3-D rectangle.
    /// </summary>
    public struct Rect3D
    {
        private double _x;
        private double _y;
        private double _z;
        private double _sizeX;
        private double _sizeY;
        private double _sizeZ;

        /// <summary>
        /// Gets or sets the X coordinate of the Rect3D.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the Rect3D.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Gets or sets the Z coordinate of the Rect3D.
        /// </summary>
        public double Z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// Gets or sets the X size of the Rect3D.
        /// </summary>
        public double SizeX
        {
            get => _sizeX;
            set
            {
                // Windows Rect3D throws ArgumentException for negative dimensions (except for Empty which uses NegativeInfinity)
                if (value < 0 && !double.IsNegativeInfinity(value))
                {
                    throw new ArgumentException("Cannot set a negative dimension.");
                }
                _sizeX = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y size of the Rect3D.
        /// </summary>
        public double SizeY
        {
            get => _sizeY;
            set
            {
                // Windows Rect3D throws ArgumentException for negative dimensions (except for Empty which uses NegativeInfinity)
                if (value < 0 && !double.IsNegativeInfinity(value))
                {
                    throw new ArgumentException("Cannot set a negative dimension.");
                }
                _sizeY = value;
            }
        }

        /// <summary>
        /// Gets or sets the Z size of the Rect3D.
        /// </summary>
        public double SizeZ
        {
            get => _sizeZ;
            set
            {
                // Windows Rect3D throws ArgumentException for negative dimensions (except for Empty which uses NegativeInfinity)
                if (value < 0 && !double.IsNegativeInfinity(value))
                {
                    throw new ArgumentException("Cannot set a negative dimension.");
                }
                _sizeZ = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of the Rect3D.
        /// </summary>
        public Point3D Location
        {
            get => new Point3D(_x, _y, _z);
            set
            {
                _x = value.X;
                _y = value.Y;
                _z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the size of the Rect3D.
        /// </summary>
        public Size3D Size
        {
            get => new Size3D(_sizeX, _sizeY, _sizeZ);
            set
            {
                // Windows Rect3D throws ArgumentException for negative dimensions (except for Empty which uses NegativeInfinity)
                if ((value.X < 0 || value.Y < 0 || value.Z < 0) && 
                    !(double.IsNegativeInfinity(value.X) || double.IsNegativeInfinity(value.Y) || double.IsNegativeInfinity(value.Z)))
                {
                    throw new ArgumentException("Cannot set a negative dimension.");
                }
                _sizeX = value.X;
                _sizeY = value.Y;
                _sizeZ = value.Z;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the Rect3D is the Empty rectangle.
        /// </summary>
        public bool IsEmpty => _sizeX < 0 || _sizeY < 0 || _sizeZ < 0;

        /// <summary>
        /// Represents an empty Rect3D.
        /// </summary>
        public static Rect3D Empty => new Rect3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

        /// <summary>
        /// Initializes a new instance of the Rect3D structure.
        /// </summary>
        /// <param name="location">The location of the Rect3D.</param>
        /// <param name="size">The size of the Rect3D.</param>
        public Rect3D(Point3D location, Size3D size)
        {
            // Windows Rect3D throws ArgumentException for negative dimensions (except for Empty which uses NegativeInfinity)
            if ((size.X < 0 || size.Y < 0 || size.Z < 0) && 
                !(double.IsNegativeInfinity(size.X) || double.IsNegativeInfinity(size.Y) || double.IsNegativeInfinity(size.Z)))
            {
                throw new ArgumentException("Cannot set a negative dimension.");
            }
            _x = location.X;
            _y = location.Y;
            _z = location.Z;
            _sizeX = size.X;
            _sizeY = size.Y;
            _sizeZ = size.Z;
        }

        /// <summary>
        /// Initializes a new instance of the Rect3D structure.
        /// </summary>
        /// <param name="x">The X coordinate of the Rect3D.</param>
        /// <param name="y">The Y coordinate of the Rect3D.</param>
        /// <param name="z">The Z coordinate of the Rect3D.</param>
        /// <param name="sizeX">The X size of the Rect3D.</param>
        /// <param name="sizeY">The Y size of the Rect3D.</param>
        /// <param name="sizeZ">The Z size of the Rect3D.</param>
        public Rect3D(double x, double y, double z, double sizeX, double sizeY, double sizeZ)
        {
            // Windows Rect3D throws ArgumentException for negative dimensions (except for Empty which uses NegativeInfinity)
            if ((sizeX < 0 || sizeY < 0 || sizeZ < 0) && 
                !(double.IsNegativeInfinity(sizeX) || double.IsNegativeInfinity(sizeY) || double.IsNegativeInfinity(sizeZ)))
            {
                throw new ArgumentException("Cannot set a negative dimension.");
            }
            _x = x;
            _y = y;
            _z = z;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _sizeZ = sizeZ;
        }

        /// <summary>
        /// Compares two Rect3D structures for equality.
        /// </summary>
        public static bool operator ==(Rect3D rect1, Rect3D rect2)
        {
            return rect1._x == rect2._x && rect1._y == rect2._y && rect1._z == rect2._z &&
                   rect1._sizeX == rect2._sizeX && rect1._sizeY == rect2._sizeY && rect1._sizeZ == rect2._sizeZ;
        }

        /// <summary>
        /// Compares two Rect3D structures for inequality.
        /// </summary>
        public static bool operator !=(Rect3D rect1, Rect3D rect2)
        {
            return !(rect1 == rect2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Rect3D structure and whether the X, Y, Z, SizeX, SizeY, and SizeZ properties of the specified Object are equal to the X, Y, Z, SizeX, SizeY, and SizeZ properties of this Rect3D structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Rect3D rect)
            {
                return this == rect;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Rect3D structure.
        /// </summary>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            return ((((((_x.GetHashCode() * 397) ^ _y.GetHashCode()) * 397) ^ _z.GetHashCode()) * 397) ^ _sizeX.GetHashCode()) * 397 ^ _sizeY.GetHashCode() * 397 ^ _sizeZ.GetHashCode();
#else
            return HashCode.Combine(_x, _y, _z, _sizeX, _sizeY, _sizeZ);
#endif
        }

        /// <summary>
        /// Creates a string representation of this Rect3D structure.
        /// </summary>
        public override string ToString()
        {
            return string.Format("({0},{1},{2},{3},{4},{5})", _x, _y, _z, _sizeX, _sizeY, _sizeZ);
        }
    }
}
#endif

