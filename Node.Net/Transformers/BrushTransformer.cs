using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Node.Net.Transformers
{
    public class BrushTransformer : TypeTransformer
    {
        public override object Transform(object item)
        {
            return null;
        }

        public Brush Transform(string name)
        {
            if (NamedBrushes.ContainsKey(name)) return NamedBrushes[name];
            var words = name.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var color_transformer = new ColorTransformer();
            return new SolidColorBrush((Color)color_transformer.Transform(name));
        }

        public static Brush Transform(IDictionary dictionary)
        {
            return null;
        }

        private Dictionary<string, Brush> _namedBrushes = null;
        public Dictionary<string, Brush> NamedBrushes
        {
            get
            {
                if (object.ReferenceEquals(null, _namedBrushes))
                {
                    _namedBrushes = new Dictionary<string, Brush>();
                    foreach (PropertyInfo property in typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static))
                    {
                        if (property.PropertyType == typeof(Brush))
                        {
                            _namedBrushes.Add(property.Name, (Brush)property.GetValue(null, null));
                        }
                    }

                }
                return _namedBrushes;
            }
        }
    }
}
