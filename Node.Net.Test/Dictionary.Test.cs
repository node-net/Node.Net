using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Node.Net;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal class DictionaryTest
    {
        [Test]
        public void GetLocalToParent_EmptyDictionary_ReturnsIdentity()
        {
            Dictionary<string, object> element = new Dictionary<string, object>();
            Matrix3D matrix = element.GetLocalToParent();
            
            // Empty dictionary should return identity matrix
            // Check individual components to diagnose issues
            Assert.That(matrix.M11, Is.EqualTo(1.0), "M11 should be 1.0");
            Assert.That(matrix.M22, Is.EqualTo(1.0), "M22 should be 1.0");
            Assert.That(matrix.M33, Is.EqualTo(1.0), "M33 should be 1.0");
            Assert.That(matrix.M44, Is.EqualTo(1.0), "M44 should be 1.0");
            Assert.That(matrix.OffsetX, Is.EqualTo(0.0), "OffsetX should be 0.0");
            Assert.That(matrix.OffsetY, Is.EqualTo(0.0), "OffsetY should be 0.0");
            Assert.That(matrix.OffsetZ, Is.EqualTo(0.0), "OffsetZ should be 0.0");
            Assert.That(matrix.IsIdentity, Is.True, "GetLocalToParent should return identity for empty dictionary");
            Assert.That(Round(matrix.GetOrientation(), 4), Is.EqualTo(0.0), "orientation should be zero");
            Assert.That(Round(matrix.GetTilt(), 3), Is.EqualTo(0.0), "tilt should be zero");
            Assert.That(Round(matrix.GetSpin(), 3), Is.EqualTo(0.0), "spin should be zero");
        }

        [Test]
        public void RotateOTS()
        {
            Dictionary<string, object> element = new Dictionary<string, object>();
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4),Is.EqualTo(0.0), "orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3),Is.EqualTo(0.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3),Is.EqualTo(0.0), "spin");

            element.SetRotationsOTS(new Vector3D(55, 15, 0));
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4), Is.EqualTo(55.0),"orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3), Is.EqualTo(15.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3), Is.EqualTo(0.0), "spin");

            Dictionary<string, object> group = new Dictionary<string, object>();
            group.Add("child", element);
            element.SetParent(group);

            element.SetRotationsOTS(new Vector3D(85.0, 32.0, 0));
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4), Is.EqualTo(85.0), "orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3), Is.EqualTo(32.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3), Is.EqualTo(0.0), "spin");

            element["XDirection"] = "1,0,0";
            element["YDirection"] = "0,0.573462,0.819232";
            element.SetRotationsOTS(new Vector3D(15.0, 23, 0));
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4), Is.EqualTo(15.0), "orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3), Is.EqualTo(23.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3), Is.EqualTo(0.0), "spin");
        }

        [Test]
        public void GetUniqueKey_WhenKeyDoesNotExist_ReturnsBaseName()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string result = dictionary.GetUniqueKey("test");
            Assert.That(result, Is.EqualTo("test"));
        }

        [Test]
        public void GetUniqueKey_WhenKeyExists_ReturnsBaseNameWithNumber()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test", "value1");
            string result = dictionary.GetUniqueKey("test");
            Assert.That(result, Is.EqualTo("test1"));
        }

        [Test]
        public void GetUniqueKey_WhenMultipleKeysExist_ReturnsNextAvailableNumber()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test", "value1");
            dictionary.Add("test1", "value2");
            dictionary.Add("test2", "value3");
            string result = dictionary.GetUniqueKey("test");
            Assert.That(result, Is.EqualTo("test3"));
        }

        [Test]
        public void GetUniqueKey_WhenBaseNameExistsButNumberedKeysSkip_ReturnsFirstAvailable()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test", "value1");
            dictionary.Add("test1", "value2");
            dictionary.Add("test3", "value4"); // test2 is missing
            string result = dictionary.GetUniqueKey("test");
            Assert.That(result, Is.EqualTo("test2")); // Should return first available
        }

        [Test]
        public void GetUniqueKey_WhenBaseNameDoesNotExistButNumberedKeysDo_ReturnsBaseName()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test1", "value1");
            dictionary.Add("test2", "value2");
            string result = dictionary.GetUniqueKey("test");
            Assert.That(result, Is.EqualTo("test")); // Base name doesn't exist, so return it
        }
    }
}
