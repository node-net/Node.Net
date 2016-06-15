using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public class MeshHelper
    {
        public static MeshGeometry3D GetRectangularMesh(double width_meters, double height_meters)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
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
