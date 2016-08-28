using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.Helpers
{
    public static class Model3DHelper
    {
        public static Model3D FromIDictionary(IDictionary source, IFactory factory)
        {
            if (source == null) return null;
            var model3DGroup = new Model3DGroup { Transform = GetTransform3D(source, factory) };
            var model3D = GetPrimaryModel3D(source, factory);
            if (model3D != null)
            {
                model3DGroup.Children.Add(model3D);
            }
            foreach (var key in source.Keys)
            {
                var childDictionary = source[key] as IDictionary;
                if (childDictionary != null)
                {
                    var childV3D = FromIDictionary(childDictionary, factory);
                    if (childV3D != null) model3DGroup.Children.Add(childV3D);
                }
            }
            if (model3DGroup.Children.Count > 0) return model3DGroup;
            return null;
        }

        private static Model3D GetPrimaryModel3D(IDictionary source, IFactory factory)
        {
            IPrimaryModel primaryModel = null;
            if (factory != null)
            {
                primaryModel = factory.Create<IPrimaryModel>(source);
            }
            if (primaryModel != null) return primaryModel.Model3D;
            return null;
        }

        private static Transform3D GetTransform3D(IDictionary source, IFactory factory)
        {
            // transform should NOT include Scale

            ITranslation itranslation = null;
            IRotations irotations = null;
            if (factory != null)
            {
                itranslation = factory.Create<ITranslation>(source);
                irotations = factory.Create<IRotations>(source);
                var matrix3D = new Matrix3D();
                if (irotations != null) matrix3D = IMatrix3DHelper.RotateXYZ(new Matrix3D(), irotations.RotationsXYZ);
                if (itranslation != null) matrix3D.Translate(itranslation.Translation);

                if (!matrix3D.IsIdentity)
                {
                    return new MatrixTransform3D { Matrix = matrix3D };
                }
            }
            return null;
        }
    }
}
