namespace Node.Net.Model3D.Test
{
    [NUnit.Framework.TestFixture]
    class CompositeRenderer_Test
    {
        class WidgetRenderer : Renderer
        {
            public override System.Windows.Media.Media3D.Model3D GetModel3D(object value)
            {
                System.Collections.IDictionary dictionary = value as System.Collections.IDictionary;
                if(!object.ReferenceEquals(null,dictionary))
                {
                    if(dictionary.Contains("Widget") || dictionary.Contains("Type") && dictionary["Type"].ToString() == "Widget")
                    {
                        System.Windows.Media.Media3D.Model3DGroup group = new System.Windows.Media.Media3D.Model3DGroup();
                        return group;
                    }
                }
                return base.GetModel3D(value);
            }
        }
        [NUnit.Framework.TestCase, NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
        public void CompositeRenderer_Usage()
        {
            Node.Net.Json.Hash cubeModel = new Json.Hash("{'Cube':{'Type':'Cube'}}");
            Renderer cubeRenderer = new Renderer();
            cubeRenderer.Resources["Cube"] = MeshGeometry3D.CreateUnitCube();
            NUnit.Framework.Assert.NotNull(cubeRenderer.GetModel3D(cubeModel));

            Node.Net.Json.Hash sphereModel = new Json.Hash("{'Sphere':{'Type':'Sphere'}}");
            Renderer sphereRenderer = new Renderer();
            sphereRenderer.Resources["Sphere"] = MeshGeometry3D.CreateUnitSphere();
            NUnit.Framework.Assert.NotNull(sphereRenderer.GetModel3D(sphereModel));
            NUnit.Framework.Assert.IsNull(sphereRenderer.GetModel3D(cubeModel));

            Node.Net.Json.Hash widgetModel = new Json.Hash("{'Widget':{'Type':'Widget'}}");
            WidgetRenderer widgetRenderer = new WidgetRenderer();
            NUnit.Framework.Assert.NotNull(widgetRenderer.GetModel3D(widgetModel),"widgetRenderer failed to render widgetModel");
            NUnit.Framework.Assert.NotNull(widgetRenderer.GetViewport3D(widgetModel), "widgetRenderer failed to render Viewport3D for widgetModel");

            CompositeRenderer compositeRenderer = new CompositeRenderer();
            compositeRenderer.Renderers.Add(cubeRenderer);
            compositeRenderer.Renderers.Add(sphereRenderer);
            compositeRenderer.Renderers.Add(widgetRenderer);
            NUnit.Framework.Assert.NotNull(compositeRenderer.GetModel3D(cubeModel));
            NUnit.Framework.Assert.NotNull(compositeRenderer.GetModel3D(sphereModel));
            NUnit.Framework.Assert.NotNull(compositeRenderer.GetModel3D(widgetModel), "compositeRenderer failed to render widgetModel");

        }
    }
}