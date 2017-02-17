using System;
using System.Collections;
using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class MeshGeometry3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType())) return CreateFromDictionary(source as IDictionary);
            }
            if (ParentFactory != null)
            {
                if(source != null)
                {
                    //var stream = ParentFactory.Create<Stream>(source);
                    return Create(target_type, ParentFactory.Create<IDictionary>(source));
                }
            }
            return null;
        }

        public IFactory ParentFactory { get; set; }

        private MeshGeometry3D CreateFromDictionary(IDictionary source)
        {
            if (ParentFactory != null)
            {
                if (source != null)
                {
                    var type = source.Get<string>("Type");
                    return ParentFactory.Create<MeshGeometry3D>($"{type}.MeshGeometry3D.");
                }
            }

            return null;
        }
    }
}
