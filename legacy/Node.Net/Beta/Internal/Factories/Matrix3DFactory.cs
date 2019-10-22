using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class Matrix3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType()))
                {
                    var matrix = CreateFromIDictionary(source as IDictionary);
                    if (matrix.HasValue) return matrix.Value;
                }
            }

            return null;
        }

        public static IDictionary GetDictionary(Matrix3D matrix)
        {
            var data = new Dictionary<string, dynamic>();
            var rotationsZXY = Matrix3DExtension.GetRotationsZXY(matrix);
            var translation = Matrix3DExtension.GetTranslation(matrix);
            data["X"] = $"{translation.X} m";
            data["Y"] = $"{translation.Y} m";
            data["Z"] = $"{translation.Z} m";
            data["RotationX"] = $"{rotationsZXY.X} deg";
            data["RotationY"] = $"{rotationsZXY.Y} deg";
            data["RotationZ"] = $"{rotationsZXY.Z} deg";
            return data;
        }
        private static Matrix3D? CreateFromIDictionary(IDictionary dictionary)
        {
            var matrix3D = new Matrix3D();

            if(dictionary.Contains("XDirection"))
            {
                var xDirection = Vector3D.Parse(dictionary.Get<string>("XDirection", "1,0,0"));
                var yDirection = Vector3D.Parse(dictionary.Get<string>("YDirection", "0,1,0"));
                matrix3D = matrix3D.SetDirectionVectorsXY(xDirection, yDirection);
                matrix3D.Translate(GetTranslation(dictionary));
                return matrix3D;
            }
            matrix3D = RotateXYZ(matrix3D, GetRotationsXYZ(dictionary));
            matrix3D.Translate(GetTranslation(dictionary));
            if (!matrix3D.IsIdentity) return matrix3D;
            return null;
        }

        public static string RotationXKey = "Spin,RotationX,Roll";
        public static string RotationYKey = "Tilt,RotationY,Pitch";
        public static string RotationZKey = "Orientation,RotationZ,Yaw";
        public static Vector3D GetRotationsXYZ(IDictionary source)
        {
            return new Vector3D(
                Internal.Units.Angle.GetDegrees(source.Get<string>(RotationXKey)),
                Internal.Units.Angle.GetDegrees(source.Get<string>(RotationYKey)),
                Internal.Units.Angle.GetDegrees(source.Get<string>(RotationZKey)));
        }

        public static Vector3D GetTranslation(IDictionary source)
        {
            return new Vector3D(
                Internal.Units.Length.GetMeters(source.Get<string>("X")),
                Internal.Units.Length.GetMeters(source.Get<string>("Y")),
                Internal.Units.Length.GetMeters(source.Get<string>("Z")));
        }

        public static Matrix3D RotateXYZ(Matrix3D matrix, Vector3D rotationsXYZ) => matrix.RotateXYZ(rotationsXYZ);
    }
}
