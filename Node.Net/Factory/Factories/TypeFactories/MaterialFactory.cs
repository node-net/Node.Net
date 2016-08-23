using System;
using System.Collections;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.TypeFactories
{
    public class MaterialFactory : Generic.TypeFactory<Material>, IFactory
    {
        public object Create(Type type,object value)
        {
            if (value == null) return null;
            if (typeof(string) == value.GetType()) return Create(value.ToString());
            if (typeof(ImageSource).IsAssignableFrom(value.GetType())) return Create(value as ImageSource);
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

        private Material Create(ImageSource imageSource)
        {
            return GetImageMaterial(imageSource);
        }

        public static Material GetImageMaterial(ImageSource imageSource, Brush specularBrush = null, double specularPower = 10)
        {
            var material = new MaterialGroup();
            var diffuse = new DiffuseMaterial
            {
                Brush = new ImageBrush
                {
                    ImageSource = imageSource,
                    TileMode = TileMode.Tile
                }
            };
            material.Children.Add(diffuse);
            if (!ReferenceEquals(null, specularBrush))
            {
                var specular = new SpecularMaterial
                {
                    Brush = specularBrush,
                    SpecularPower = specularPower
                };
                material.Children.Add(specular);
            }
            return material;
        }
    }
}
