using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public sealed class Transform3DFactory : Generic.TargetTypeFactory<Transform3D>
    {
        public override Transform3D Create(object source)
        {
            if (source == null) return null;
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType()))
                {
                    return CreateFromIDictionary(source as IDictionary);
                }
            }
            return null;
        }

        private Transform3D CreateFromIDictionary(IDictionary source)
        {
            var matrix3D = new Matrix3D();

            var rotations = Extension.IDictionaryExtension.GetRotationsXYZ(source);
            matrix3D = Helpers.Matrix3DHelper.RotateXYZ(new Matrix3D(), Extension.IDictionaryExtension.GetRotationsXYZ(source));
            matrix3D.Translate(Extension.IDictionaryExtension.GetTranslation(source));

            if (!matrix3D.IsIdentity)
            {
                return new MatrixTransform3D { Matrix = matrix3D };
            }
            return null;
        }
    }
}
