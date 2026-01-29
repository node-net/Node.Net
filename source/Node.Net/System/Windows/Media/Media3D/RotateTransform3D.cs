#if !IS_WINDOWS && USE_POLYFILL
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Rotates an object in the 3-D x-y-z plane.
    /// </summary>
    public class RotateTransform3D : Transform3D
    {
        private Rotation3D _rotation;
        private Point3D _centerX;
        private Point3D _centerY;
        private Point3D _centerZ;

        /// <summary>
        /// Gets or sets the Rotation3D that specifies the rotation.
        /// </summary>
        public Rotation3D Rotation
        {
            get => _rotation;
            set => _rotation = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the Point3D about which to rotate.
        /// </summary>
        public Point3D CenterX
        {
            get => _centerX;
            set => _centerX = value;
        }

        /// <summary>
        /// Gets or sets the Point3D about which to rotate.
        /// </summary>
        public Point3D CenterY
        {
            get => _centerY;
            set => _centerY = value;
        }

        /// <summary>
        /// Gets or sets the Point3D about which to rotate.
        /// </summary>
        public Point3D CenterZ
        {
            get => _centerZ;
            set => _centerZ = value;
        }

        /// <summary>
        /// Initializes a new instance of the RotateTransform3D class.
        /// </summary>
        public RotateTransform3D()
        {
            _rotation = new AxisAngleRotation3D();
            _centerX = new Point3D(0, 0, 0);
            _centerY = new Point3D(0, 0, 0);
            _centerZ = new Point3D(0, 0, 0);
        }

        /// <summary>
        /// Initializes a new instance of the RotateTransform3D class with the specified rotation.
        /// </summary>
        /// <param name="rotation">The rotation to apply.</param>
        public RotateTransform3D(Rotation3D rotation)
        {
            _rotation = rotation ?? throw new ArgumentNullException(nameof(rotation));
            _centerX = new Point3D(0, 0, 0);
            _centerY = new Point3D(0, 0, 0);
            _centerZ = new Point3D(0, 0, 0);
        }

        /// <summary>
        /// Transforms the specified Point3D.
        /// </summary>
        public Point3D Transform(Point3D point)
        {
            if (_rotation is AxisAngleRotation3D axisAngle)
            {
                Matrix3D matrix = new Matrix3D();
                matrix.Rotate(new Quaternion(axisAngle.Axis, axisAngle.Angle));
                return matrix.Transform(point);
            }
            return point;
        }

        /// <summary>
        /// Transforms the specified Vector3D.
        /// </summary>
        public Vector3D Transform(Vector3D vector)
        {
            if (_rotation is AxisAngleRotation3D axisAngle)
            {
                Matrix3D matrix = new Matrix3D();
                matrix.Rotate(new Quaternion(axisAngle.Axis, axisAngle.Angle));
                return matrix.Transform(vector);
            }
            return vector;
        }
    }
}
#endif

