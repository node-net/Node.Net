using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace Node.Net.Internal
{
    internal sealed class ColorFactory : Dictionary<string, Color>, IFactory
    {
        public static ColorFactory Default { get; } = new ColorFactory();

        public object Create(Type targetType, object source)
        {
            if (source != null && source is string)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                Color? color = Create(source.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                if (color.HasValue)
                {
                    return color.Value;
                }
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public Color? Create(string name)
        {
            if (ContainsKey(name))
            {
                return this[name];
            }

            foreach (PropertyInfo property in typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (property.Name == name)
                {
#pragma warning disable CS8605 // Unboxing a possibly null value.
                    Color color = (Color)property.GetValue(null, null);
#pragma warning restore CS8605 // Unboxing a possibly null value.
                    this[name] = color;
                    return color;
                }
            }
            string[]? words = name.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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