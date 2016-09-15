using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Factories.Helpers
{
    public static class MaterialHelper
    {
        public static Material FromString(string source, IFactory factory)
        {
            if (factory != null)
            {
                var icolor = factory.Create<IColor>(source);
                if (icolor != null) return new DiffuseMaterial { Brush = new SolidColorBrush(icolor.Color) };
            }
            return null;
        }

        public static Material GetImageMaterial(ImageSource imageSource, Brush specularBrush = null, double specularPower = 10)
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
