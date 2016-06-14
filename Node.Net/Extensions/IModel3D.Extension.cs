using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public static class IModel3DExtension
    {
        public static void Update(IModel3D model3D)
        {
            UpdateLocalToParent(model3D);

            var parent = model3D as IParent;
            if(parent != null)
            {
                var deepModels = parent.DeepCollect<IModel3D>();
                foreach(var deepModel in deepModels)
                {
                    UpdateLocalToParent(deepModel);
                }
            }
        }

        public static void UpdateLocalToParent(IModel3D model3D)
        {
            if (model3D != null)
            {
                var matrix = new Matrix3D();
                matrix.Rotate(GetQuaternionRotation(model3D as IDictionary));
                matrix.Translate(GetTranslationVector3D(model3D as IDictionary));
                model3D.LocalToParent = matrix;
            }
        }
        public static Vector3D GetTranslationVector3D(IDictionary value)
        {
            if (value == null) return new Vector3D();
            var result = new Vector3D();
            if (value != null)
            {
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
            }
            return result;
        }


        private static double GetLengthMeters(IDictionary dictionary, string key)
        {
            return Measurement.Length.Parse(dictionary[key].ToString())[Measurement.LengthUnit.Meters];
        }
        private static double GetRotationDegrees(IDictionary dictionary, string key)
        {
            if (object.ReferenceEquals(null, dictionary)) return 0;
            if (!dictionary.Contains(key)) return 0;
            if (object.ReferenceEquals(null, dictionary[key])) return 0;
            return Measurement.Angle.Parse(dictionary[key].ToString())[Measurement.AngularUnit.Degrees];
        }

        public static Quaternion GetQuaternionRotation(IDictionary value)
        {
            if (value == null) return new Quaternion();
            var rotationZ = new Quaternion();
            var rotationY = new Quaternion();
            var rotationX = new Quaternion();
            //QuaternionRotation3D rotation = new QuaternionRotation3D();
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
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }
            if (value.Contains("Spin"))
            {
                var rotationX_degrees = GetRotationDegrees(value, "Spin");
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }

            var total_rotation = Quaternion.Multiply(rotationX, Quaternion.Multiply(rotationY, rotationZ));
            return total_rotation;
            //return new RotateTransform3D(new QuaternionRotation3D(total_rotation));
        }
    }
}
