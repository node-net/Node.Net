using NUnit.Framework;

namespace Node.Net.Measurement
{
    [TestFixture,Category("Node.Net.Measurement.Angle")]
    class Angle_Test
    {
        [TestCase]
        public void Measurement_Angle_Conversions()
        {
            // 1.57079633 radians = 90 degrees
            var angle_degrees = Angle.Convert(1.57079633, AngularUnit.Radians, AngularUnit.Degrees);
            Assert.True(System.Math.Abs(angle_degrees - 90) < 0.0001);
        }

        [TestCase]
        public void Measurement_Angle_Usage()
        {
            var angle = new Angle();
            angle = new Angle(90, AngularUnit.Degrees);
            Assert.True(System.Math.Abs(angle[AngularUnit.Degrees] - 90) < 0.0001);
            Assert.True(System.Math.Abs(angle[AngularUnit.Radians] - 1.57079633) < 0.0001);
            Assert.AreEqual("90 deg", angle.ToString());

            var angle30 = Angle.Parse("30");
            Assert.True(System.Math.Abs(angle30[AngularUnit.Degrees] - 30) < 0.0001);

            var angle45 = Angle.Parse("45 deg");
            angle45.PropertyChanged += angle45_PropertyChanged;
            Assert.True(System.Math.Abs(angle45[AngularUnit.Degrees] - 45) < 0.0001);
            Assert.AreNotEqual(0, angle.CompareTo(angle45));
            Assert.False(angle.Equals(angle45));
            Assert.AreNotEqual(angle.GetHashCode(), angle45.GetHashCode());

            angle45[AngularUnit.Degrees] = 44.9999;
            angle45.Round(0);
            Assert.True(System.Math.Abs(angle45[AngularUnit.Degrees] - 45) < 0.0001);
            Assert.AreEqual(AngularUnit.Degrees, angle45.Units);
            angle45.Units = AngularUnit.Radians;
            Assert.AreEqual(AngularUnit.Radians, angle45.Units);
            Assert.AreNotEqual(0, angle45.CompareTo(null));
            Assert.AreNotEqual(0, angle45.CompareTo(false));
            Assert.False(angle45.Equals(false));

            Assert.Throws<System.FormatException>(() => Angle.Parse("invalid"));
            Assert.Throws<System.FormatException>(() => Angle.Parse("10.5 nogood"));

            angle = new Angle(-400, AngularUnit.Degrees);
            angle.Normalize();
            angle = new Angle(400, AngularUnit.Degrees);
            angle.Normalize();
        }

        [TestCase]
        public void Measurement_Angle_Parse()
        {
            var angle = Angle.Parse("");
            Assert.AreEqual(0, angle[AngularUnit.Degrees]);
        }
        static void angle45_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}
