namespace Node.Net.Model3D
{
    public class Material
    {
        public static System.Windows.Media.Media3D.Material GetDiffuse(System.Windows.Media.Color color)
        {
            System.Windows.Media.Media3D.DiffuseMaterial diffuse =
                new System.Windows.Media.Media3D.DiffuseMaterial(
                      new System.Windows.Media.SolidColorBrush(color));
            return diffuse;
        }
    }
}
