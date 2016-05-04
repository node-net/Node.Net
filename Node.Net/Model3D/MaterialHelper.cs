using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public class MaterialHelper
    {
        public static Material GetImageMaterial(ImageSource imageSource)
        {
            MaterialGroup material = new MaterialGroup();
            DiffuseMaterial diffuse = new DiffuseMaterial()
            {
                Brush = new ImageBrush()
                {
                    ImageSource = imageSource,
                    TileMode = TileMode.Tile
                }
            };
            material.Children.Add(diffuse);
            return material;
        }
    }
}
