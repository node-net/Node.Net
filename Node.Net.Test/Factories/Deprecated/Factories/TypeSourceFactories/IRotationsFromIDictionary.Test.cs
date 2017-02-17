using NUnit.Framework;
using System.Collections.Generic;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class IRotationsFromIDictionaryTest
    {
        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(10, 20, 30)]
        public void IRotationsFromIDictionary_Usage(double rotationX, double rotationY, double rotationZ)
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.IRotationsFromIDictionary();
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["RotationX"] = $"{rotationX} deg";
            dictionary["RotationY"] = $"{rotationY} deg";
            dictionary["RotationZ"] = $"{rotationZ} deg";

            var rotationXYZ = factory.Create<IRotations>(dictionary).RotationsXYZ;
            Assert.AreEqual(rotationX, rotationXYZ.X);
            Assert.AreEqual(rotationY, rotationXYZ.Y);
            Assert.AreEqual(rotationZ, rotationXYZ.Z);

            dictionary = new Dictionary<string, dynamic>();
            dictionary["Spin"] = $"{rotationX} deg";
            dictionary["Tilt"] = $"{rotationY} deg";
            dictionary["Orientation"] = $"{rotationZ} deg";
            rotationXYZ = factory.Create<IRotations>(dictionary).RotationsXYZ;
            Assert.AreEqual(rotationX, rotationXYZ.X);
            Assert.AreEqual(rotationY, rotationXYZ.Y);
            Assert.AreEqual(rotationZ, rotationXYZ.Z);

            dictionary = new Dictionary<string, dynamic>();
            dictionary["Roll"] = $"{rotationX} degrees";
            dictionary["Pitch"] = $"{rotationY} degrees";
            dictionary["Yaw"] = $"{rotationZ} degrees";
            rotationXYZ = factory.Create<IRotations>(dictionary).RotationsXYZ;
            Assert.AreEqual(rotationX, rotationXYZ.X);
            Assert.AreEqual(rotationY, rotationXYZ.Y);
            Assert.AreEqual(rotationZ, rotationXYZ.Z);
        }
        [Test]
        public void IRotationsFromIDictionary_Null()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.IRotationsFromIDictionary();
            var rotation = factory.Create<IRotations>(null).RotationsXYZ;
            Assert.AreEqual(0, rotation.X);
            Assert.AreEqual(0, rotation.Y);
            Assert.AreEqual(0, rotation.Z);
        }
    }
}
