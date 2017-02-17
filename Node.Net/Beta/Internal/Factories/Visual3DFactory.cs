using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class Visual3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (ParentFactory != null)
            {
                var model3D = ParentFactory.Create<Model3D>(source);
                if (model3D != null) return new ModelVisual3D { Content = model3D };
            }
            return null;
        }
        public IFactory ParentFactory { get; set; }
    }
}
