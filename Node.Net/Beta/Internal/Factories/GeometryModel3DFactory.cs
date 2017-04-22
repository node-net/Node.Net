using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class GeometryModel3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType())) return CreateFromDictionary(source as IDictionary);
                if (typeof(MeshGeometry3D).IsAssignableFrom(source.GetType())) return CreateFromMeshGeometry3D(source as MeshGeometry3D);
            }
            if (ParentFactory != null)
            {
                if (source != null)
                {
                    return Create(target_type, ParentFactory.Create<IDictionary>(source));
                }
            }
            return null;
        }

        public IFactory ParentFactory { get; set; }

        private GeometryModel3D CreateFromDictionary(IDictionary source)
        {
            if (ParentFactory != null)
            {
                if (source != null)
                {
                    var type = source.Get<string>("Type");
                    if (type.Length > 0)
                    {
                        var geometryModel3D = ParentFactory.Create<GeometryModel3D>($"{type}.GeometryModel3D.");
                        if (geometryModel3D != null) return geometryModel3D;
                        if (!locked)
                        {
                            try
                            {
                                locked = true;
                                geometryModel3D = ParentFactory.Create<GeometryModel3D>($"{type}.");
                                if (geometryModel3D != null) return geometryModel3D;
                            }
                            finally { locked = false; }
                        }
                        var mesh = ParentFactory.Create<MeshGeometry3D>(source);
                        if (mesh != null) return CreateFromMeshGeometry3D(mesh);
                    }
                }
            }

            return null;
        }
        private bool locked = false;
        private static GeometryModel3D CreateFromMeshGeometry3D(MeshGeometry3D mesh)
        {
            if (mesh == null) return null;
            return new GeometryModel3D
            {
                Geometry = mesh,
                Material = new DiffuseMaterial(Brushes.Blue)
            };

        }
    }
}
