using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node.Net.Factory.Factories.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.Helpers
{
    class IFactoryHelper
    {
        public static IFactory FromObject(object source,IFactory factory)
        {
            return CreateDefaultFactory();
        }

        public static IFactory CreateDefaultFactory()
        {
            var factory = new Factory();
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
            return factory;
        }
    }
}
