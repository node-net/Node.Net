using NUnit.Framework;

namespace Node.Net.Measurement
{
    [NUnit.Framework.TestFixture]
    class Angle_Test
    {
        [NUnit.Framework.TestCase]
        public void Measurement_Angle_Conversions()
        {
            // 1.57079633 radians = 90 degrees
            double angle_degrees = Angle.Convert(1.57079633, AngularUnit.Radians, AngularUnit.Degrees);
            NUnit.Framework.Assert.True(System.Math.Abs(angle_degrees - 90) < 0.0001);
        }

        [NUnit.Framework.TestCase]
        public void Measurement_Angle_Usage()
        {
            Angle angle = new Angle();
            angle = new Angle(90, AngularUnit.Degrees);
            NUnit.Framework.Assert.True(System.Math.Abs(angle[AngularUnit.Degrees] - 90) < 0.0001);
            NUnit.Framework.Assert.True(System.Math.Abs(angle[AngularUnit.Radians] - 1.57079633) < 0.0001);
            NUnit.Framework.Assert.AreEqual("90 deg", angle.ToString());

            Angle angle45 = Angle.Parse("45 deg");
            angle45.PropertyChanged += angle45_PropertyChanged;
            NUnit.Framework.Assert.True(System.Math.Abs(angle45[AngularUnit.Degrees] - 45) < 0.0001);
            NUnit.Framework.Assert.AreNotEqual(0, angle.CompareTo(angle45));
            NUnit.Framework.Assert.False(angle.Equals(angle45));
            NUnit.Framework.Assert.AreNotEqual(angle.GetHashCode(), angle45.GetHashCode());

            angle45[AngularUnit.Degrees] = 44.9999;
            angle45.Round(0);
            NUnit.Framework.Assert.True(System.Math.Abs(angle45[AngularUnit.Degrees] - 45) < 0.0001);
            NUnit.Framework.Assert.AreEqual(AngularUnit.Degrees, angle45.Units);
            angle45.Units = AngularUnit.Radians;
            NUnit.Framework.Assert.AreEqual(AngularUnit.Radians, angle45.Units);
            NUnit.Framework.Assert.AreNotEqual(0, angle45.CompareTo(null));
            NUnit.Framework.Assert.AreNotEqual(0, angle45.CompareTo(false));
            NUnit.Framework.Assert.False(angle45.Equals(false));

            NUnit.Framework.Assert.Throws<System.FormatException>(() => Angle.Parse("invalid"));
            NUnit.Framework.Assert.Throws<System.FormatException>(() => Angle.Parse("10.5 nogood"));

            angle = new Angle(-400, AngularUnit.Degrees);
            angle.Normalize();
            angle = new Angle(400, AngularUnit.Degrees);
            angle.Normalize();
        }

        [NUnit.Framework.TestCase]
        public void Measurement_Angle_Parse()
        {
            Angle angle = Angle.Parse("");
            NUnit.Framework.Assert.AreEqual(0, angle[AngularUnit.Degrees]);
        }
        void angle45_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}
