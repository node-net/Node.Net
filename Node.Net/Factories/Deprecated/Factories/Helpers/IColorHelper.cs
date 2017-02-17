using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    public sealed class IColorHelper
    {
        class ConcreteColor : IColor { public Color Color { get; set; } }
        private static ConcreteColor concreteColor = new ConcreteColor();
        public static IColor FromString(string source, IFactory factory)
        {
            if (source == null) return null;
            if (NamedColors.ContainsKey(source))
            {
                concreteColor.Color = NamedColors[source];
                return concreteColor;
            }
            var words = source.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 3)
            {
                concreteColor.Color = Color.FromRgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]));
                return concreteColor;
            }
            if (words.Length == 4)
            {
                concreteColor.Color = Color.FromArgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]), Convert.ToByte(words[3]));
                return concreteColor;
            }
            return null;
        }

        private static Dictionary<string, Color> _namedColors;
        public static Dictionary<string, Color> NamedColors
        {
            get
            {
                if (ReferenceEquals(null, _namedColors))
                {
                    _namedColors = new Dictionary<string, Color>();
                    foreach (PropertyInfo property in typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static))
                    {
                        if (property.PropertyType == typeof(Color))
                        {
                            _namedColors.Add(property.Name, (Color)property.GetValue(null, null));
                        }
                    }

                }
                return _namedColors;
            }
        }
    }
}
