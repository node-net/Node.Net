using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.TypeFactories
{
    public class MeshGeometry3DFactory : Generic.TypeFactory<MeshGeometry3D>, IFactory
    {
        public object Create(Type type, object value)
        {
            if (value == null) return null;
            return null;
        }
        public static MeshGeometry3D GetRectangularMesh2(double width_meters, double height_meters)
        {
            var mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(width_meters / 2.0, height_meters / -2.0, 0));
            mesh.Positions.Add(new Point3D(width_meters / -2.0, height_meters / -2.0, 0));
            mesh.Positions.Add(new Point3D(width_meters / -2.0, height_meters / 2.0, 0));
            mesh.Positions.Add(new Point3D(width_meters / 2.0, height_meters / 2.0, 0));
            mesh.Normals.Add(new Vector3D(0, 0, 1));
            mesh.Normals.Add(new Vector3D(0, 0, 1));
            mesh.Normals.Add(new Vector3D(0, 0, 1));
            mesh.Normals.Add(new Vector3D(0, 0, 1));
            mesh.TextureCoordinates.Add(new Point(1, 1));
            mesh.TextureCoordinates.Add(new Point(0, 1));
            mesh.TextureCoordinates.Add(new Point(0, 0));
            mesh.TextureCoordinates.Add(new Point(1, 0));
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            return mesh;
        }
    }
}
