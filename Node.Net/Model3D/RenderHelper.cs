using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public class RenderHelper
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
        public static Vector3D GetTranslationMeters(object value)
        {
            Vector3D translation = new Vector3D();
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary))
            {
                if (dictionary.Contains("X"))
                {
                    translation.X = Measurement.Length.GetLengthMeters(dictionary, "X");
                }
                if (dictionary.Contains("Y"))
                {
                    translation.Y = Measurement.Length.GetLengthMeters(dictionary, "Y"); ;
                }
                if (dictionary.Contains("Z"))
                {
                    translation.Z = Measurement.Length.GetLengthMeters(dictionary, "Z");
                }
            }
            return translation;
        }
        public static Vector3D GetScaleMeters(object value)
        {
            Vector3D scale = new Vector3D(1, 1, 1);
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary))
            {
                if (dictionary.Contains("ScaleX"))
                {
                    scale.X = Measurement.Length.GetLengthMeters(dictionary, "ScaleX");
                }
                if (dictionary.Contains("Length"))
                {
                    scale.X = Measurement.Length.GetLengthMeters(dictionary, "Length");
                }
                if (dictionary.Contains("ScaleY"))
                {
                    scale.Y = Measurement.Length.GetLengthMeters(dictionary, "ScaleY");
                }
                if (dictionary.Contains("Width"))
                {
                    scale.Y = Measurement.Length.GetLengthMeters(dictionary, "Width");
                }
                if (dictionary.Contains("ScaleZ"))
                {
                    scale.Z = Measurement.Length.GetLengthMeters(dictionary, "ScaleZ");
                }
                if (dictionary.Contains("Height"))
                {
                    scale.Z = Measurement.Length.GetLengthMeters(dictionary, "Height");
                }
            }
            return scale;
        }
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
