using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Extension
{
    [TestFixture]
    class IDictionaryExtensionTest
    {
        [Test]
        public void IDictionary_LocalToWorld()
        {
            var factory = new Factory(); // to ensure static intializeer for Factory gets called
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 m";

            var childDictionary = new Dictionary<string, dynamic>();
            childDictionary["X"] = "1 m";

            dictionary["child"] = childDictionary;
            var child = dictionary["child"] as IDictionary;

            ObjectExtension.UpdateParentBindings(dictionary);
            Assert.AreSame(dictionary, ObjectExtension.GetParent(child), "ObjectExtension.GetParent(child)");

            var localToWorld = IDictionaryExtension.GetLocalToWorld(child);
            var worldOrigin = localToWorld.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(11, worldOrigin.X);


        }
    }
}
