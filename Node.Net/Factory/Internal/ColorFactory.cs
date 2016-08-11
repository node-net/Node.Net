using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace Node.Net.Factory.Internal
{
    class ColorFactory : IFactory
    {
        public T Create<T>(object value)
        {
            return (T)(object)Create(value);
        }

        public Color Create(object value)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(string))
                {
                    var name = value.ToString();
                    if (NamedColors.ContainsKey(value.ToString())) return NamedColors[value.ToString()];
                    var words = name.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 3)
                    {
                        return Color.FromRgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]));
                    }
                    if (words.Length == 4)
                    {
                        return Color.FromArgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]), Convert.ToByte(words[3]));
                    }
                }
            }
            throw new Exception("unable to create Color from argument");
        }

        private Dictionary<string, Color> _namedColors;
        public Dictionary<string, Color> NamedColors
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
