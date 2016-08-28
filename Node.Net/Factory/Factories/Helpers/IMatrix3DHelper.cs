using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.Helpers
{
    public static class IMatrix3DHelper
    {
        class ConcreteMatrix3D : IMatrix3D { public Matrix3D Matrix3D { get; set; } = new Matrix3D(); }
        public static IMatrix3D FromIDictionary(IDictionary source,IFactory factory)
        {
            var matrix3D = new Matrix3D();
            var irotations = factory.Create<IRotations>(source);
            var iscale = factory.Create<IScale>(source);
            var itranslation = factory.Create<ITranslation>(source);
            if(irotations != null)
            {
                matrix3D = RotateXYZ(new Matrix3D(), irotations.RotationsXYZ);
            }
            if(iscale != null) matrix3D.Scale(iscale.Scale);
            if (itranslation != null) matrix3D.Translate(itranslation.Translation);

            if(!matrix3D.IsIdentity)
            {
                return new ConcreteMatrix3D { Matrix3D = matrix3D };
            }
            return null;
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
