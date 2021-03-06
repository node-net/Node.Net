﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Internal
{
    internal static class Angle
    {
        public static double GetDegrees(string value)
        {
            StringBuilder? unitsSB = new StringBuilder();
            StringBuilder? numberSB = new StringBuilder();
            foreach (char ch in value)
            {
                if (unitsSB.Length < 1
                    && (Char.IsDigit(ch) || ch == '.' || ch == 'E' || ch == '+' || ch == 'e' || ch == '-'))
                {
                    numberSB.Append(ch);
                }
                else
                {
                    unitsSB.Append(ch);
                }
            }

            if (numberSB.Length == 0)
            {
                return 0.0;
            }

            double double_value = Convert.ToDouble(numberSB.ToString());
            return double_value * GetUnitsConversion(unitsSB.ToString().Trim());
        }

        private static readonly Dictionary<string, double> UnitsConversionFactors = new Dictionary<string, double>
        {
            { "radians", 1.0 / 0.0174533}
        };

        private static readonly Dictionary<string, string> UnitsAliases = new Dictionary<string, string>
        {
            { "rad", "radians" }
        };

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