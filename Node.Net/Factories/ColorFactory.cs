using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace Node.Net.Factories
{
    public sealed class ColorFactory : Generic.TargetTypeFactory<Color>
    {
        public Color DefaultColor = Colors.Transparent;
        public override Color Create(object source)
        {

            if (source != null)
            {
                if (source.GetType() == typeof(string)) return Create(source.ToString());
            }
            return DefaultColor;
        }

        public Color Create(string name)
        {
            if(NamedColors.ContainsKey(name))
            {
                return NamedColors[name];
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
            return DefaultColor;
        }

        private static Dictionary<string, Color> namedColors;
        public static Dictionary<string,Color> NamedColors
        {
            get
            {
                if(namedColors == null)
                {
                    namedColors = new Dictionary<string, Color>();
                    foreach (PropertyInfo property in typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static))
                    {
                        namedColors.Add(property.Name, (Color)property.GetValue(null, null));
                    }
                }
                return namedColors;
            }
        }
    }
}
