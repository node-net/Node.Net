#if !IS_WINDOWS && USE_POLYFILL
using System;

namespace System.Windows.Media
{
    /// <summary>
    /// Represents a 3x3 affine transformation matrix used for transformations in 2-D space.
    /// </summary>
    public struct Matrix
    {
        private double _m11;
        private double _m12;
        private double _m21;
        private double _m22;
        private double _offsetX;
        private double _offsetY;

        /// <summary>
        /// Gets or sets the value of the first row and first column of this Matrix structure.
        /// </summary>
        public double M11
        {
            get => _m11;
            set => _m11 = value;
        }

        /// <summary>
        /// Gets or sets the value of the first row and second column of this Matrix structure.
        /// </summary>
        public double M12
        {
            get => _m12;
            set => _m12 = value;
        }

        /// <summary>
        /// Gets or sets the value of the second row and first column of this Matrix structure.
        /// </summary>
        public double M21
        {
            get => _m21;
            set => _m21 = value;
        }

        /// <summary>
        /// Gets or sets the value of the second row and second column of this Matrix structure.
        /// </summary>
        public double M22
        {
            get => _m22;
            set => _m22 = value;
        }

        /// <summary>
        /// Gets or sets the value of the third row and first column of this Matrix structure.
        /// </summary>
        public double OffsetX
        {
            get => _offsetX;
            set => _offsetX = value;
        }

        /// <summary>
        /// Gets or sets the value of the third row and second column of this Matrix structure.
        /// </summary>
        public double OffsetY
        {
            get => _offsetY;
            set => _offsetY = value;
        }

        /// <summary>
        /// Initializes a new instance of the Matrix structure as an identity matrix.
        /// </summary>
        public Matrix()
        {
            _m11 = 1.0;
            _m12 = 0.0;
            _m21 = 0.0;
            _m22 = 1.0;
            _offsetX = 0.0;
            _offsetY = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the Matrix structure.
        /// </summary>
        public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
        {
            _m11 = m11;
            _m12 = m12;
            _m21 = m21;
            _m22 = m22;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }

        /// <summary>
        /// Appends a rotation of the specified angle to this Matrix structure.
        /// </summary>
        /// <param name="angle">The angle of rotation in degrees.</param>
        public void Rotate(double angle)
        {
            double angleInRadians = angle * (Math.PI / 180.0);
            double cos = Math.Cos(angleInRadians);
            double sin = Math.Sin(angleInRadians);

            // Rotation matrix: [cos -sin; sin cos]
            // Append: this = this * rotation
            double newM11 = _m11 * cos - _m12 * sin;
            double newM12 = _m11 * sin + _m12 * cos;
            double newM21 = _m21 * cos - _m22 * sin;
            double newM22 = _m21 * sin + _m22 * cos;

            _m11 = newM11;
            _m12 = newM12;
            _m21 = newM21;
            _m22 = newM22;
        }

        /// <summary>
        /// Multiplies the specified Vector by the specified Matrix and returns the result.
        /// </summary>
        public static System.Windows.Vector Multiply(System.Windows.Vector vector, Matrix matrix)
        {
            // Vector transformation: [x y] * [m11 m12; m21 m22] = [x*m11+y*m21, x*m12+y*m22]
            double x = vector.X * matrix._m11 + vector.Y * matrix._m21;
            double y = vector.X * matrix._m12 + vector.Y * matrix._m22;
            return new System.Windows.Vector(x, y);
        }

        /// <summary>
        /// Compares two Matrix structures for equality.
        /// </summary>
        public static bool operator ==(Matrix matrix1, Matrix matrix2)
        {
            return matrix1._m11 == matrix2._m11 && matrix1._m12 == matrix2._m12 &&
                   matrix1._m21 == matrix2._m21 && matrix1._m22 == matrix2._m22 &&
                   matrix1._offsetX == matrix2._offsetX && matrix1._offsetY == matrix2._offsetY;
        }

        /// <summary>
        /// Compares two Matrix structures for inequality.
        /// </summary>
        public static bool operator !=(Matrix matrix1, Matrix matrix2)
        {
            return !(matrix1 == matrix2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Matrix structure and whether it is identical to this Matrix.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Matrix matrix)
            {
                return this == matrix;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Matrix structure.
        /// </summary>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            return ((((((_m11.GetHashCode() * 397) ^ _m12.GetHashCode()) * 397) ^ _m21.GetHashCode()) * 397) ^ _m22.GetHashCode()) * 397 ^ _offsetX.GetHashCode() * 397 ^ _offsetY.GetHashCode();
#else
            return HashCode.Combine(_m11, _m12, _m21, _m22, _offsetX, _offsetY);
#endif
        }

        /// <summary>
        /// Creates a string representation of this Matrix structure.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{{M11:{0} M12:{1} M21:{2} M22:{3} OffsetX:{4} OffsetY:{5}}}",
                _m11, _m12, _m21, _m22, _offsetX, _offsetY);
        }
    }
}
#endif

