using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class GeometryModel3DFactory : IFactory
    {
        public object Create(Type type, object value)
        {
            if (value.GetType() == typeof(MeshGeometry3D)) return Create(value as MeshGeometry3D);
            return null;
        }

        private GeometryModel3D Create(MeshGeometry3D mesh)
        {
            return new GeometryModel3D
            {
                Geometry = mesh
            };
        }
    }
}
