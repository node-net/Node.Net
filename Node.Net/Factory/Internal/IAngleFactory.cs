using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factory.Internal
{
    class ConcreteAngle : IAngle
    {
        public double Angle { get; set; }
    }
    class IAngleFactory : IFactory
    {
        public object Create(Type type, object value)
        {
            return new ConcreteAngle { Angle = Create(value) };
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

            if (numberSB.Length == 0) return 0.0;
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
                    unitsConversionFactors.Add("radians", 1.0 / 0.0174533);
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
                    unitsAliases.Add("rad", "radians");
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
