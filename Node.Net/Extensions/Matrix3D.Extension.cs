using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public static class Matrix3DExtension
    {
        public static Matrix3D RotateXYZ(Matrix3D matrix, Vector3D rotationsXYZ)
        {
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), rotationsXYZ.Z));

            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));

            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, rotationsXYZ.X));
            return matrix;
        }

        public static Point3D[] Transform(Matrix3D matrix, Point3D[] points)
        {
            var transformed_points = new List<Point3D>();
            foreach (var point in points)
            {
                transformed_points.Add(matrix.Transform(point));
            }

            return transformed_points.ToArray();
        }
    }
}
