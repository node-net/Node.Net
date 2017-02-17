using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class MeshGeometry3DFactory : IFactory
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
