namespace Node.Net.Model3D
{
    public class Viewport3D
    {
        public static void Update(System.Windows.Controls.Viewport3D viewport)
        {
            viewport.Resources = new System.Windows.ResourceDictionary();
        }

        public static System.Collections.Generic.List<System.Windows.Media.Media3D.Visual3D> GetVisual3DChildren(System.Windows.Controls.Viewport3D viewport)
        {
            System.Collections.Generic.List<System.Windows.Media.Media3D.Visual3D> visual3DChildren
                = new System.Collections.Generic.List<System.Windows.Media.Media3D.Visual3D>();
            foreach (System.Windows.Media.Media3D.Visual3D v3d in viewport.Children)
            {
                visual3DChildren.Add(v3d);
            }
            return visual3DChildren;
        }
    }
}
