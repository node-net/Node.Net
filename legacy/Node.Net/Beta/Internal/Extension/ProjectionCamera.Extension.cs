using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Beta.Internal
{
    static class ProjectionCameraExtension
    {
        public static Matrix3D GetLocalToWorld(ProjectionCamera camera)
        {
            return GetCameraMatrix3D(camera.Position, camera.LookDirection, camera.UpDirection);
        }
        public static Matrix3D GetWorldToLocal(ProjectionCamera camera)
        {
            var tmp = GetLocalToWorld(camera);
            tmp.Invert();
            return tmp;
        }
        public static Matrix3D GetCameraMatrix3D(Point3D position, Vector3D lookDirection, Vector3D upDirection)
        {
            var matrix = new Matrix3D();
            var zAngle = Vector3D.AngleBetween(lookDirection, new Vector3D(0, 0, -1));
            if (zAngle != 0.0)
            {
                var normal = Vector3D.CrossProduct(lookDirection, new Vector3D(0, 0, -1));
                matrix.Rotate(new Quaternion(normal, zAngle));
                if (Round(matrix.Transform(new Vector3D(0, 0, -1)).Z, 4) != -1.0)
                {
                    matrix = new Matrix3D();
                    matrix.Rotate(new Quaternion(normal, -zAngle));
                }


            }
            var localYAxis = matrix.Transform(new Vector3D(0, 1, 0));
            var yAngle = Vector3D.AngleBetween(upDirection, localYAxis);
            {
                if (Round(yAngle, 5) != 0.0)
                {
                    var normal = Vector3D.CrossProduct(upDirection, localYAxis);
                    matrix.Rotate(new Quaternion(normal, yAngle));
                    if (Round(matrix.Transform(localYAxis).Y, 4) != Round(localYAxis.Y, 4))
                    {
                        matrix.Rotate(new Quaternion(normal, yAngle * -2.0));
                    }
                }
            }

            var localNegZAxis = matrix.Transform(new Vector3D(0, 0, -1));
            zAngle = Vector3D.AngleBetween(lookDirection, localNegZAxis);
            if (Round(zAngle, 5) != 0.0)
            {
                var normal = Vector3D.CrossProduct(lookDirection, localNegZAxis);
                matrix.Rotate(new Quaternion(normal, zAngle));
                if (Round(matrix.Transform(localNegZAxis).Z, 4) != Round(localNegZAxis.Z, 4))
                {
                    matrix.Rotate(new Quaternion(normal, zAngle * -2.0));
                }
            }

            matrix.Translate(new Vector3D(position.X, position.Y, position.Z));
            return matrix;
        }

    }
}
