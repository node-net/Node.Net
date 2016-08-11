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
        public T Create<T>(object value)
        {
            return (T)(object)new ConcreteTranslation
            {
                Translation = new Vector3D(GetX(value), GetY(value), GetZ(value))
            };
        }

        public double GetX(object value)
        {
            var x = 0.0;
            var dictionary = value as IDictionary;
            if (dictionary != null)
            {
                if (dictionary.Contains("X"))
                {
                    x = Convert.ToDouble(dictionary["X"]);
                }
            }
            return x;
        }
        public double GetY(object value)
        {
            var x = 0.0;
            return x;
        }
        public double GetZ(object value)
        {
            var x = 0.0;
            return x;
        }

        public static ITranslationFactory Default { get; } = new ITranslationFactory();
    }
}
