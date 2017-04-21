using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Matrix3DExtension
    {
        //public static Matrix3D RotateXYZ(this Matrix3D matrix, Vector3D rotationsXYZ) => Node.Net.Beta.Internal.Factories.Matrix3DFactory.RotateXYZ(matrix, rotationsXYZ);
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
            return new Vector3D(0, 0, 0);
        }

    }
}
