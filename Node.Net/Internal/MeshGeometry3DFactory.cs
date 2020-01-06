using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
    internal sealed class MeshGeometry3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null && source is IDictionary)
            {
                return CreateFromDictionary(source as IDictionary);
            }
            if (ParentFactory != null && source != null)
            {
                return Create(targetType, ParentFactory.Create<IDictionary>(source));
            }
            return null;
        }

        public IFactory ParentFactory { get; set; }

        private readonly Dictionary<string, MeshGeometry3D> cache = new Dictionary<string, MeshGeometry3D>();

        private MeshGeometry3D CreateFromDictionary(IDictionary source)
        {
            if (ParentFactory != null && source != null)
            {
                var type = source.Get<string>("Type");
                var name = $"MeshGeometry3D.{type}.xaml";
                if (cache.ContainsKey(name))
                {
                    return cache[name];
                }

                var mesh = ParentFactory.Create<MeshGeometry3D>(name);
                cache.Add(name, mesh);
                return mesh;
            }

            return null;
        }
    }
}