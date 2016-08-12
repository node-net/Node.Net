using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class ConcreteTranslation : ITranslation
    {
        public Vector3D Translation { get; set; }
    }
    class ITranslationFactory : IFactory
    {
        public object Create(Type type,object value)
        {
            return new ConcreteTranslation
            {
                Translation = new Vector3D(GetX(value), GetY(value), GetZ(value))
            };
        }

        private string GetDictionaryValue(object value,string name)
        {
            var dictionary = value as IDictionary;
            if(dictionary != null && dictionary.Contains(name))
            {
                return dictionary[name].ToString();
            }
            return string.Empty;
        }
        public double GetX(object value)
        {
            return Factory.Default.Create<ILength>(GetDictionaryValue(value, "X")).Length;
        }
        public double GetY(object value)
        {
            return Factory.Default.Create<ILength>(GetDictionaryValue(value, "Y")).Length;
        }
        public double GetZ(object value)
        {
            return Factory.Default.Create<ILength>(GetDictionaryValue(value, "Z")).Length;
        }

        public static ITranslationFactory Default { get; } = new ITranslationFactory();
    }
}
