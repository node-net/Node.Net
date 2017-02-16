using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Prototype.Internal.Factories
{
    sealed class GeometryModel3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (ParentFactory != null)
            {

            }
            return null;
        }

        public IFactory ParentFactory { get; set; }
    }
}
