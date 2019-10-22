namespace Node.Net.Model3D.Rendering
{
    class MeshGeometry3DRenderer : Base
    {
        public MeshGeometry3DRenderer(IRenderer renderer)
            : base(renderer)
        {}
        public virtual System.Windows.Media.Media3D.MeshGeometry3D GetMeshGeometry3D(string name)
        {
            if (Renderer.Resources.Contains(name))
            {
                System.Windows.Media.Media3D.MeshGeometry3D mesh = Renderer.Resources[name] as System.Windows.Media.Media3D.MeshGeometry3D;
                if (!object.ReferenceEquals(null, mesh)) return mesh;
            }
            return null;
        }

        public System.Windows.Media.Media3D.MeshGeometry3D GetMeshGeometry3D(object value,
            Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            if (typeof(System.Windows.Media.Media3D.MeshGeometry3D).IsAssignableFrom(value.GetType()))
                return value as System.Windows.Media.Media3D.MeshGeometry3D;

            return GetMeshGeometry3D(value as System.Collections.IDictionary, units);
        }
        protected virtual System.Windows.Media.Media3D.MeshGeometry3D GetMeshGeometry3D(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            if (value.Contains("Type"))
            {
                string typeName = value["Type"].ToString();
                return GetMeshGeometry3D(typeName);
            }
            return null;
        }

    }
}
