using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Factories.Helpers
{
    static class ILocalToWorldHelper
    {
        class ConcreteLocalToWorld : ILocalToWorld { public Matrix3D LocalToWorld { get; set; } = new Matrix3D(); }
        public static ILocalToWorld FromIDictionary(IDictionary dictionary, IFactory factory)
        {
            IMatrix3D imatrix = IMatrix3DHelper.FromIDictionary(dictionary, factory);
            Matrix3D localToWorld = imatrix.Matrix3D;
            var parent = dictionary.GetParent();
            if(parent != null)
            {
                ILocalToWorld parentLocalToWorld = factory.Create<ILocalToWorld>(parent);
                localToWorld.Append(parentLocalToWorld.LocalToWorld);
            }
            
            return new ConcreteLocalToWorld { LocalToWorld = localToWorld };
        }
    }
}
