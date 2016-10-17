using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Helpers
{
    class Matrix3DHelper
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
    }
}
