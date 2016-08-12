using System;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class Matrix3DFactory : IFactory
    {
        public object Create(Type type,object value)
        {
            var matrix3D = RotateXYZ(new Matrix3D(), IRotationsFactory.Default.Create<IRotations>(value).RotationsXYZ);
            matrix3D.Translate(ITranslationFactory.Default.Create<ITranslation>(value).Translation);
            return matrix3D;
        }

        public static Matrix3D RotateXYZ(Matrix3D matrix, Vector3D rotationsXYZ)
        {
            matrix.Rotate(new Quaternion(new Vector3D(1, 0, 0), rotationsXYZ.X));

            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));

            var localZ = matrix.Transform(new Vector3D(0, 0, 1));
            matrix.Rotate(new Quaternion(localZ, rotationsXYZ.Z));
            return matrix;
        }
    }
}
