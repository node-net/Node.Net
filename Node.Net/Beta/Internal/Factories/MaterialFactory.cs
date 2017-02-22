using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    class MaterialFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (!typeof(Material).IsAssignableFrom(target_type)) return null;
            if (source != null)
            {
                if(typeof(Brush).IsAssignableFrom(source.GetType()))
                {
                    return new DiffuseMaterial(source as Brush);
                }

            }
            if (ParentFactory != null)
            {
                var brush = ParentFactory.Create<Brush>(source);
                if (brush != null) return new DiffuseMaterial(brush);
                //return Create(target_type, ParentFactory.Create<Brush>(source));
                /*
                // Brushes creating
                if (source.GetType() == typeof(Color)) return new SolidColorBrush((Color)source);
                if (source.GetType() == typeof(ImageSource)) return CreateFromImageSource(source as ImageSource);

                var color = ParentFactory.Create(typeof(Color), source);
                if (color != null) return Create(target_type, color);*/
            }
            return null;
        }
        public IFactory ParentFactory { get; set; }
    }
}
