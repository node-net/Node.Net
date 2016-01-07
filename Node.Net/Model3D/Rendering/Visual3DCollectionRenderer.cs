using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D.Rendering
{
    class Visual3DCollectionRenderer : Base
    {
        public Visual3DCollectionRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer) { }
        private Visual3DRenderer visual3DRenderer = null;
        public Visual3DRenderer Visual3DRenderer
        {
            get {
                if(object.ReferenceEquals(null,visual3DRenderer))
                {
                    visual3DRenderer = new Visual3DRenderer(Renderer);
                }
                return visual3DRenderer; 
            }
            set { visual3DRenderer = value; }
        }

        public Visual3D[] GetVisual3DCollection(object value,
        Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter) => GetVisual3DCollection(value as System.Collections.IDictionary, units);
        protected virtual Visual3D[] GetVisual3DCollection(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            List<Visual3D> results = new List<Visual3D>();
            foreach(object key in value)
            {
                Visual3D visual3D = visual3DRenderer.GetVisual3D(value[key], units);
                if(!object.ReferenceEquals(null,visual3D))
                {
                    results.Add(visual3D);
                }
            }
            return results.ToArray();
        }
    }
}
