using System;
using System.Collections;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class MaterialFactory : IFactory
    {
        public object Create(Type type,object value)
        {
            if (value == null) return null;
            if (typeof(string) == value.GetType()) return Create(value.ToString());
            return Create(value as IDictionary);
        }

        private IFactory Factory
        {
            get { return Node.Net.Factory.Factory.Default; }
        }
        private Material Create(IDictionary dictionary)
        {
            if (dictionary == null) return null;
            var materialName = GetDictionaryValue(dictionary, "Material");
            if(materialName.Length > 0)
            {
                return Factory.Create<Material>(materialName);
            }
            return null;
        }

        private static string GetDictionaryValue(object value, string name)
        {
            var dictionary = value as IDictionary;
            if (dictionary != null && dictionary.Contains(name))
            {
                return dictionary[name].ToString();
            }
            return string.Empty;
        }

        private Material Create(string name)
        {
            var color = Factory.Create<Color>(name);
            return new DiffuseMaterial { Brush = new SolidColorBrush(color) };
        }

    }
}
