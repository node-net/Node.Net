using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Visual3DExtension
    {
        public static Model3D GetModel3D(Visual3D v3d) => Beta.Internal.Visual3DExtension.GetModel3D(v3d);
    }
}
