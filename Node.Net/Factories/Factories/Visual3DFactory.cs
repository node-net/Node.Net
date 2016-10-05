using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Factories
{
    public sealed class Visual3DFactory : IFactory
    {
        public object Create(Type targetType,object source,IFactory helper)
        {
            return FromIDictionary(source as IDictionary, helper);
        }
        public static Visual3D FromIDictionary(IDictionary source, IFactory factory)
        {
            if (source == null) return null;
            var model3D = factory.Create<Model3D>(source, null);
            if (model3D != null)
            {
                return new ModelVisual3D { Content = model3D };
            }
            return null;
        }
    }
}
