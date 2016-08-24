using System;
using System.Collections;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class ConcreteScale : IScale
    {
        public Vector3D Scale { get; set; }
    }
    class IScaleFactory : IFactory
    {
        public object Create(Type type, object value)
        {
            return new ConcreteScale
            {
                Scale = new Vector3D(GetScaleX(value), GetScaleY(value), GetScaleZ(value))
            };
        }

        private IFactory HelperFactory = null;
        private Internal.TypeFactories.ILengthFactory LengthFactory = new TypeFactories.ILengthFactory();

        private double GetScale(string value)
        {
            ILength length = null;
            if (HelperFactory != null)
            {
                length = HelperFactory.Create<ILength>(value);
            }
            if (length == null) length = LengthFactory.Create<ILength>(value);
            return length.Length;
        }

        public double GetScaleX(object value)
        {
            var stringValue = GetDictionaryValue(value, "ScaleX");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Length");
            return GetScale(stringValue);// HelperFactory.Create<ILength>(stringValue).Length;
        }
        public double GetScaleY(object value)
        {
            var stringValue = GetDictionaryValue(value, "ScaleY");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Width");
            return GetScale(stringValue);// HelperFactory.Create<ILength>(stringValue).Length;
        }
        public double GetScaleZ(object value)
        {
            var stringValue = GetDictionaryValue(value, "ScaleZ");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Height");
            return GetScale(stringValue);// HelperFactory.Create<ILength>(stringValue).Length;
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
        public static IScaleFactory Default { get; } = new IScaleFactory();
    }
}
