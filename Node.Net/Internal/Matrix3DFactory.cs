using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
    internal sealed class Matrix3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null && source is IDictionary)
            {
                var matrix = CreateFromIDictionary(source as IDictionary);
                if (matrix.HasValue)
                {
                    return matrix.Value;
                }
            }

            return null;
        }

        public static IDictionary GetDictionary(Matrix3D matrix)
        {
            var data = new Dictionary<string, dynamic>();
            var rotationsZXY = matrix.GetRotationsZXY();
            var translation = matrix.GetTranslation();
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
            var log = new StringBuilder();
            var matrix3D = new Matrix3D();
            var xDirection = new Vector3D(1, 0, 0);
            var yDirection = new Vector3D(0, 1, 0);
            if (dictionary.Contains("XDirection"))
            {
                try
                {
                    if (dictionary.Contains("XDirection"))
                    {
                        var xDirectionValue = dictionary.Get<string>("XDirection", "1,0,0");
                        log.Append(" XDirection = ").AppendLine(xDirectionValue);
                        xDirection = Vector3D.Parse(xDirectionValue);
                    }
                    if (dictionary.Contains("YDirection"))
                    {
                        var yDirectionValue = dictionary.Get<string>("YDirection", "0,1,0");
                        log.Append(" YDirection = ").AppendLine(yDirectionValue);
                        yDirection = Vector3D.Parse(yDirectionValue);
                    }
                }
                catch (Exception e)
                {
                    log.Append(" IDictionary FullName ").AppendLine(dictionary.GetFullName());
                    throw new InvalidOperationException($"Matrix3DFactory.CreateFromIDictionary{Environment.NewLine}{log}", e);
                }
                matrix3D = matrix3D.SetDirectionVectorsXY(xDirection, yDirection);
                matrix3D.Translate(GetTranslation(dictionary));
                return matrix3D;
            }
            matrix3D = RotateXYZ(matrix3D, GetRotationsXYZ(dictionary));
            matrix3D.Translate(GetTranslation(dictionary));
            if (!matrix3D.IsIdentity)
            {
                return matrix3D;
            }

            return null;
        }

        private const string RotationXKey = "Spin,RotationX,Roll";
        private const string RotationZKey = "Orientation,RotationZ,Yaw";

        public static string RotationYKey { get; set; } = "Tilt,RotationY,Pitch";

        public static Vector3D GetRotationsXYZ(IDictionary source)
        {
            if (source.Contains("Orientation"))
            {
                var orientation = Internal.Angle.GetDegrees(source.Get<string>("Orientation"));
                var tilt = Internal.Angle.GetDegrees(source.Get<string>("Tilt"));
                var spin = Internal.Angle.GetDegrees(source.Get<string>("Spin"));
                var matrix = new Matrix3D().RotateOTS(new Vector3D(orientation, tilt, spin));
                return matrix.GetRotationsXYZ();
            }
            else
            {
                return new Vector3D(
                    Internal.Angle.GetDegrees(source.Get<string>(RotationXKey)),
                    Internal.Angle.GetDegrees(source.Get<string>(RotationYKey)),
                    Internal.Angle.GetDegrees(source.Get<string>(RotationZKey)));
            }
        }

        public static Vector3D GetTranslation(IDictionary source)
        {
            return new Vector3D(
                Length.GetMeters(source.Get<string>("X")),
                Length.GetMeters(source.Get<string>("Y")),
                Length.GetMeters(source.Get<string>("Z")));
        }

        public static Matrix3D RotateXYZ(Matrix3D matrix, Vector3D rotationsXYZ) => matrix.RotateXYZ(rotationsXYZ);
    }
}