#if !IS_WINDOWS && USE_POLYFILL
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents a 4x4 matrix that is used for transformations in 3-D space.
    /// </summary>
    public struct Matrix3D
    {
        private double _m11;
        private double _m12;
        private double _m13;
        private double _m14;
        private double _m21;
        private double _m22;
        private double _m23;
        private double _m24;
        private double _m31;
        private double _m32;
        private double _m33;
        private double _m34;
        private double _offsetX;
        private double _offsetY;
        private double _offsetZ;
        private double _m44;

        /// <summary>
        /// Gets or sets the value of the first row and first column of this Matrix3D structure.
        /// </summary>
        public double M11
        {
            get => _m11;
            set => _m11 = value;
        }

        /// <summary>
        /// Gets or sets the value of the first row and second column of this Matrix3D structure.
        /// </summary>
        public double M12
        {
            get => _m12;
            set => _m12 = value;
        }

        /// <summary>
        /// Gets or sets the value of the first row and third column of this Matrix3D structure.
        /// </summary>
        public double M13
        {
            get => _m13;
            set => _m13 = value;
        }

        /// <summary>
        /// Gets or sets the value of the first row and fourth column of this Matrix3D structure.
        /// </summary>
        public double M14
        {
            get => _m14;
            set => _m14 = value;
        }

        /// <summary>
        /// Gets or sets the value of the second row and first column of this Matrix3D structure.
        /// </summary>
        public double M21
        {
            get => _m21;
            set => _m21 = value;
        }

        /// <summary>
        /// Gets or sets the value of the second row and second column of this Matrix3D structure.
        /// </summary>
        public double M22
        {
            get => _m22;
            set => _m22 = value;
        }

        /// <summary>
        /// Gets or sets the value of the second row and third column of this Matrix3D structure.
        /// </summary>
        public double M23
        {
            get => _m23;
            set => _m23 = value;
        }

        /// <summary>
        /// Gets or sets the value of the second row and fourth column of this Matrix3D structure.
        /// </summary>
        public double M24
        {
            get => _m24;
            set => _m24 = value;
        }

        /// <summary>
        /// Gets or sets the value of the third row and first column of this Matrix3D structure.
        /// </summary>
        public double M31
        {
            get => _m31;
            set => _m31 = value;
        }

        /// <summary>
        /// Gets or sets the value of the third row and second column of this Matrix3D structure.
        /// </summary>
        public double M32
        {
            get => _m32;
            set => _m32 = value;
        }

        /// <summary>
        /// Gets or sets the value of the third row and third column of this Matrix3D structure.
        /// </summary>
        public double M33
        {
            get => _m33;
            set => _m33 = value;
        }

        /// <summary>
        /// Gets or sets the value of the third row and fourth column of this Matrix3D structure.
        /// </summary>
        public double M34
        {
            get => _m34;
            set => _m34 = value;
        }

        /// <summary>
        /// Gets or sets the value of the fourth row and first column of this Matrix3D structure.
        /// </summary>
        public double OffsetX
        {
            get => _offsetX;
            set => _offsetX = value;
        }

        /// <summary>
        /// Gets or sets the value of the fourth row and second column of this Matrix3D structure.
        /// </summary>
        public double OffsetY
        {
            get => _offsetY;
            set => _offsetY = value;
        }

        /// <summary>
        /// Gets or sets the value of the fourth row and third column of this Matrix3D structure.
        /// </summary>
        public double OffsetZ
        {
            get => _offsetZ;
            set => _offsetZ = value;
        }

        /// <summary>
        /// Gets or sets the value of the fourth row and fourth column of this Matrix3D structure.
        /// </summary>
        public double M44
        {
            get => _m44;
            set => _m44 = value;
        }

        /// <summary>
        /// Initializes a new instance of the Matrix3D structure.
        /// </summary>
        public Matrix3D()
        {
            _m11 = 1.0;
            _m12 = 0.0;
            _m13 = 0.0;
            _m14 = 0.0;
            _m21 = 0.0;
            _m22 = 1.0;
            _m23 = 0.0;
            _m24 = 0.0;
            _m31 = 0.0;
            _m32 = 0.0;
            _m33 = 1.0;
            _m34 = 0.0;
            _offsetX = 0.0;
            _offsetY = 0.0;
            _offsetZ = 0.0;
            _m44 = 1.0;
        }

        /// <summary>
        /// Initializes a new instance of the Matrix3D structure.
        /// </summary>
        public Matrix3D(double m11, double m12, double m13, double m14,
                       double m21, double m22, double m23, double m24,
                       double m31, double m32, double m33, double m34,
                       double offsetX, double offsetY, double offsetZ, double m44)
        {
            _m11 = m11;
            _m12 = m12;
            _m13 = m13;
            _m14 = m14;
            _m21 = m21;
            _m22 = m22;
            _m23 = m23;
            _m24 = m24;
            _m31 = m31;
            _m32 = m32;
            _m33 = m33;
            _m34 = m34;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _offsetZ = offsetZ;
            _m44 = m44;
        }

        /// <summary>
        /// Gets a value that indicates whether this Matrix3D structure is an identity matrix.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return _m11 == 1.0 && _m12 == 0.0 && _m13 == 0.0 && _m14 == 0.0 &&
                       _m21 == 0.0 && _m22 == 1.0 && _m23 == 0.0 && _m24 == 0.0 &&
                       _m31 == 0.0 && _m32 == 0.0 && _m33 == 1.0 && _m34 == 0.0 &&
                       _offsetX == 0.0 && _offsetY == 0.0 && _offsetZ == 0.0 && _m44 == 1.0;
            }
        }

        /// <summary>
        /// Gets the determinant of this Matrix3D structure.
        /// </summary>
        public double Determinant
        {
            get
            {
                // Calculate 4x4 matrix determinant
                double a = _m11, b = _m12, c = _m13, d = _m14;
                double e = _m21, f = _m22, g = _m23, h = _m24;
                double i = _m31, j = _m32, k = _m33, l = _m34;
                double m = _offsetX, n = _offsetY, o = _offsetZ, p = _m44;

                return a * (f * (k * p - l * o) - g * (j * p - l * n) + h * (j * o - k * n)) -
                       b * (e * (k * p - l * o) - g * (i * p - l * m) + h * (i * o - k * m)) +
                       c * (e * (j * p - l * n) - f * (i * p - l * m) + h * (i * n - j * m)) -
                       d * (e * (j * o - k * n) - f * (i * o - k * m) + g * (i * n - j * m));
            }
        }

        /// <summary>
        /// Sets this Matrix3D structure to an identity matrix.
        /// </summary>
        public void SetIdentity()
        {
            _m11 = 1.0;
            _m12 = 0.0;
            _m13 = 0.0;
            _m14 = 0.0;
            _m21 = 0.0;
            _m22 = 1.0;
            _m23 = 0.0;
            _m24 = 0.0;
            _m31 = 0.0;
            _m32 = 0.0;
            _m33 = 1.0;
            _m34 = 0.0;
            _offsetX = 0.0;
            _offsetY = 0.0;
            _offsetZ = 0.0;
            _m44 = 1.0;
        }

        /// <summary>
        /// Transforms the specified Point3D by this Matrix3D and returns the result.
        /// </summary>
        public Point3D Transform(Point3D point)
        {
            double x = point.X;
            double y = point.Y;
            double z = point.Z;
            double w = 1.0;

            double resultX = (x * _m11) + (y * _m21) + (z * _m31) + (w * _offsetX);
            double resultY = (x * _m12) + (y * _m22) + (z * _m32) + (w * _offsetY);
            double resultZ = (x * _m13) + (y * _m23) + (z * _m33) + (w * _offsetZ);
            double resultW = (x * _m14) + (y * _m24) + (z * _m34) + (w * _m44);

            if (resultW != 1.0 && resultW != 0.0)
            {
                resultX /= resultW;
                resultY /= resultW;
                resultZ /= resultW;
            }

            return new Point3D(resultX, resultY, resultZ);
        }

        /// <summary>
        /// Transforms the specified Vector3D by this Matrix3D and returns the result.
        /// </summary>
        public Vector3D Transform(Vector3D vector)
        {
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            return new Vector3D(
                (x * _m11) + (y * _m21) + (z * _m31),
                (x * _m12) + (y * _m22) + (z * _m32),
                (x * _m13) + (y * _m23) + (z * _m33)
            );
        }

        /// <summary>
        /// Appends the specified Matrix3D structure to this Matrix3D structure.
        /// </summary>
        public void Append(Matrix3D matrix)
        {
            double m11 = (_m11 * matrix._m11) + (_m12 * matrix._m21) + (_m13 * matrix._m31) + (_m14 * matrix._offsetX);
            double m12 = (_m11 * matrix._m12) + (_m12 * matrix._m22) + (_m13 * matrix._m32) + (_m14 * matrix._offsetY);
            double m13 = (_m11 * matrix._m13) + (_m12 * matrix._m23) + (_m13 * matrix._m33) + (_m14 * matrix._offsetZ);
            double m14 = (_m11 * matrix._m14) + (_m12 * matrix._m24) + (_m13 * matrix._m34) + (_m14 * matrix._m44);

            double m21 = (_m21 * matrix._m11) + (_m22 * matrix._m21) + (_m23 * matrix._m31) + (_m24 * matrix._offsetX);
            double m22 = (_m21 * matrix._m12) + (_m22 * matrix._m22) + (_m23 * matrix._m32) + (_m24 * matrix._offsetY);
            double m23 = (_m21 * matrix._m13) + (_m22 * matrix._m23) + (_m23 * matrix._m33) + (_m24 * matrix._offsetZ);
            double m24 = (_m21 * matrix._m14) + (_m22 * matrix._m24) + (_m23 * matrix._m34) + (_m24 * matrix._m44);

            double m31 = (_m31 * matrix._m11) + (_m32 * matrix._m21) + (_m33 * matrix._m31) + (_m34 * matrix._offsetX);
            double m32 = (_m31 * matrix._m12) + (_m32 * matrix._m22) + (_m33 * matrix._m32) + (_m34 * matrix._offsetY);
            double m33 = (_m31 * matrix._m13) + (_m32 * matrix._m23) + (_m33 * matrix._m33) + (_m34 * matrix._offsetZ);
            double m34 = (_m31 * matrix._m14) + (_m32 * matrix._m24) + (_m33 * matrix._m34) + (_m34 * matrix._m44);

            double offsetX = (_offsetX * matrix._m11) + (_offsetY * matrix._m21) + (_offsetZ * matrix._m31) + (_m44 * matrix._offsetX);
            double offsetY = (_offsetX * matrix._m12) + (_offsetY * matrix._m22) + (_offsetZ * matrix._m32) + (_m44 * matrix._offsetY);
            double offsetZ = (_offsetX * matrix._m13) + (_offsetY * matrix._m23) + (_offsetZ * matrix._m33) + (_m44 * matrix._offsetZ);
            double m44 = (_offsetX * matrix._m14) + (_offsetY * matrix._m24) + (_offsetZ * matrix._m34) + (_m44 * matrix._m44);

            _m11 = m11;
            _m12 = m12;
            _m13 = m13;
            _m14 = m14;
            _m21 = m21;
            _m22 = m22;
            _m23 = m23;
            _m24 = m24;
            _m31 = m31;
            _m32 = m32;
            _m33 = m33;
            _m34 = m34;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _offsetZ = offsetZ;
            _m44 = m44;
        }

        /// <summary>
        /// Prepends the specified Matrix3D structure to this Matrix3D structure.
        /// </summary>
        public void Prepend(Matrix3D matrix)
        {
            double m11 = (matrix._m11 * _m11) + (matrix._m12 * _m21) + (matrix._m13 * _m31) + (matrix._m14 * _offsetX);
            double m12 = (matrix._m11 * _m12) + (matrix._m12 * _m22) + (matrix._m13 * _m32) + (matrix._m14 * _offsetY);
            double m13 = (matrix._m11 * _m13) + (matrix._m12 * _m23) + (matrix._m13 * _m33) + (matrix._m14 * _offsetZ);
            double m14 = (matrix._m11 * _m14) + (matrix._m12 * _m24) + (matrix._m13 * _m34) + (matrix._m14 * _m44);

            double m21 = (matrix._m21 * _m11) + (matrix._m22 * _m21) + (matrix._m23 * _m31) + (matrix._m24 * _offsetX);
            double m22 = (matrix._m21 * _m12) + (matrix._m22 * _m22) + (matrix._m23 * _m32) + (matrix._m24 * _offsetY);
            double m23 = (matrix._m21 * _m13) + (matrix._m22 * _m23) + (matrix._m23 * _m33) + (matrix._m24 * _offsetZ);
            double m24 = (matrix._m21 * _m14) + (matrix._m22 * _m24) + (matrix._m23 * _m34) + (matrix._m24 * _m44);

            double m31 = (matrix._m31 * _m11) + (matrix._m32 * _m21) + (matrix._m33 * _m31) + (matrix._m34 * _offsetX);
            double m32 = (matrix._m31 * _m12) + (matrix._m32 * _m22) + (matrix._m33 * _m32) + (matrix._m34 * _offsetY);
            double m33 = (matrix._m31 * _m13) + (matrix._m32 * _m23) + (matrix._m33 * _m33) + (matrix._m34 * _offsetZ);
            double m34 = (matrix._m31 * _m14) + (matrix._m32 * _m24) + (matrix._m33 * _m34) + (matrix._m34 * _m44);

            double offsetX = (matrix._offsetX * _m11) + (matrix._offsetY * _m21) + (matrix._offsetZ * _m31) + (matrix._m44 * _offsetX);
            double offsetY = (matrix._offsetX * _m12) + (matrix._offsetY * _m22) + (matrix._offsetZ * _m32) + (matrix._m44 * _offsetY);
            double offsetZ = (matrix._offsetX * _m13) + (matrix._offsetY * _m23) + (matrix._offsetZ * _m33) + (matrix._m44 * _offsetZ);
            double m44 = (matrix._offsetX * _m14) + (matrix._offsetY * _m24) + (matrix._offsetZ * _m34) + (matrix._m44 * _m44);

            _m11 = m11;
            _m12 = m12;
            _m13 = m13;
            _m14 = m14;
            _m21 = m21;
            _m22 = m22;
            _m23 = m23;
            _m24 = m24;
            _m31 = m31;
            _m32 = m32;
            _m33 = m33;
            _m34 = m34;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _offsetZ = offsetZ;
            _m44 = m44;
        }

        /// <summary>
        /// Translates the Matrix3D structure by the specified Vector3D structure.
        /// </summary>
        public void Translate(Vector3D offset)
        {
            _offsetX += offset.X;
            _offsetY += offset.Y;
            _offsetZ += offset.Z;
        }

        /// <summary>
        /// Scales the Matrix3D structure by the specified Vector3D structure.
        /// </summary>
        public void Scale(Vector3D scale)
        {
            _m11 *= scale.X;
            _m12 *= scale.X;
            _m13 *= scale.X;
            _m14 *= scale.X;
            _m21 *= scale.Y;
            _m22 *= scale.Y;
            _m23 *= scale.Y;
            _m24 *= scale.Y;
            _m31 *= scale.Z;
            _m32 *= scale.Z;
            _m33 *= scale.Z;
            _m34 *= scale.Z;
        }

        /// <summary>
        /// Scales the Matrix3D structure by the specified Vector3D structure about the specified Point3D structure.
        /// </summary>
        public void ScaleAt(Vector3D scale, Point3D center)
        {
            Translate(new Vector3D(-center.X, -center.Y, -center.Z));
            Scale(scale);
            Translate(new Vector3D(center.X, center.Y, center.Z));
        }

        /// <summary>
        /// Rotates this Matrix3D structure by the specified Quaternion.
        /// </summary>
        public void Rotate(Quaternion quaternion)
        {
            // Normalize the quaternion
            Quaternion q = quaternion;
            double lengthSquared = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
            // If quaternion is zero or invalid, return early (no rotation)
            if (lengthSquared == 0)
            {
                return;
            }
            if (lengthSquared > 0)
            {
                double length = Math.Sqrt(lengthSquared);
                q = new Quaternion(q.X / length, q.Y / length, q.Z / length, q.W / length);
            }

            // Use quaternion directly (Windows WPF convention)
            double x = q.X;
            double y = q.Y;
            double z = q.Z;
            double w = q.W;

            double xx = x * x;
            double yy = y * y;
            double zz = z * z;
            double xy = x * y;
            double xz = x * z;
            double yz = y * z;
            double wx = w * x;
            double wy = w * y;
            double wz = w * z;

            // Standard quaternion to rotation matrix conversion
            Matrix3D rotationMatrix = new Matrix3D(
                1 - 2 * (yy + zz), 2 * (xy - wz), 2 * (xz + wy), 0,
                2 * (xy + wz), 1 - 2 * (xx + zz), 2 * (yz - wx), 0,
                2 * (xz - wy), 2 * (yz + wx), 1 - 2 * (xx + yy), 0,
                0, 0, 0, 1
            );

            Append(rotationMatrix);
        }

        /// <summary>
        /// Inverts this Matrix3D structure.
        /// </summary>
        public void Invert()
        {
            double det = Determinant;
            if (Math.Abs(det) < 1e-10)
            {
                throw new InvalidOperationException("Matrix is not invertible (determinant is zero or near zero).");
            }

            double invDet = 1.0 / det;

            // Calculate adjugate matrix (transpose of cofactor matrix)
            double a11 = _m11, a12 = _m12, a13 = _m13, a14 = _m14;
            double a21 = _m21, a22 = _m22, a23 = _m23, a24 = _m24;
            double a31 = _m31, a32 = _m32, a33 = _m33, a34 = _m34;
            double a41 = _offsetX, a42 = _offsetY, a43 = _offsetZ, a44 = _m44;

            // Calculate cofactors for 3x3 submatrices
            double c11 = a22 * (a33 * a44 - a34 * a43) - a23 * (a32 * a44 - a34 * a42) + a24 * (a32 * a43 - a33 * a42);
            double c12 = -(a21 * (a33 * a44 - a34 * a43) - a23 * (a31 * a44 - a34 * a41) + a24 * (a31 * a43 - a33 * a41));
            double c13 = a21 * (a32 * a44 - a34 * a42) - a22 * (a31 * a44 - a34 * a41) + a24 * (a31 * a42 - a32 * a41);
            double c14 = -(a21 * (a32 * a43 - a33 * a42) - a22 * (a31 * a43 - a33 * a41) + a23 * (a31 * a42 - a32 * a41));

            double c21 = -(a12 * (a33 * a44 - a34 * a43) - a13 * (a32 * a44 - a34 * a42) + a14 * (a32 * a43 - a33 * a42));
            double c22 = a11 * (a33 * a44 - a34 * a43) - a13 * (a31 * a44 - a34 * a41) + a14 * (a31 * a43 - a33 * a41);
            double c23 = -(a11 * (a32 * a44 - a34 * a42) - a12 * (a31 * a44 - a34 * a41) + a14 * (a31 * a42 - a32 * a41));
            double c24 = a11 * (a32 * a43 - a33 * a42) - a12 * (a31 * a43 - a33 * a41) + a13 * (a31 * a42 - a32 * a41);

            double c31 = a12 * (a23 * a44 - a24 * a43) - a13 * (a22 * a44 - a24 * a42) + a14 * (a22 * a43 - a23 * a42);
            double c32 = -(a11 * (a23 * a44 - a24 * a43) - a13 * (a21 * a44 - a24 * a41) + a14 * (a21 * a43 - a23 * a41));
            double c33 = a11 * (a22 * a44 - a24 * a42) - a12 * (a21 * a44 - a24 * a41) + a14 * (a21 * a42 - a22 * a41);
            double c34 = -(a11 * (a22 * a43 - a23 * a42) - a12 * (a21 * a43 - a23 * a41) + a13 * (a21 * a42 - a22 * a41));

            double c41 = -(a12 * (a23 * a34 - a24 * a33) - a13 * (a22 * a34 - a24 * a32) + a14 * (a22 * a33 - a23 * a32));
            double c42 = a11 * (a23 * a34 - a24 * a33) - a13 * (a21 * a34 - a24 * a31) + a14 * (a21 * a33 - a23 * a31);
            double c43 = -(a11 * (a22 * a34 - a24 * a32) - a12 * (a21 * a34 - a24 * a31) + a14 * (a21 * a32 - a22 * a31));
            double c44 = a11 * (a22 * a33 - a23 * a32) - a12 * (a21 * a33 - a23 * a31) + a13 * (a21 * a32 - a22 * a31);

            // Transpose and multiply by inverse determinant
            _m11 = c11 * invDet;
            _m12 = c21 * invDet;
            _m13 = c31 * invDet;
            _m14 = c41 * invDet;
            _m21 = c12 * invDet;
            _m22 = c22 * invDet;
            _m23 = c32 * invDet;
            _m24 = c42 * invDet;
            _m31 = c13 * invDet;
            _m32 = c23 * invDet;
            _m33 = c33 * invDet;
            _m34 = c43 * invDet;
            _offsetX = c14 * invDet;
            _offsetY = c24 * invDet;
            _offsetZ = c34 * invDet;
            _m44 = c44 * invDet;
        }

        /// <summary>
        /// Multiplies the specified matrices.
        /// </summary>
        public static Matrix3D Multiply(Matrix3D matrix1, Matrix3D matrix2)
        {
            Matrix3D result = matrix1;
            result.Append(matrix2);
            return result;
        }

        /// <summary>
        /// Multiplies the specified matrices.
        /// </summary>
        public static Matrix3D operator *(Matrix3D matrix1, Matrix3D matrix2)
        {
            return Multiply(matrix1, matrix2);
        }

        /// <summary>
        /// Compares two Matrix3D structures for equality.
        /// </summary>
        public static bool operator ==(Matrix3D matrix1, Matrix3D matrix2)
        {
            return matrix1._m11 == matrix2._m11 && matrix1._m12 == matrix2._m12 &&
                   matrix1._m13 == matrix2._m13 && matrix1._m14 == matrix2._m14 &&
                   matrix1._m21 == matrix2._m21 && matrix1._m22 == matrix2._m22 &&
                   matrix1._m23 == matrix2._m23 && matrix1._m24 == matrix2._m24 &&
                   matrix1._m31 == matrix2._m31 && matrix1._m32 == matrix2._m32 &&
                   matrix1._m33 == matrix2._m33 && matrix1._m34 == matrix2._m34 &&
                   matrix1._offsetX == matrix2._offsetX && matrix1._offsetY == matrix2._offsetY &&
                   matrix1._offsetZ == matrix2._offsetZ && matrix1._m44 == matrix2._m44;
        }

        /// <summary>
        /// Compares two Matrix3D structures for inequality.
        /// </summary>
        public static bool operator !=(Matrix3D matrix1, Matrix3D matrix2)
        {
            return !(matrix1 == matrix2);
        }

        /// <summary>
        /// Determines whether the specified Object is a Matrix3D structure and whether the properties of the specified Object are equal to the properties of this Matrix3D structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Matrix3D matrix)
            {
                return this == matrix;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Matrix3D structure.
        /// </summary>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            // Calculate hash1 for first 8 matrix values
            int hash1 = _m11.GetHashCode();
            hash1 = (hash1 * 397) ^ _m12.GetHashCode();
            hash1 = (hash1 * 397) ^ _m13.GetHashCode();
            hash1 = (hash1 * 397) ^ _m14.GetHashCode();
            hash1 = (hash1 * 397) ^ _m21.GetHashCode();
            hash1 = (hash1 * 397) ^ _m22.GetHashCode();
            hash1 = (hash1 * 397) ^ _m23.GetHashCode();
            hash1 = (hash1 * 397) ^ _m24.GetHashCode();
            
            // Calculate hash2 for remaining 8 matrix values
            int hash2 = _m31.GetHashCode();
            hash2 = (hash2 * 397) ^ _m32.GetHashCode();
            hash2 = (hash2 * 397) ^ _m33.GetHashCode();
            hash2 = (hash2 * 397) ^ _m34.GetHashCode();
            hash2 = (hash2 * 397) ^ _offsetX.GetHashCode();
            hash2 = (hash2 * 397) ^ _offsetY.GetHashCode();
            hash2 = (hash2 * 397) ^ _offsetZ.GetHashCode();
            hash2 = (hash2 * 397) ^ _m44.GetHashCode();
            
            // Combine hash1 and hash2
            return (hash1 * 397) ^ hash2;
#else
            int hash1 = HashCode.Combine(_m11, _m12, _m13, _m14, _m21, _m22, _m23, _m24);
            int hash2 = HashCode.Combine(_m31, _m32, _m33, _m34, _offsetX, _offsetY, _offsetZ, _m44);
            return HashCode.Combine(hash1, hash2);
#endif
        }

        /// <summary>
        /// Creates a string representation of this Matrix3D structure.
        /// </summary>
        public override string ToString()
        {
            // Windows Matrix3D doesn't implement IFormattable, only ToString()
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                _m11, _m12, _m13, _m14,
                _m21, _m22, _m23, _m24,
                _m31, _m32, _m33, _m34,
                _offsetX, _offsetY, _offsetZ, _m44);
        }
    }
}
#endif

