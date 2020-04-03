using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Internal
{
    internal sealed class Matrix3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null && source is IDictionary)
            {
                Matrix3D? matrix = CreateFromIDictionary(source as IDictionary);
                if (matrix.HasValue)
                {
                    return matrix.Value;
                }
            }

            return null;
        }

        public static IDictionary GetDictionary(Matrix3D matrix)
        {
            Dictionary<string, dynamic>? data = new Dictionary<string, dynamic>();
            //Vector3D rotationsZXY = matrix.GetRotationsZXY();
            Point3D translation = matrix.GetTranslation();
            data["X"] = $"{translation.X} m";
            data["Y"] = $"{translation.Y} m";
            data["Z"] = $"{translation.Z} m";
            Vector3D ots = matrix.GetRotationsOTS();
            if (Abs(ots.X) > 0.0001)
            {
                data["Orientation"] = $"{Round(ots.X, 4)} deg";
            }
            if (Abs(ots.Y) > 0.0001)
            {
                data["Tilt"] = $"{Round(ots.Y, 4)} deg";
            }
            if (Abs(ots.Z) > 0.0001)
            {
                data["Spin"] = $"{Round(ots.Z, 4)} deg";
            }
            //data["RotationX"] = $"{rotationsZXY.X} deg";
            //data["RotationY"] = $"{rotationsZXY.Y} deg";
            //data["RotationZ"] = $"{rotationsZXY.Z} deg";
            return data;
        }

        private static Matrix3D? CreateFromIDictionary(IDictionary dictionary)
        {
            StringBuilder? log = new StringBuilder();
            Matrix3D matrix3D = new Matrix3D();
            Vector3D xDirection = new Vector3D(1, 0, 0);
            Vector3D yDirection = new Vector3D(0, 1, 0);
            if (dictionary.Contains("XDirection"))
            {
                try
                {
                    if (dictionary.Contains("XDirection"))
                    {
                        string? xDirectionValue = dictionary.Get<string>("XDirection", "1,0,0");
                        log.Append(" XDirection = ").AppendLine(xDirectionValue);
                        xDirection = Vector3D.Parse(xDirectionValue);
                    }
                    if (dictionary.Contains("YDirection"))
                    {
                        string? yDirectionValue = dictionary.Get<string>("YDirection", "0,1,0");
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
            if (dictionary.Contains("Orientation") ||
                dictionary.Contains("Tilt") ||
                dictionary.Contains("Spin"))
            {
                matrix3D = RotateOTS(matrix3D, GetRotationsOTS(dictionary));
            }
            else
            {
                matrix3D = RotateXYZ(matrix3D, GetRotationsXYZ(dictionary));
            }
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
            if (source.Contains("Orientation") || source.Contains("Tilt") || source.Contains("Spin"))
            {
                double orientation = Internal.Angle.GetDegrees(source.Get<string>("Orientation"));
                double tilt = Internal.Angle.GetDegrees(source.Get<string>("Tilt"));
                double spin = Internal.Angle.GetDegrees(source.Get<string>("Spin"));
                Matrix3D matrix = new Matrix3D().RotateOTS(new Vector3D(orientation, tilt, spin));
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

        public static Vector3D GetRotationsOTS(IDictionary source)
        {
            double orientation = Internal.Angle.GetDegrees(source.Get<string>("Orientation"));
            double tilt = Internal.Angle.GetDegrees(source.Get<string>("Tilt"));
            double spin = Internal.Angle.GetDegrees(source.Get<string>("Spin"));
            return new Vector3D(orientation, tilt, spin);
        }

        public static Vector3D GetTranslation(IDictionary source)
        {
            return new Vector3D(
                Length.GetMeters(source.Get<string>("X")),
                Length.GetMeters(source.Get<string>("Y")),
                Length.GetMeters(source.Get<string>("Z")));
        }

        public static Matrix3D RotateXYZ(Matrix3D matrix, Vector3D rotationsXYZ) => matrix.RotateXYZ(rotationsXYZ);
        public static Matrix3D RotateOTS(Matrix3D matrix, Vector3D rotationsOTS) => matrix.RotateOTS(rotationsOTS);
    }
}