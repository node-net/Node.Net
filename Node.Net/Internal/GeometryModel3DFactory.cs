using System;
using System.Collections;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
    internal sealed class GeometryModel3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null)
            {
                Type? sourceType = source.GetType();
                if (typeof(IDictionary).IsAssignableFrom(sourceType))
                {
                    return CreateFromDictionary(source as IDictionary);
                }

                if (typeof(MeshGeometry3D).IsAssignableFrom(sourceType))
                {
                    return CreateFromMeshGeometry3D(source as MeshGeometry3D);
                }

                if (ParentFactory != null)
                {
                    return Create(targetType, ParentFactory.Create<IDictionary>(source));
                }
            }
            return null;
        }

        public IFactory ParentFactory { get; set; }

        private GeometryModel3D CreateFromDictionary(IDictionary source)
        {
            if (source == null)
            {
                return null;
            }

            if (ParentFactory != null && source?.Contains("Type") == true)
            {
                string? type = source["Type"].ToString();
                if (type.Length > 0)
                {
                    string? name = $"GeometryModel3D.{type}.xaml";
                    GeometryModel3D? geometryModel3D = ParentFactory.Create<GeometryModel3D>(name);
                    if (geometryModel3D != null)
                    {
                        return geometryModel3D;
                    }

                    MeshGeometry3D? mesh = ParentFactory.Create<MeshGeometry3D>(source);
                    if (mesh != null)
                    {
                        return CreateFromMeshGeometry3D(mesh);
                    }
                }
            }

            return null;
        }

        private static GeometryModel3D CreateFromMeshGeometry3D(MeshGeometry3D mesh)
        {
            if (mesh == null)
            {
                return null;
            }

            return new GeometryModel3D
            {
                Geometry = mesh,
                Material = new DiffuseMaterial(Brushes.Blue)
            };
        }
    }
}