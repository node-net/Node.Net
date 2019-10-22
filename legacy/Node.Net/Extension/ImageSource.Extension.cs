using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class ImageSourceExtension
    {
        public static Material GetMaterial(this ImageSource imageSource, System.Windows.Media.Brush specularBrush = null, double specularPower = 10) => Beta.Internal.ImageSourceExtension.GetMaterial(imageSource, specularBrush, specularPower);
    }
}
