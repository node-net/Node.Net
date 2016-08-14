using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class ConcreteRotations : IRotations
    {
        public Vector3D RotationsXYZ { get; set; }
    }
    class IRotationsFactory : IFactory
    {
        public object Create(Type type, object value)
        {
            return new ConcreteRotations
            {
                RotationsXYZ = new Vector3D(GetRotationX(value), GetRotationY(value), GetRotationZ(value))
            };
        }

        public static double GetRotationX(object value)
        {
            var stringValue = GetDictionaryValue(value, "RotationX");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Spin");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Roll");
            return Factory.Default.Create<ILength>(stringValue).Length;
        }
        public static double GetRotationY(object value)
        {
            var stringValue = GetDictionaryValue(value, "RotationY");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Tilt");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Pitch");
            return Factory.Default.Create<ILength>(stringValue).Length;
        }
        public static double GetRotationZ(object value)
        {
            var stringValue = GetDictionaryValue(value, "RotationZ");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Orientation");
            if (stringValue.Length == 0) stringValue = GetDictionaryValue(value, "Yaw");
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
        public static IRotationsFactory Default { get; } = new IRotationsFactory();
    }
}
