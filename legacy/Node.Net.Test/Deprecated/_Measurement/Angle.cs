namespace Node.Net.Measurement
{
    public class Angle : System.IComparable,
                         System.ComponentModel.INotifyPropertyChanged
    {
        private static System.Collections.Generic.Dictionary<AngularUnit, double> conversionToRadians = new System.Collections.Generic.Dictionary<AngularUnit, double>();
        private static System.Collections.Generic.Dictionary<AngularUnit, string> abbreviations = new System.Collections.Generic.Dictionary<AngularUnit, string>();
        static Angle()
        {
            conversionToRadians.Add(AngularUnit.Radians, 1);
            conversionToRadians.Add(AngularUnit.Degrees, 0.01745329);

            abbreviations.Add(AngularUnit.Degrees, "deg");
            abbreviations.Add(AngularUnit.Radians, "rad");
        }
        public static double Convert(double value, AngularUnit valueUnits, AngularUnit desiredUnits) => value * conversionToRadians[valueUnits] / conversionToRadians[desiredUnits];

        public static Angle Parse(string value)
        {
            if (value == "") return new Angle();
            double angleValue = System.Convert.ToDouble(value.Split(' ')[0]);
            string unit = value.Split(' ')[1];
            foreach (AngularUnit aunit in abbreviations.Keys)
            {
                if (unit == abbreviations[aunit])
                {
                    return new Angle(angleValue, aunit);
                }
            }
            return new Angle(System.Convert.ToDouble(value), AngularUnit.Degrees);
            throw new System.FormatException("Unable to parse an Angle from string \"" + value + "\"");
        }

        private AngularUnit angularUnit = AngularUnit.Degrees;
        public AngularUnit Units
        {
            get { return angularUnit; }
            set
            {
                if (angularUnit != value)
                {
                    angleValue = Convert(angleValue, angularUnit, value);
                    angularUnit = value;
                    NotifyPropertyChanged("Units");
                }
            }
        }
        private double angleValue = 0;

        public Angle() { }
        public Angle(double value, AngularUnit units) { angleValue = value; angularUnit = units; }
        public double this[AngularUnit targetUnits]
        {
            get
            {
                return Convert(angleValue, angularUnit, targetUnits);
            }
            set
            {
                double newValue = Convert(value, targetUnits, angularUnit);
                if (angleValue != newValue)
                {
                    angleValue = newValue;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        public void Set(string value)
        {
            Angle tmp = Parse(value);
            angleValue = tmp.angleValue;
            angularUnit = tmp.angularUnit;
        }
        public void Round(int decimals)
        {
            angleValue = (double)(System.Math.Round(angleValue, decimals, System.MidpointRounding.AwayFromZero));
        }

        public void Normalize()
        {
            while (this[AngularUnit.Degrees] > 180) this[AngularUnit.Degrees] = this[AngularUnit.Degrees] - 360;
            while (this[AngularUnit.Degrees] < -180) this[AngularUnit.Degrees] = this[AngularUnit.Degrees] + 360;
        }
        public int CompareTo(object instance)
        {
            if (object.ReferenceEquals(null, instance)) return 1;

            Angle thatAngle = instance as Angle;
            if (!object.ReferenceEquals(null, thatAngle))
            {
                return this[AngularUnit.Degrees].CompareTo(thatAngle[AngularUnit.Degrees]);
            }
            return GetType().FullName.CompareTo(instance.GetType().FullName);
        }

        public override int GetHashCode() => this[AngularUnit.Degrees].GetHashCode();

        public override bool Equals(object obj)
        {
            Angle thatAngle = obj as Angle;
            if (!object.ReferenceEquals(null, thatAngle))
            {
                return this[AngularUnit.Degrees].Equals(thatAngle[AngularUnit.Degrees]);
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

        public override string ToString() => angleValue.ToString() + " " + abbreviations[angularUnit];
    }
}
