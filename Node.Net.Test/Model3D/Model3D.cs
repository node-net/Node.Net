namespace Node.Net.Model3D
{
    public class Model3D
    {
        public static System.Windows.Media.Media3D.Model3D GetSunlight()
        {
            System.Windows.Media.Media3D.Model3DGroup sunlight
                = new System.Windows.Media.Media3D.Model3DGroup();
            System.Windows.Media.Media3D.DirectionalLight directionalLight
                = new System.Windows.Media.Media3D.DirectionalLight(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF999999"),
                    new System.Windows.Media.Media3D.Vector3D(-0.32139380484327, 0.383022221559489, -0.866025403784439));
            sunlight.Children.Add(directionalLight);
            System.Windows.Media.Media3D.AmbientLight ambientLight
                = new System.Windows.Media.Media3D.AmbientLight(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF666666"));
            sunlight.Children.Add(ambientLight);
            return sunlight;
        }

        public static System.Windows.Media.Media3D.Model3D GetCube()
        {
            System.Windows.Media.Media3D.GeometryModel3D geometryModel
               = new System.Windows.Media.Media3D.GeometryModel3D();
            geometryModel.Material = new System.Windows.Media.Media3D.DiffuseMaterial(System.Windows.Media.Brushes.Blue);
            geometryModel.Geometry = MeshGeometry3D.CreateUnitCube();
            return geometryModel;
        }

        public static System.Windows.Media.Media3D.Model3D GetPyramid()
        {
            System.Windows.Media.Media3D.GeometryModel3D geometryModel
               = new System.Windows.Media.Media3D.GeometryModel3D();
            geometryModel.Material = new System.Windows.Media.Media3D.DiffuseMaterial(System.Windows.Media.Brushes.Blue);
            geometryModel.Geometry = MeshGeometry3D.CreateUnitPyramid();
            return geometryModel;
        }
    }
}
