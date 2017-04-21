using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net
{
    public static class Matrix3DExtension
    {
        public static Matrix3D RotateXYZ(this Matrix3D matrix, Vector3D rotationsXYZ)
        {
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), rotationsXYZ.Z));

            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));

            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, rotationsXYZ.X));
            return matrix;
        }
        public static Vector3D GetRotationsXYZ(this Matrix3D matrix)
        {
            var rotationZ = GetRotationZ(matrix);
            var rotationY = GetRotationY(matrix, rotationZ);
            return new Vector3D(GetRotationX(matrix, rotationZ, rotationY), rotationY, rotationZ);
        }

        public static double GetRotationZ(this Matrix3D matrix)
        {
            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            var localZ = matrix.Transform(new Vector3D(0, 0, 1));

            // rotation about the Z axis
            localX.Z = 0.0;
            var angle = Vector3D.AngleBetween(localX, new Vector3D(1, 0, 0));
            var rotationZ = localX.Y < 0.0 ? angle * -1.0 : angle;
            if (Abs(rotationZ - 180.0) > 0.01 && Abs(localX.Y) < 0.0001) rotationZ = 0.0;
            if (Abs(localZ.Z + 1.0) < 0.01) rotationZ = 0.0;
            return rotationZ;
        }
        public static double GetRotationY(this Matrix3D matrix)
        {
            return GetRotationY(matrix, GetRotationZ(matrix));
        }
        public static double GetRotationY(this Matrix3D matrix, double rotationZ)
        {
            // back off z rotation
            var matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));

            var localX = matrix2.Transform(new Vector3D(1, 0, 0));
            var localY = matrix2.Transform(new Vector3D(0, 1, 0));

            // rotation about the Y axis
            localX.Y = 0.0;
            var angle = Vector3D.AngleBetween(localX, new Vector3D(1, 0, 0));
            var rotationY = localX.Z > 0 ? angle * -1.0 : angle;
            if (Abs(rotationY - 180.0) > 0.01 && Abs(localX.Z) < 0.0001) rotationY = 0.0;
            if (Abs(localY.Y + 1.0) < 0.01) rotationY = 0.0;

            return rotationY;
        }

        public static double GetRotationX(this Matrix3D matrix)
        {
            var rotationZ = matrix.GetRotationZ();
            var rotationY = matrix.GetRotationY(rotationZ);
            return GetRotationX(matrix, rotationZ, rotationY);
        }
        public static double GetRotationX(this Matrix3D matrix, double rotationZ, double rotationY)
        {
            // back off z rotation and y rotation
            var matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));
            var matrix3 = new Matrix3D();
            matrix3.Append(matrix2);
            matrix3 = matrix3.RotateXYZ(new Vector3D(0.0, rotationY * -1.0, 0.0));


            //var localX = matrix2.Transform(new Vector3D(1, 0, 0));
            var localY = matrix3.Transform(new Vector3D(0, 1, 0));
            localY.X = 0.0;
            var angle = Vector3D.AngleBetween(localY, new Vector3D(0, 1, 0));
            var rotationX = localY.Z < 0 ? angle * -1.0 : angle;
            //if (Abs(rotationY - 180.0) > 0.01 && Abs(localX.Z) < 0.0001) rotationY = 0.0;
            //if (Abs(localY.Y + 1.0) < 0.01) rotationY = 0.0;

            // rotation about the X axis
            return rotationX;
        }
    }
}
