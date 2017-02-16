using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public static class ImageSourceExtension
    {
        public static Material GetImageMaterial(this ImageSource imageSource, Brush specularBrush = null, double specularPower = 10)
        {
            var material = new MaterialGroup();
            var diffuse = new DiffuseMaterial
            {
                Brush = new ImageBrush
                {
                    ImageSource = imageSource,
                    TileMode = TileMode.Tile
                }
            };
            material.Children.Add(diffuse);
            if (!ReferenceEquals(null, specularBrush))
            {
                var specular = new SpecularMaterial
                {
                    Brush = specularBrush,
                    SpecularPower = specularPower
                };
                material.Children.Add(specular);
            }
            return material;
        }
    }
}
