﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace Node.Net.Deprecated.Transformers
{
    public class ColorTransformer : TypeTransformer
    {
        public override object Transform(object item)
        {
            return null;
        }

        public Color Transform(string name)
        {
            if (NamedColors.ContainsKey(name)) return NamedColors[name];
            var words = name.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 3)
            {
                return Color.FromRgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]));
            }
            if (words.Length == 4)
            {
                return Color.FromArgb(Convert.ToByte(words[0]), Convert.ToByte(words[1]), Convert.ToByte(words[2]), Convert.ToByte(words[3]));
            }
            return Colors.Black;
        }

        public static Color Transform(IDictionary dictionary)
        {
            return Colors.Black;
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