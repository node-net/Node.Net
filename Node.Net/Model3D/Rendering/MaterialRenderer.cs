namespace Node.Net.Model3D.Rendering
{
    class MaterialRenderer : Base
    {
        public MaterialRenderer(IRenderer renderer):base(renderer)
        { }
        public virtual System.Windows.Media.Media3D.Material GetMaterial(string name)
        {
            if (Renderer.Resources.Contains(name))
            {
                System.Windows.Media.Media3D.Material material =
                    Renderer.Resources[name] as System.Windows.Media.Media3D.Material;
                if (!object.ReferenceEquals(null, material)) return material;
            }
            // Factory (creation method)
            return Node.Net.Model3D.Material.GetDiffuse(System.Windows.Media.Colors.Blue);
        }
        public virtual System.Windows.Media.Media3D.Material GetMaterial(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (value.Contains("Material")) { return GetMaterial(value["Material"].ToString()); }
            return Node.Net.Model3D.Material.GetDiffuse(System.Windows.Media.Colors.Blue);
        }
        public virtual System.Windows.Media.Media3D.Material GetBackMaterial(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (value.Contains("BackMaterial")) { return GetMaterial(value["BackMaterial"].ToString()); }
            return Node.Net.Model3D.Material.GetDiffuse(System.Windows.Media.Colors.Yellow);
        }
    }
}
