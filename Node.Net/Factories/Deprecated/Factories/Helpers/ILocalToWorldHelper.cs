using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    static class ILocalToWorldHelper
    {
        private static IFactory transformFactory = Helpers.IFactoryHelper.CreateTransformFactory();
        class ConcreteLocalToWorld : ILocalToWorld { public Matrix3D LocalToWorld { get; set; } = new Matrix3D(); }
        public static ILocalToWorld FromIDictionary(IDictionary dictionary, IFactory factory)
        {
            IMatrix3D imatrix = transformFactory.Create<IMatrix3D>(dictionary,null);// IMatrix3DHelper.FromIDictionary(dictionary, transformFactory);
            Matrix3D localToWorld = imatrix.Matrix3D;
            var parent = dictionary.GetParent();
            if(parent != null)
            {
                ILocalToWorld parentLocalToWorld = transformFactory.Create<ILocalToWorld>(parent,null);
                localToWorld.Append(parentLocalToWorld.LocalToWorld);
            }

            return new ConcreteLocalToWorld { LocalToWorld = localToWorld };
        }
    }
}
