using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.Helpers
{
    public static class Model3DHelper
    {
        public static Model3D FromIDictionary(IDictionary source,IFactory factory)
        {
            if (source == null) return null;
            var model3DGroup = new Model3DGroup { Transform = GetTransform3D(source,factory) };
            var model3D = GetPrimaryModel3D(source,factory);
            if (model3D != null)
            {
                model3DGroup.Children.Add(model3D);
            }
            foreach (var key in source.Keys)
            {
                var childDictionary = source[key] as IDictionary;
                if (childDictionary != null)
                {
                    var childV3D = FromIDictionary(childDictionary,factory);
                    if (childV3D != null) model3DGroup.Children.Add(childV3D);
                }
            }
            if (model3DGroup.Children.Count > 0) return model3DGroup;
            return null;
        }

        private static Model3D GetPrimaryModel3D(IDictionary source,IFactory factory)
        {
            IPrimaryModel primaryModel = null;
            if (factory != null)
            {
                primaryModel = factory.Create<IPrimaryModel>(source);
            }
            if (primaryModel != null) return primaryModel.Model3D;
            return null;
        }

        private static Transform3D GetTransform3D(IDictionary source,IFactory factory)
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
            //if (itranslation == null) itranslation = HelperFactory.Create<ITranslation>(source);
            //if (irotations == null) irotations = HelperFactory.Create<IRotations>(source);

            

            return null;
        }
        /*
        private IFactory HelperFactory
        {
            get
            {
                if (helperFactory == null)
                {
                    var factory = new Factories.CompositeFactory();
                    factory.Add(nameof(ITypeName), new Factories.TypeFactories.ITypeNameFactory());
                    factory.Add(nameof(IAngle), new Factories.TypeFactories.IAngleFactory());
                    factory.Add(nameof(ILength), new Factories.TypeFactories.ILengthFactory());
                    factory.Add(nameof(IScale), new Factories.TypeFactories.IScaleFactory());
                    factory.Add(nameof(IRotations), new Factories.TypeFactories.IRotationsFactory());
                    factory.Add(nameof(ITranslation), new Factories.TypeFactories.ITranslationFactory());
                    factory.Add("Transform3D", new Factories.TypeFactories.Transform3DFactory());
                    factory.Add(nameof(IPrimaryModel), new Factories.TypeSourceFactories.IPrimaryModelFromIDictionary { Parent = this });
                    helperFactory = factory;
                }
                return helperFactory;
            }
        }
        private IFactory helperFactory;
        */
    }
}
