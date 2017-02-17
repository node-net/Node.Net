using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Measurement.Test
{
    [TestFixture]
    class IDictionaryExtensionTest
    {
        [Test]
        public void IDictionaryExtension_GetLengthAsString()
        {
            Assert.AreEqual("0 m", IDictionaryExtension.GetLengthAsString(null, null));
            Assert.AreEqual("0 m", IDictionaryExtension.GetLengthAsString(null, "X"));
            var dictionary = new Dictionary<string, dynamic>();
            Assert.AreEqual("0 m", IDictionaryExtension.GetLengthAsString(dictionary, "X"));
            dictionary["X"] = "10 m";
            Assert.AreEqual("10 m", IDictionaryExtension.GetLengthAsString(dictionary, "X"));
        }
        [Test]
        public void IDictionaryExtension_GetLengthMeters()
        {
            Assert.AreEqual(0, IDictionaryExtension.GetLengthMeters(null, null));
            Assert.AreEqual(0, IDictionaryExtension.GetLengthMeters(null, "X"));
            var dictionary = new Dictionary<string, dynamic>();
            Assert.AreEqual(0, IDictionaryExtension.GetLengthMeters(dictionary, "X"));
            dictionary["X"] = "10 m";
            Assert.AreEqual(10.0, IDictionaryExtension.GetLengthMeters(dictionary, "X"));
        }
        [Test]
        public void IDictionaryExtension_SetLength()
        {
            var dictionary = new Dictionary<string, dynamic>();
            IDictionaryExtension.SetLength(dictionary,"X","10 m");
            Assert.AreEqual("10 m",IDictionaryExtension.GetLengthAsString(dictionary, "X"));
            IDictionaryExtension.SetLength(dictionary, "Y", 20);
            Assert.AreEqual(20, IDictionaryExtension.GetLengthMeters(dictionary, "Y"));
        }

        [Test]
        public void IDictionaryExtension_GetAngleAsString()
        {
            Assert.AreEqual("0 deg", IDictionaryExtension.GetAngleAsString(null, null));
            Assert.AreEqual("0 deg", IDictionaryExtension.GetAngleAsString(null, "Orientaion"));
            var dictionary = new Dictionary<string, dynamic>();
            Assert.AreEqual("0 deg", IDictionaryExtension.GetAngleAsString(dictionary, "Orientation"));
            dictionary["Orientation"] = "10 deg";
            Assert.AreEqual("10 deg", IDictionaryExtension.GetAngleAsString(dictionary, "Orientation"));
        }
        [Test]
        public void IDictionaryExtension_GetAngleDegrees()
        {
            Assert.AreEqual(0, IDictionaryExtension.GetAngleDegrees(null, null));
            Assert.AreEqual(0, IDictionaryExtension.GetAngleDegrees(null, "Orienation"));
            var dictionary = new Dictionary<string, dynamic>();
            Assert.AreEqual(0, IDictionaryExtension.GetAngleDegrees(dictionary, "Orientation"));
            dictionary["Orientation"] = "10 deg";
            Assert.AreEqual(10.0, IDictionaryExtension.GetAngleDegrees(dictionary, "Orientation"));
        }
        [Test]
        public void IDictionaryExtension_SetAngle()
        {
            var dictionary = new Dictionary<string, dynamic>();
            IDictionaryExtension.SetAngle(dictionary, "Orientation", "10 deg");
            Assert.AreEqual("10 deg", IDictionaryExtension.GetAngleAsString(dictionary, "Orientation"));
            IDictionaryExtension.SetAngle(dictionary, "Tilt", 20);
            Assert.AreEqual(20, IDictionaryExtension.GetAngleDegrees(dictionary, "Tilt"));
        }

    }
}
