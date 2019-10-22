using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal
{
    public static class Visual3DExtension
    {
        public static Model3D GetModel3D(Visual3D v3d)
        {
            if (v3d == null) return null;
            var model3DGroup = new Model3DGroup();
            var mv3d = v3d as ModelVisual3D;
            if (mv3d != null)
            {
                if (mv3d.Content != null) return mv3d.Content;
            }
            return model3DGroup;
        }
    }
}
