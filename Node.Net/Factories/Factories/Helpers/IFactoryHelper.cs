using Node.Net.Factories.Factories.Generic;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Factories.Helpers
{
    class IFactoryHelper
    {
        public static IFactory FromObject(object source, IFactory factory)
        {
            return CreateDefaultFactory();
        }

        public static IFactory CreateDefaultFactory()
        {
            var factory = new CompositeFactory();
            factory.Add(new FunctionAdapter3<IFactory, object>(FromObject));
            factory.Add(new FunctionAdapter2<IAngle, string>(IAngleHelper.FromString));
            factory.Add(new FunctionAdapter2<ILength, string>(ILengthHelper.FromString));
            factory.Add(new FunctionAdapter3<ITranslation, IDictionary>(ITranslationHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<IScale, IDictionary>(IScaleHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<IRotations, IDictionary>(IRotationsHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<IMatrix3D, IDictionary>(IMatrix3DHelper.FromIDictionary));
            factory.Add(new FunctionAdapter2<ITypeName, IDictionary>(ITypeNameHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<IColor, string>(IColorHelper.FromString));
            factory.Add(new FunctionAdapter3<Material, string>(MaterialHelper.FromString));
            factory.Add(new FunctionAdapter3<IPrimaryModel, IDictionary>(IPrimaryModelHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<Model3D, IDictionary>(Model3DHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<Visual3D, IDictionary>(Visual3DHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<double[], IList>(ArrayHelper.DoubleArrayFromIList));
            factory.Add(new FunctionAdapter3<double[,], IList>(ArrayHelper.DoubleArray2DFromIList));
            factory.Add(new FunctionAdapter3<ILocalToParent, IDictionary>(ILocalToParentHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<ILocalToWorld, IDictionary>(ILocalToWorldHelper.FromIDictionary));
            return factory;
        }
        public static IFactory CreateTransformFactory()
        {
            var factory = new CompositeFactory();
            factory.Add(new FunctionAdapter3<IFactory, object>(FromObject));
            factory.Add(new FunctionAdapter2<IAngle, string>(IAngleHelper.FromString));
            factory.Add(new FunctionAdapter2<ILength, string>(ILengthHelper.FromString));
            factory.Add(new FunctionAdapter3<ITranslation, IDictionary>(ITranslationHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<IRotations, IDictionary>(IRotationsHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<IMatrix3D, IDictionary>(IMatrix3DHelper.FromIDictionary));
            factory.Add(new FunctionAdapter2<ITypeName, IDictionary>(ITypeNameHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<IColor, string>(IColorHelper.FromString));
            factory.Add(new FunctionAdapter3<Material, string>(MaterialHelper.FromString));
            factory.Add(new FunctionAdapter3<IPrimaryModel, IDictionary>(IPrimaryModelHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<Model3D, IDictionary>(Model3DHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<Visual3D, IDictionary>(Visual3DHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<double[], IList>(ArrayHelper.DoubleArrayFromIList));
            factory.Add(new FunctionAdapter3<double[,], IList>(ArrayHelper.DoubleArray2DFromIList));
            factory.Add(new FunctionAdapter3<ILocalToParent, IDictionary>(ILocalToParentHelper.FromIDictionary));
            factory.Add(new FunctionAdapter3<ILocalToWorld, IDictionary>(ILocalToWorldHelper.FromIDictionary));
            return factory;
        }
    }
}
