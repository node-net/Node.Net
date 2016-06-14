using System.Windows.Media.Media3D;

namespace Node.Net.Model
{
    public class SpatialElement : Element, IModel3D
    {
        public Matrix3D LocalToParent { get; set; } = new Matrix3D();
    }
}
