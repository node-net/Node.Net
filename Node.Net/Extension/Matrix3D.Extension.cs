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
        public static Matrix3D RotateXYZ(this Matrix3D matrix, Vector3D rotationsXYZ) => Node.Net.Beta.Internal.Factories.Matrix3DFactory.RotateXYZ(matrix, rotationsXYZ);
    }
}
