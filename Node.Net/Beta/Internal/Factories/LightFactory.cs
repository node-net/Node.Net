using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    class LightFactory : IFactory
    {
        public object Create(Type target_type,object source)
        {
            if (target_type == null) return null;
            if (!target_type.IsAssignableFrom(target_type)) return null;
            if(source != null)
            {
                if (source.GetType() == typeof(Color)) return new AmbientLight { Color = (Color)source };
                if (typeof(IDictionary).IsAssignableFrom(source.GetType())) return CreateFromIDictionary(source as IDictionary);
            }
            return null;
        }
        public IFactory ParentFactory { get; set; }
        private Light CreateFromIDictionary(IDictionary source)
        {
            if(source.Contains("Direction") && source.Contains("Color"))
            {
                if (ParentFactory != null)
                {
                    return new DirectionalLight
                    {
                        Direction = ParentFactory.Create<Vector3D>(source["Direction"]),
                        Color = ParentFactory.Create<Color>(source["Color"])
                    };
                }
            }
            return null;
        }
    }
}
