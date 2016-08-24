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

        private IFactory HelperFactory = null;
        private Internal.TypeFactories.ILengthFactory LengthFactory = new TypeFactories.ILengthFactory();
        private static string GetDictionaryValue(object value,string name)
        {
            var dictionary = value as IDictionary;
            if(dictionary != null && dictionary.Contains(name))
            {
                return dictionary[name].ToString();
            }
            return string.Empty;
        }
        private double GetLength(string name,object value)
        {
            ILength length = null;
            if (HelperFactory != null)
            {
                length = HelperFactory.Create<ILength>(GetDictionaryValue(value, name));
            }
            if (length == null) length = LengthFactory.Create<ILength>(GetDictionaryValue(value, name));
            return length.Length;
        }
        public double GetX(object value) => GetLength("X", value);
        public double GetY(object value) => GetLength("Y", value);
        public double GetZ(object value) => GetLength("Z", value);

        //public static ITranslationFactory Default { get; } = new ITranslationFactory();
    }
}
