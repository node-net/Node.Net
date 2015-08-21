namespace Node.Net.Model3D
{
    public class PrimitivesRenderer : Renderer
    {
        public PrimitivesRenderer() { Initialize(); }

        private void Initialize()
        {
            Resources["Sunlight"] = Model3D.GetSunlight();
            Resources["Cube"] = MeshGeometry3D.CreateUnitCube();
            Resources["Cube.Top"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCube(), 0, 0, -0.5);
            Resources["Cube.Bottom"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCube(), 0, 0, 0.5);
            Resources["Cube.Back"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCube(), 0.5, 0, 0);
            Resources["Cube.Back.Bottom"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCube(), 0.5, 0, 0.5);
            Resources["Pyramid"] = MeshGeometry3D.CreateUnitPyramid();
            Resources["Pyramid.Top"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitPyramid(), 0, 0, -0.5);
            Resources["Pyramid.Bottom"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitPyramid(), 0, 0, 0.5);
            Resources["Cylinder"] = MeshGeometry3D.CreateUnitCylinder();
            Resources["Cylinder.Top"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCylinder(), 0, 0, -0.5);
            Resources["Cylinder.Bottom"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCylinder(), 0, 0, 0.5);
            Resources["Cone"] = MeshGeometry3D.CreateUnitCone();
            Resources["Cone.Top"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCone(), 0, 0, -0.5);
            Resources["Cone.Bottom"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCone(), 0, 0, 0.5);
            Resources["Sphere"] = MeshGeometry3D.CreateUnitSphere();
            Resources["Sphere.Top"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitSphere(), 0, 0, -0.5);
            Resources["Sphere.Bottom"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitSphere(), 0, 0, 0.5);
            Resources["Hemisphere"] = MeshGeometry3D.CreateUnitHemisphere();
            Resources["Hemisphere.Top"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitHemisphere(), 0, 0, -0.5);
            Resources["Hemisphere.Bottom"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitHemisphere(), 0, 0, 0.5);
            Resources["Red"] = Material.GetDiffuse(System.Windows.Media.Colors.Red);
            Resources["Blue"] = Material.GetDiffuse(System.Windows.Media.Colors.Blue);
            Resources["Yellow"] = Material.GetDiffuse(System.Windows.Media.Colors.Yellow);
            Resources["Green"] = Material.GetDiffuse(System.Windows.Media.Colors.Green);
            Resources["Purple"] = Material.GetDiffuse(System.Windows.Media.Colors.Purple);
            Resources["Orange"] = Material.GetDiffuse(System.Windows.Media.Colors.Orange);
            Resources["White"] = Material.GetDiffuse(System.Windows.Media.Colors.White);
            Resources["Gray"] = Material.GetDiffuse(System.Windows.Media.Colors.Gray);

            System.Windows.Media.Media3D.GeometryModel3D source
                = new System.Windows.Media.Media3D.GeometryModel3D();
            source.Geometry = Resources["Pyramid.Top"] as System.Windows.Media.Media3D.MeshGeometry3D;
            source.Material = Resources["Gray"] as System.Windows.Media.Media3D.Material;
            source.BackMaterial = Resources["White"] as System.Windows.Media.Media3D.Material;
            Resources["Source"] = source;
        }
    }
}
