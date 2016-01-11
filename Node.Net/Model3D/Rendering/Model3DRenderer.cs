namespace Node.Net.Model3D.Rendering
{
    class Model3DRenderer : Base
    {
        public Model3DRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer) { }
        private GeometryModel3DRenderer geometryModel3DRenderer = null;

        public GeometryModel3DRenderer GeometryModel3DRenderer
        {
            get { 
                if(object.ReferenceEquals(null,geometryModel3DRenderer))
                {
                    geometryModel3DRenderer = new GeometryModel3DRenderer(Renderer);
                }
                return geometryModel3DRenderer; 
            }
            set { geometryModel3DRenderer = value; }
        }
        public MatrixTransform3DRenderer MatrixTransform3DRenderer
        {
            get { return GeometryModel3DRenderer.MatrixTransform3DRenderer; }
            set { GeometryModel3DRenderer.MatrixTransform3DRenderer = value; }
        }

        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(string name)
        {
            if (Renderer.Resources.Contains(name))
            {
                System.Windows.Media.Media3D.Model3D model3D = Renderer.Resources[name] as System.Windows.Media.Media3D.Model3D;
                if (!object.ReferenceEquals(null, model3D)) return model3D;
            }
            return null;
        }
        
        public event Model3DRequestedEventHandler Model3DRequested;
        private bool rendererLocked = false;
        public System.Windows.Media.Media3D.Model3D GetModel3D(object value,
                                                      Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if(!object.ReferenceEquals(null,Model3DRequested))
            {
                System.Windows.Media.Media3D.Model3D model3D
                    = Model3DRequested(value, units);
                if (!object.ReferenceEquals(null, model3D)) return model3D;
            }

            if (object.ReferenceEquals(null, value)) return null;
            if (typeof(System.Windows.Media.Media3D.Model3D).IsAssignableFrom(value.GetType()))
                return value as System.Windows.Media.Media3D.Model3D;

            System.Collections.IDictionary childDictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, childDictionary))
            {
                System.Windows.Media.Media3D.Model3D childModel = GetModel3D(childDictionary, units);
                if (!object.ReferenceEquals(null, childModel))
                {
                    System.Windows.Media.Media3D.Model3DGroup model3DGroup = new System.Windows.Media.Media3D.Model3DGroup();
                    model3DGroup.Children.Add(childModel);
                    return model3DGroup;
                }
            }
            System.Windows.Media.Media3D.MeshGeometry3D mesh = value as System.Windows.Media.Media3D.MeshGeometry3D;
            if (!object.ReferenceEquals(null, mesh)) return GetModel3D(mesh);
            System.Windows.Media.Media3D.GeometryModel3D geoModel = value as System.Windows.Media.Media3D.GeometryModel3D;
            if (!object.ReferenceEquals(null, geoModel)) return GetModel3D(geoModel);

            if(!object.ReferenceEquals(null,Renderer))
            {
                if (!rendererLocked)
                {
                    rendererLocked = true;

                    try
                    {
                        return Renderer.GetModel3D(value);
                    }
                    finally { rendererLocked = false; }
                }
            }
            return null;
        }
        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            System.Windows.Media.Media3D.Model3DGroup model3DGroup = new System.Windows.Media.Media3D.Model3DGroup();

            // Primary Geometry
            string typeName = GetTypeString(value);
            System.Windows.Media.Media3D.Model3D model = GetModel3D(typeName);
            if(object.ReferenceEquals(null,model))
            {
                System.Windows.Media.Media3D.GeometryModel3D geometryModel3D
                    = GeometryModel3DRenderer.GetGeometryModel3D(value, units);
                if (!object.ReferenceEquals(null, geometryModel3D))
                {
                    model3DGroup.Children.Add(geometryModel3DRenderer.GetGeometryModel3D(value, units));
                }
            }
            else
            {
                model.Transform = MatrixTransform3DRenderer.GetMatrixTransform3D_ScaleOnly(value, units);
                model3DGroup.Children.Add(model);
            }

            // Children
            foreach (object key in value.Keys)
            {
                System.Windows.Media.Media3D.Model3D childModel = GetModel3D(value[key], units);
                if (!object.ReferenceEquals(null, childModel)) model3DGroup.Children.Add(childModel);
            }
            if (model3DGroup.Children.Count > 0)
            {
                model3DGroup.Transform = MatrixTransform3DRenderer.GetMatrixTransform3D_NoScale(value, units);
                return model3DGroup;
            }
            return null;
        }

        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(
            System.Windows.Media.Media3D.MeshGeometry3D mesh)
        {
            System.Windows.Media.Media3D.Model3DGroup model3DGroup = new System.Windows.Media.Media3D.Model3DGroup();
            model3DGroup.Children.Add(GeometryModel3DRenderer.GetGeometryModel3D(mesh));
            return model3DGroup;
        }

        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(
            System.Windows.Media.Media3D.GeometryModel3D geometryModel3D)
        {
            System.Windows.Media.Media3D.Model3DGroup model3DGroup = new System.Windows.Media.Media3D.Model3DGroup();
            model3DGroup.Children.Add(geometryModel3D);
            return model3DGroup;
        }
    }
}
