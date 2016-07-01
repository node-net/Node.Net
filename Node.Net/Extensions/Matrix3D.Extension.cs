using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public static class Matrix3DExtension
    {
        public static Matrix3D RotateLocal(Matrix3D m, Vector3D axis, double rotation_degrees)
        {
            var result = Matrix3D.Multiply(m, new Matrix3D());
            result.Rotate(new Quaternion(axis, rotation_degrees));
            return result;
        }
    }
}
