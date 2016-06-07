using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public interface IVisual3DTransformer
    {
        Visual3D GetVisual3D(object value);
    }
}
