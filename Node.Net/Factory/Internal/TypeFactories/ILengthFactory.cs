using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Factory.Internal.TypeFactories
{
    class ConcreteLength : ILength
    {
        public double Length { get; set; }
    }
    class ILengthFactory : Factories.Generic.TypeFactory<ILength>, ITypeFactory<ILength>, IFactory
    {
        public object Create(Type type, object value)
        {
            return Create(value);
            //return new ConcreteLength { Length = Create(value) };
        }

        public ILength Create(object source) { return new ConcreteLength { Length = GetDouble(source) }; }
        private static double GetDouble(object value)
        {
            if (value == null) return 0.0;
            if (value.GetType() == typeof(string)) return GetDouble(value.ToString());
            return Convert.ToDouble(value);
        }

        private static double GetDouble(string value)
        {
            var unitsSB = new StringBuilder();
            var numberSB = new StringBuilder();
            foreach (char ch in value)
            {
                if (unitsSB.Length < 1 &&
                    (Char.IsDigit(ch) || ch == '.' || ch == 'E' || ch == '+' || ch == 'e' || ch == '-'))
                {
                    numberSB.Append(ch);
                }
                else
                {
                    unitsSB.Append(ch);
                }
            }

            if (numberSB.Length == 0) return 0.0;
            var double_value = Convert.ToDouble(numberSB.ToString());
            return double_value * GetUnitsConversion(unitsSB.ToString().Trim());
        }

        private static readonly Dictionary<string, double> unitsConversionFactors = new Dictionary<string, double>();
        private static Dictionary<string, double> UnitsConversionFactors
        {
            get
            {
                if (unitsConversionFactors.Count == 0)
                {
                    unitsConversionFactors.Add("feet", 1.0 / 3.28084);
                    unitsConversionFactors.Add("inch", 1.0 / 39.3701);
                    unitsConversionFactors.Add("centimeter", 1.0 / 100.0);
                    unitsConversionFactors.Add("millimeter", 1.0 / 1000.0);
                }
                return unitsConversionFactors;
            }
        }
        private static readonly Dictionary<string, string> unitsAliases = new Dictionary<string, string>();
        private static Dictionary<string, string> UnitsAliases
        {
            get
            {
                if (unitsAliases.Count == 0)
                {
                    unitsAliases.Add("'", "feet");
                    unitsAliases.Add("ft", "feet");
                    unitsAliases.Add("foot", "feet");
                    unitsAliases.Add("inches", "inch");
                    unitsAliases.Add("\"", "inch");
                    unitsAliases.Add("cm", "centimeter");
                    unitsAliases.Add("mm", "millimeter");
                }
                return unitsAliases;
            }
        }
        private static double GetUnitsConversion(string units)
        {
            if (UnitsAliases.ContainsKey(units))
            {
                return UnitsConversionFactors[UnitsAliases[units]];
            }
            if (UnitsConversionFactors.ContainsKey(units))
            {
                return UnitsConversionFactors[units];
            }
            return 1.0;
        }
    }
}
