using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public class Point3DExtension
    {
        public static Point3D GetRotationOTSDegrees(IDictionary value)
        {
            Point3D rotationOTS = new Point3D();
            if (value.Contains("RotationZ"))
            {
                double rotationZ_degrees = Measurement.Angle.GetRotationDegrees(value, "RotationZ");
                rotationOTS.X = rotationZ_degrees;
            }
            if (value.Contains("Orientation"))
            {
                double rotationZ_degrees = Measurement.Angle.GetRotationDegrees(value, "Orientation");
                rotationOTS.X = rotationZ_degrees;
            }
            if (value.Contains("RotationY"))
            {
                double rotationY_degrees = Measurement.Angle.GetRotationDegrees(value, "RotationY");
                rotationOTS.Y = rotationY_degrees;
            }
            if (value.Contains("Tilt"))
            {
                double rotationY_degrees = Measurement.Angle.GetRotationDegrees(value, "Tilt");
                rotationOTS.Y = rotationY_degrees;
            }
            if (value.Contains("RotationX"))
            {
                double rotationX_degrees = Measurement.Angle.GetRotationDegrees(value, "RotationX");
                rotationOTS.Z = rotationX_degrees;
            }
            if (value.Contains("Spin"))
            {
                double rotationX_degrees = Measurement.Angle.GetRotationDegrees(value, "Spin");
                rotationOTS.Z = rotationX_degrees;
            }

            return rotationOTS;
        }
    }
}
