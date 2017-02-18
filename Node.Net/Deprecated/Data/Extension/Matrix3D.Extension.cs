using System.Windows.Media.Media3D;

namespace Node.Net.Data.Deprecated.Extension
{
    static class Matrix3DExtension
    {
        public static Matrix3D RotateXYZ(Matrix3D matrix, double xRotationDegrees, double yRotationDegrees, double zRotationDegrees)
        {
            matrix.Rotate(new Quaternion(new Vector3D(1, 0, 0), xRotationDegrees));

            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, yRotationDegrees));

            var localZ = matrix.Transform(new Vector3D(0, 0, 1));
            matrix.Rotate(new Quaternion(localZ, zRotationDegrees));
            return matrix;
        }
    }
}
