using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated
{
    public interface IModel3D : IChild
    {

        Matrix3D LocalToParent { get; }
    }
}
