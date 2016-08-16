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

        public static double GetScaleX(object value)
        {
            var stringValue = GetDictionaryValue(value, "ScaleX");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Length");
            return Factory.Default.Create<ILength>(stringValue).Length;
        }
        public static double GetScaleY(object value)
        {
            var stringValue = GetDictionaryValue(value, "ScaleY");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Width");
            return Factory.Default.Create<ILength>(stringValue).Length;
        }
        public static double GetScaleZ(object value)
        {
            var stringValue = GetDictionaryValue(value, "ScaleZ");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Height");
            return Factory.Default.Create<ILength>(stringValue).Length;
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
