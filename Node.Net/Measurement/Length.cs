namespace Node.Net.Measurement
{
    public class Length : System.IComparable, 
                          System.ComponentModel.INotifyPropertyChanged
    {
        public Length() { }
        public Length(double value, LengthUnit units) { lengthValue = value; lengthUnit = units; }

        private double lengthValue = 0;
        private LengthUnit lengthUnit = LengthUnit.Meters;

        public double this[LengthUnit targetUnits]
        {
            get
            {
                return Convert(lengthValue, lengthUnit, targetUnits);
            }
            set
            {
                double newValue = Convert(value, targetUnits, lengthUnit);
                if (lengthValue != newValue)
                {
                    lengthValue = newValue;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        public LengthUnit Units
        {
            get { return lengthUnit; }
            set
            {
                if (lengthUnit != value)
                {
                    lengthValue = Convert(lengthValue, lengthUnit, value);
                    lengthUnit = value;
                    NotifyPropertyChanged("Units");
                }
            }
        }
        public void Round(int decimals)
        {
            lengthValue = (double)(System.Math.Round(lengthValue, decimals, System.MidpointRounding.AwayFromZero));
        }
        public int CompareTo(object instance)
        {
            if (object.ReferenceEquals(null, instance)) return 1;

            Length thatLength = instance as Length;
            if (!object.ReferenceEquals(null, thatLength))
            {
                return this[LengthUnit.Meters].CompareTo(thatLength[LengthUnit.Meters]);
            }
            return GetType().FullName.CompareTo(instance.GetType().FullName);
        }

        public override int GetHashCode()
        {
            return this[LengthUnit.Meters].GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Length thatLength = obj as Length;
            if (!object.ReferenceEquals(null, thatLength))
            {
                return this[LengthUnit.Meters].Equals(thatLength[LengthUnit.Meters]);
            }
            return base.Equals(obj);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }

        public override string ToString()
        {
            return lengthValue.ToString() + " " + abbreviations[lengthUnit];
        }
        private static System.Collections.Generic.Dictionary<LengthUnit, double> conversionToMeters = new System.Collections.Generic.Dictionary<LengthUnit, double>();
        private static System.Collections.Generic.Dictionary<LengthUnit, string> abbreviations = new System.Collections.Generic.Dictionary<LengthUnit, string>();
        static Length()
        {
            conversionToMeters.Add(LengthUnit.Meters, 1.0);
            conversionToMeters.Add(LengthUnit.Feet, 0.3048);
            conversionToMeters.Add(LengthUnit.Inches, 0.0254);
            conversionToMeters.Add(LengthUnit.Yards, 0.9144);
            conversionToMeters.Add(LengthUnit.Millimeters, 0.001);
            conversionToMeters.Add(LengthUnit.Centimeters, 0.01);
            conversionToMeters.Add(LengthUnit.Kilometers, 1000);
            conversionToMeters.Add(LengthUnit.Miles, 1609.34);

            abbreviations.Add(LengthUnit.Meters, "m");
            abbreviations.Add(LengthUnit.Feet, "ft");
            abbreviations.Add(LengthUnit.Inches, "in");
            abbreviations.Add(LengthUnit.Yards, "yd");
            abbreviations.Add(LengthUnit.Millimeters, "mm");
            abbreviations.Add(LengthUnit.Centimeters, "cm");
            abbreviations.Add(LengthUnit.Kilometers, "km");
            abbreviations.Add(LengthUnit.Miles, "mi");
        }
        public static double Convert(double value, LengthUnit valueUnits, LengthUnit desiredUnits)
        {
            return value * conversionToMeters[valueUnits] / conversionToMeters[desiredUnits];
        }
        public static Length Parse(string value)
        {
            if (value.Length == 0) return new Length();
            double lengthValue = System.Convert.ToDouble(value.Split(' ')[0]);
            string unit = value.Split(' ')[1];
            foreach (LengthUnit lunit in abbreviations.Keys)
            {
                if (unit == abbreviations[lunit])
                {
                    return new Length(lengthValue, lunit);
                }
            }
            throw new System.FormatException("Unable to parse a Length from string \"" + value + "\"");
        }
    }
}
