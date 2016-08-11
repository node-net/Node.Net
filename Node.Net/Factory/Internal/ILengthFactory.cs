using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Factory.Internal
{
    class ConcreteLength : ILength
    {
        public double Length { get; set; }
    }
    class ILengthFactory : IFactory
    {
        public T Create<T>(object value)
        {
            return (T)(object)new ConcreteLength { Length = Create(value) };
        }

        private double Create(object value)
        {
            if (value == null) return 0.0;
            if (value.GetType() == typeof(string)) return Create(value.ToString());
            return Convert.ToDouble(value);
        }

        private double Create(string value)
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

            var double_value = Convert.ToDouble(numberSB.ToString());
            return double_value * GetUnitsConversion(unitsSB.ToString().Trim());
        }

        private static Dictionary<string, double> unitsConversionFactors = new Dictionary<string, double>();
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
        private static Dictionary<string, string> unitsAliases = new Dictionary<string, string>();
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
        private double GetUnitsConversion(string units)
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
