namespace Node.Net.Model3D.Rendering
{
    class GeometryModel3DRenderer : Base
    {
        public GeometryModel3DRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer) { }
        private MaterialRenderer materialRenderer2 = null;//new MaterialRenderer();
        private MeshGeometry3DRenderer meshGeometry3DRenderer2 = null;//new MeshGeometry3DRenderer();
        private MatrixTransform3DRenderer matrixTransform3DRenderer2 = null;//new MatrixTransform3DRenderer();

        public MaterialRenderer MaterialRenderer
        {
            get { 
                if(object.ReferenceEquals(null,materialRenderer2))
                {
                    materialRenderer2 = new MaterialRenderer(Renderer);
                }
                return materialRenderer2; 
            }
            set { materialRenderer2 = value; }
        }
        public MeshGeometry3DRenderer MeshGeometry3DRenderer
        {
            get { 
                if(object.ReferenceEquals(null,meshGeometry3DRenderer2))
                {
                    meshGeometry3DRenderer2 = new MeshGeometry3DRenderer(Renderer);
                }
                return meshGeometry3DRenderer2; 
            }
            set { meshGeometry3DRenderer2 = value; }
        }

        public MatrixTransform3DRenderer MatrixTransform3DRenderer
        {
            get { 
                if(object.ReferenceEquals(null,matrixTransform3DRenderer2))
                {
                    matrixTransform3DRenderer2 = new MatrixTransform3DRenderer(Renderer);
                }
                return matrixTransform3DRenderer2; 
            }
            set { matrixTransform3DRenderer2 = value; }
        }

        public System.Windows.Media.Media3D.GeometryModel3D GetGeometryModel3D(object value,
            Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            if (typeof(System.Windows.Media.Media3D.GeometryModel3D).IsAssignableFrom(value.GetType())) 
                return value as System.Windows.Media.Media3D.GeometryModel3D;
            return GetGeometryModel3D(value as System.Collections.IDictionary, units);
        }
        protected virtual System.Windows.Media.Media3D.GeometryModel3D GetGeometryModel3D(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            System.Windows.Media.Media3D.GeometryModel3D result =
                new System.Windows.Media.Media3D.GeometryModel3D();
            result.Geometry = MeshGeometry3DRenderer.GetMeshGeometry3D(value, units);
            if (object.ReferenceEquals(null, result.Geometry)) return null;
            result.Material = MaterialRenderer.GetMaterial(value, units);
            result.BackMaterial = MaterialRenderer.GetBackMaterial(value, units);
            result.Transform = MatrixTransform3DRenderer.GetMatrixTransform3D_ScaleOnly(value, units);
            return result;
        }
        public System.Windows.Media.Media3D.GeometryModel3D GetGeometryModel3D(
            System.Windows.Media.Media3D.MeshGeometry3D mesh)
        {
            if (object.ReferenceEquals(null, mesh)) return null;
            System.Windows.Media.Media3D.GeometryModel3D result =
                new System.Windows.Media.Media3D.GeometryModel3D();
            result.Geometry = mesh;
            if (object.ReferenceEquals(null, result.Geometry)) return null;
            result.Material = Material.GetDiffuse(System.Windows.Media.Colors.Gray);
            result.BackMaterial = Material.GetDiffuse(System.Windows.Media.Colors.White);
            return result;
        }
    }
}
