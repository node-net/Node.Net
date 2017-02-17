using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Prototype.Internal.Factories
{
    sealed class Transform3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (ParentFactory != null)
            {
                var matrix3D = ParentFactory.Create(typeof(Matrix3D), source);
                if (matrix3D != null)
                {
                    return new MatrixTransform3D { Matrix = (Matrix3D)matrix3D };
                }
            }

            return null;
        }

        public IFactory ParentFactory { get; set; }
    }
}
