using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class ColorFactory : Dictionary<string, Color>, IFactory
    {
        public static ColorFactory Default { get; } = new ColorFactory();
        public object Create(Type targetType, object source)
        {
            if (source != null)
            {
                if (source.GetType() == typeof(string))
                {
                    var color = Create(source.ToString());
                    if (color.HasValue) return color.Value;
                }
            }

            return null;
        }

        public Color? Create(string name)
        {
            if (ContainsKey(name)) return this[name];
            foreach (PropertyInfo property in typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (property.Name == name)
                {
                    var color = (Color)property.GetValue(null, null);
                    this[name] = color;
                    return color;
                }
            }
            var words = name.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 3)
            {
                return Color.FromRgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]));

            }
            if (words.Length == 4)
            {
                return Color.FromArgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]), Convert.ToByte(words[3]));
            }
            return null;
        }
    }
}
