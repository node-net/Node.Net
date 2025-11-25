#if !IS_WINDOWS
using System;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents a 3-D rotation of a specified angle about a specified axis.
    /// </summary>
    public class AxisAngleRotation3D : Rotation3D
    {
        private Vector3D _axis;
        private double _angle;

        /// <summary>
        /// Gets or sets the axis of the 3-D rotation.
        /// </summary>
        public Vector3D Axis
        {
            get => _axis;
            set => _axis = value;
        }

        /// <summary>
        /// Gets or sets the angle of the 3-D rotation, in degrees.
        /// </summary>
        public double Angle
        {
            get => _angle;
            set => _angle = value;
        }

        /// <summary>
        /// Initializes a new instance of the AxisAngleRotation3D class.
        /// </summary>
        public AxisAngleRotation3D()
        {
            _axis = new Vector3D(0, 0, 1);
            _angle = 0;
        }

        /// <summary>
        /// Initializes a new instance of the AxisAngleRotation3D class with the specified axis and angle.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation, in degrees.</param>
        public AxisAngleRotation3D(Vector3D axis, double angle)
        {
            _axis = axis;
            _angle = angle;
        }
    }
}
#endif

