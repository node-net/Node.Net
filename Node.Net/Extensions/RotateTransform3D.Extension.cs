using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public class RotateTransform3DExtension
    {
        public static RotateTransform3D GetRotateTransform3D(System.Collections.IDictionary value)
        {
            Quaternion rotationZ = new Quaternion();
            Quaternion rotationY = new Quaternion();
            Quaternion rotationX = new Quaternion();
            QuaternionRotation3D rotation = new QuaternionRotation3D();
            if (value.Contains("RotationZ"))
            {
                double rotationZ_degrees = Measurement.Angle.GetRotationDegrees(value, "RotationZ");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("Orientation"))
            {
                double rotationZ_degrees = Measurement.Angle.GetRotationDegrees(value, "Orientation");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("RotationY"))
            {
                double rotationY_degrees = Measurement.Angle.GetRotationDegrees(value, "RotationY");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("Tilt"))
            {
                double rotationY_degrees = Measurement.Angle.GetRotationDegrees(value, "Tilt");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("RotationX"))
            {
                double rotationX_degrees = Measurement.Angle.GetRotationDegrees(value, "RotationX");
                rotationX = new Quaternion(new Vector3D(0, 1, 0), rotationX_degrees);
            }
            if (value.Contains("Spin"))
            {
                double rotationX_degrees = Measurement.Angle.GetRotationDegrees(value, "Spin");
                rotationX = new Quaternion(new Vector3D(0, 1, 0), rotationX_degrees);
            }

            Quaternion total_rotation = Quaternion.Multiply(rotationX, Quaternion.Multiply(rotationY, rotationZ));
            return new RotateTransform3D(new QuaternionRotation3D(total_rotation));
        }
    }
}
