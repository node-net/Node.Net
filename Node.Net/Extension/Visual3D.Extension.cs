using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Visual3DExtension
    {
        public static Model3D GetModel3D(this Visual3D v3d) => Beta.Internal.Visual3DExtension.GetModel3D(v3d);
        public static Rect3D FindBounds(this Visual3D visual)
        {
            return VisualTreeHelper.GetDescendantBounds(visual);
        }
        public static Visual3D Clone(this Visual3D v3d)
        {
            var memory = new MemoryStream();
            XamlWriter.Save(v3d, memory);
            memory.Flush();
            memory.Seek(0, SeekOrigin.Begin);
            return XamlReader.Load(memory) as Visual3D;
        }
    }
}
