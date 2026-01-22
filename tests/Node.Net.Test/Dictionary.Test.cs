using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Math;
using Node.Net;

namespace Node.Net.Test
{
    internal class DictionaryTest
    {
        [Test]
        public async Task GetLocalToParent_EmptyDictionary_ReturnsIdentity()
        {
            Dictionary<string, object> element = new Dictionary<string, object>();
            Matrix3D matrix = element.GetLocalToParent();
            
            // Empty dictionary should return identity matrix
            // Check individual components to diagnose issues
            await Assert.That(matrix.M11).IsEqualTo(1.0);
            await Assert.That(matrix.M22).IsEqualTo(1.0);
            await Assert.That(matrix.M33).IsEqualTo(1.0);
            await Assert.That(matrix.M44).IsEqualTo(1.0);
            await Assert.That(matrix.OffsetX).IsEqualTo(0.0);
            await Assert.That(matrix.OffsetY).IsEqualTo(0.0);
            await Assert.That(matrix.OffsetZ).IsEqualTo(0.0);
            await Assert.That(matrix.IsIdentity).IsTrue();
            await Assert.That(Round(matrix.GetOrientation(), 4)).IsEqualTo(0.0);
            await Assert.That(Round(matrix.GetTilt(), 3)).IsEqualTo(0.0);
            await Assert.That(Round(matrix.GetSpin(), 3)).IsEqualTo(0.0);
        }

        [Test]
        public async Task RotateOTS()
        {
            Dictionary<string, object> element = new Dictionary<string, object>();
            await Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4)).IsEqualTo(0.0);
            await Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3)).IsEqualTo(0.0);
            await Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3)).IsEqualTo(0.0);

            element.SetRotationsOTS(new Vector3D(55, 15, 0));
            await Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4)).IsEqualTo(55.0);
            await Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3)).IsEqualTo(15.0);
            await Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3)).IsEqualTo(0.0);

            Dictionary<string, object> group = new Dictionary<string, object>();
            group.Add("child", element);
            element.SetParent(group);

            element.SetRotationsOTS(new Vector3D(85.0, 32.0, 0));
            await Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4)).IsEqualTo(85.0);
            await Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3)).IsEqualTo(32.0);
            await Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3)).IsEqualTo(0.0);

            element["XDirection"] = "1,0,0";
            element["YDirection"] = "0,0.573462,0.819232";
            element.SetRotationsOTS(new Vector3D(15.0, 23, 0));
            await Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4)).IsEqualTo(15.0);
            await Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3)).IsEqualTo(23.0);
            await Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3)).IsEqualTo(0.0);
        }

        [Test]
        public async Task GetUniqueKey_WhenKeyDoesNotExist_ReturnsBaseName()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string result = dictionary.GetUniqueKey("test");
            await Assert.That(result).IsEqualTo("test");
        }

        [Test]
        public async Task GetUniqueKey_WhenKeyExists_ReturnsBaseNameWithNumber()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test", "value1");
            string result = dictionary.GetUniqueKey("test");
            await Assert.That(result).IsEqualTo("test1");
        }

        [Test]
        public async Task GetUniqueKey_WhenMultipleKeysExist_ReturnsNextAvailableNumber()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test", "value1");
            dictionary.Add("test1", "value2");
            dictionary.Add("test2", "value3");
            string result = dictionary.GetUniqueKey("test");
            await Assert.That(result).IsEqualTo("test3");
        }

        [Test]
        public async Task GetUniqueKey_WhenBaseNameExistsButNumberedKeysSkip_ReturnsFirstAvailable()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test", "value1");
            dictionary.Add("test1", "value2");
            dictionary.Add("test3", "value4"); // test2 is missing
            string result = dictionary.GetUniqueKey("test");
            await Assert.That(result).IsEqualTo("test2"); // Should return first available
        }

        [Test]
        public async Task GetUniqueKey_WhenBaseNameDoesNotExistButNumberedKeysDo_ReturnsBaseName()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("test1", "value1");
            dictionary.Add("test2", "value2");
            string result = dictionary.GetUniqueKey("test");
            await Assert.That(result).IsEqualTo("test"); // Base name doesn't exist, so return it
        }
    }
}
