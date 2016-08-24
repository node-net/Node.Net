using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class Model3DFactory : IFactory
    {
        public IFactory HelperFactory = new Factories.DefaultFactory();
        /*
        {
            get { return Node.Net.Factory.Factory.Default; }
        }*/
        public object Create(Type type, object value)
        {
            if (value == null) return null;
            var modelVisual3D = new System.Windows.Media.Media3D.ModelVisual3D();
            var typeName = HelperFactory.Create<ITypeName>(value).TypeName;
            if (typeName.Length > 0)
            {
                var model3D = HelperFactory.Create<System.Windows.Media.Media3D.Model3D>(typeName);
                if (model3D != null)
                {
                    var model3DGroup = new Model3DGroup
                    {
                        Transform = HelperFactory.Create<Transform3D>(value)
                    };
                    model3DGroup.Children.Add(model3D);
                    return model3D;
                }
                //return modelVisual3D;
            }
            return null;
        }


    }
}
