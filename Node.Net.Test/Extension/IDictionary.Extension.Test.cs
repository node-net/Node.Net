using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Test.Extension
{
    [TestFixture]
    internal class IDictionaryExtensionTest
    {
        [Test]
        public void GetRotations()
        {
            var rotations = new Dictionary<string, string>().GetRotations();
            Assert.AreEqual(0, rotations.Z, "rotations.Z");

            rotations = new Dictionary<string, string>
            {
                { "Orientation","-180 deg" }
            }.GetRotations();
            Assert.AreEqual(-180.0, rotations.Z, "rotations.Z");
        }

        [Test]
        public void Find()
        {
            var assembly = typeof(IDictionaryExtensionTest).Assembly;
            var data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            Assert.NotNull(data, nameof(data));
            //data.DeepUpdateParents();
            var a = data.Find<IDictionary>("objectA");
            Assert.NotNull(a, nameof(a));
        }

        private bool Filter(object v)
        {
            return true;
        }

        [Test]
        public void Collect()
        {
            var assembly = typeof(IDictionaryExtensionTest).Assembly;
            var data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            Assert.NotNull(data, nameof(data));
            data.Collect(typeof(IDictionary));
            data.Collect<IDictionary>(Filter);
            data.Collect<IDictionary>("");
            data.Collect<IDictionary>("test");
            data.CollectKeys();
        }

        [Test]
        public void DeepUpdateParents()
        {
            var assembly = typeof(IDictionaryExtensionTest).Assembly;
            var data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            Assert.NotNull(data, nameof(data));
            data.DeepUpdateParents();
            IDictionaryExtension.DeepUpdateParents(null);
            data.ComputeHashCode();
        }

        [Test]
        public void GetLengthMeters()
        {
            var data = new Dictionary<string, object>
            {
                {"X" ,"2 m" }
            };
            Assert.AreEqual(2.0, data.GetLengthMeters("X"));
        }

        [Test]
        public void GetLocalToParent()
        {
            var data = new Dictionary<string, object>
            {
                {"X", "10 m" },
                {"Y","1 m" }
            };

            var localToParent = data.GetLocalToParent();
            var origin = localToParent.Transform(new System.Windows.Media.Media3D.Point3D(0, 0, 0));
            Assert.AreEqual(10, origin.X);
            Assert.AreEqual(1, origin.Y);

            var mstring = localToParent.ToString();
            Assert.AreEqual(32, mstring.Length);

            var m = Matrix3D.Parse(mstring);
            var origin2 = m.Transform(new System.Windows.Media.Media3D.Point3D(0, 0, 0));
            Assert.AreEqual(10, origin2.X);
            Assert.AreEqual(1, origin2.Y);

            var i = Matrix3D.Parse("Identity");
            Assert.True(i.IsIdentity);
        }

        [Test]
        public void Get()
        {
            var data = new Dictionary<string, object>
            {
                {"Name","test" }
            };

            Assert.AreEqual("test", data.Get<string>("Name"), "Name");
            Assert.AreEqual("test", data.Get<string>("Name,Description"), "Name,Description");
            Assert.AreEqual("test", data.Get<string>("Description,Name"), "Description,Name");

            var data2 = new Dictionary<string, object>
            {
                {"Description","example" }
            };

            Assert.AreEqual("example", data2.Get<string>("Description"), "Description");
            Assert.AreEqual("example", data2.Get<string>("Name,Description"), "Name,Description");
            Assert.AreEqual("example", data2.Get<string>("Description,Name"), "Description,Name");
        }
    }
}