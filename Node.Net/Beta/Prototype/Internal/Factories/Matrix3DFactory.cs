using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Prototype.Internal.Factories
{
    sealed class Matrix3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
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

        private static Matrix3D? CreateFromIDictionary(IDictionary dictionary)
        {

            var matrix3D = new Matrix3D();
            //var rotations = GetRotationsXYZ(dictionary);
            matrix3D = RotateXYZ(matrix3D, GetRotationsXYZ(dictionary));
            matrix3D.Translate(GetTranslation(dictionary));
            if (!matrix3D.IsIdentity) return matrix3D;
            /*
            var rotations = source.GetRotationsXYZ();// Extension.IDictionaryExtension.GetRotationsXYZ(source);
            matrix3D = Helpers.Matrix3DHelper.RotateXYZ(new Matrix3D(), source.GetRotationsXYZ());// Extension.IDictionaryExtension.GetRotationsXYZ(source));
            matrix3D.Translate(source.GetTranslation());// Extension.IDictionaryExtension.GetTranslation(source));

            if (!matrix3D.IsIdentity)
            {
                return new MatrixTransform3D { Matrix = matrix3D };
            }*/
            return null;
        }

        public static Vector3D GetRotationsXYZ(IDictionary source)
        {
            return new Vector3D(
                Internal.Units.Angle.GetDegrees(source.Get<string>("RotationX,Spin,Roll")),
                Internal.Units.Angle.GetDegrees(source.Get<string>("RotationY,Tilt,Pitch")),
                Internal.Units.Angle.GetDegrees(source.Get<string>("RotationZ,Orientation,Yaw")));
        }

        public static Vector3D GetTranslation(IDictionary source)
        {
            return new Vector3D(
                Internal.Units.Length.GetMeters(source.Get<string>("X")),
                Internal.Units.Length.GetMeters(source.Get<string>("Y")),
                Internal.Units.Length.GetMeters(source.Get<string>("Z")));
        }

        public static Matrix3D RotateXYZ(Matrix3D matrix, Vector3D rotationsXYZ)
        {
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), rotationsXYZ.Z));

            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));

            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, rotationsXYZ.X));
            return matrix;
        }
    }
}
