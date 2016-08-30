﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Factories.Factories.Helpers
{
    public sealed class IAngleHelper
    {
        class ConcreteAngle : IAngle { public double Angle { get; set; } }

        public static IAngle FromString(string source)
        {
            if (source == null) return new ConcreteAngle { Angle = 0 };
            return new ConcreteAngle { Angle = GetAngle(source) };
        }

        private static double GetAngle(string value)
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
                    unitsConversionFactors.Add("radians", 1.0 / 0.0174533);
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
