using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Model3D.Rendering
{
    class Visual3DRenderer : Base
    {
        public Visual3DRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer)
        {
        }
        private Model3DRenderer model3DRenderer = null;
        public Model3DRenderer Model3DRenderer
        {
            get
            {
                if (object.ReferenceEquals(null, model3DRenderer))
                {
                    model3DRenderer = new Model3DRenderer(Renderer);
                }
                return model3DRenderer;
            }
            set { model3DRenderer = value; }
        }

        public System.Windows.Media.Media3D.Visual3D GetVisual3D(object value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            if (typeof(System.Windows.Media.Media3D.ModelVisual3D).IsAssignableFrom(value.GetType()))
                return value as System.Windows.Media.Media3D.ModelVisual3D;

            IDictionary dictionary = value as IDictionary;
            if(!object.ReferenceEquals(null,dictionary))
            {
                if(dictionary.Contains("Visual3D"))
                {
                    if(Renderer.Resources.Contains(dictionary["Visual3D"].ToString()))
                    {
                        System.Windows.Media.Media3D.Visual3D visual3D =
                            Renderer.Resources[dictionary["Visual3D"].ToString()] as
                               System.Windows.Media.Media3D.Visual3D;
                        if (!object.ReferenceEquals(null, visual3D)) return visual3D;
                    }
                }
            }
            System.Windows.Media.Media3D.ModelVisual3D model = new System.Windows.Media.Media3D.ModelVisual3D();
            model.Content = model3DRenderer.GetModel3D(value, units);
            return model;
        }
    }
}
