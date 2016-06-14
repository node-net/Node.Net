using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Transformers
{
    public class Transform3DTransformer : TypeTransformer
    {
        public override object Transform(object item)
        {
            return null;
        }

        public static Transform3D Transform(string name)
        {
            return new TranslateTransform3D();
        }

        public static Transform3D Transform(IDictionary dictionary)
        {
            return new TranslateTransform3D();
        }

        public static Vector3D ToTranslation(IDictionary value)
        {
            var result = new Vector3D();
            if (value.Contains("X"))
            {
                result.X = GetLengthMeters(value, "X");
            }
            if (value.Contains("Y"))
            {
                result.Y = GetLengthMeters(value, "Y"); ;
            }
            if (value.Contains("Z"))
            {
                result.Z = GetLengthMeters(value, "Z");
            }
            return result;
        }

        public static RotateTransform3D GetRotateTransform3D(IDictionary value)
        {
            var rotationZ = new Quaternion();
            var rotationY = new Quaternion();
            var rotationX = new Quaternion();
            var rotation = new QuaternionRotation3D();
            if (value.Contains("RotationZ"))
            {
                var rotationZ_degrees = GetRotationDegrees(value, "RotationZ");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("Orientation"))
            {
                var rotationZ_degrees = GetRotationDegrees(value, "Orientation");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("RotationY"))
            {
                var rotationY_degrees = GetRotationDegrees(value, "RotationY");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("Tilt"))
            {
                var rotationY_degrees = GetRotationDegrees(value, "Tilt");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("RotationX"))
            {
                var rotationX_degrees = GetRotationDegrees(value, "RotationX");
                rotationX = new Quaternion(new Vector3D(0, 1, 0), rotationX_degrees);
            }
            if (value.Contains("Spin"))
            {
                var rotationX_degrees = GetRotationDegrees(value, "Spin");
                rotationX = new Quaternion(new Vector3D(0, 1, 0), rotationX_degrees);
            }

            var total_rotation = Quaternion.Multiply(rotationX, Quaternion.Multiply(rotationY, rotationZ));
            return new RotateTransform3D(new QuaternionRotation3D(total_rotation));
        }


        public static Vector3D ToScale(IDictionary value)
        {
            var result = new Vector3D(1, 1, 1);
            if (value.Contains("ScaleX"))
            {
                result.X = GetLengthMeters(value, "ScaleX");
            }
            if (value.Contains("Length"))
            {
                result.X = GetLengthMeters(value, "Length");
            }
            if (value.Contains("ScaleY"))
            {
                result.Y = GetLengthMeters(value, "ScaleY");
            }
            if (value.Contains("Width"))
            {
                result.Y = GetLengthMeters(value, "Width");
            }
            if (value.Contains("ScaleZ"))
            {
                result.Z = GetLengthMeters(value, "ScaleZ");
            }
            if (value.Contains("Height"))
            {
                result.Z = GetLengthMeters(value, "Height");
            }
            return result;
        }

        private static double GetLengthMeters(IDictionary dictionary, string key)
        {
            if (dictionary.Contains(key))
            {
                return Measurement.Length.Parse(dictionary[key].ToString())[Measurement.LengthUnit.Meters];
            }
            return 0;
        }
        private static double GetRotationDegrees(IDictionary dictionary, string key)
        {
            if (object.ReferenceEquals(null, dictionary)) return 0;
            if (!dictionary.Contains(key)) return 0;
            if (object.ReferenceEquals(null, dictionary[key])) return 0;
            return Measurement.Angle.Parse(dictionary[key].ToString())[Measurement.AngularUnit.Degrees];
        }
    }
}
