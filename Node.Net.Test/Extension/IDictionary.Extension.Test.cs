using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Test.Extension
{
    [TestFixture]
    internal class IDictionaryExtensionTest
    {
        [Test]
        public void GetRotations()
        {
            Vector3D rotations = new Dictionary<string, string>().GetRotations();
            Assert.That(rotations.Z,Is.EqualTo(0), "rotations.Z");

            rotations = new Dictionary<string, string>
            {
                { "Orientation","-180 deg" }
            }.GetRotations();
            Assert.That(rotations.Z, Is.EqualTo(-180.0), "rotations.Z");
        }

        [Test]
        public void Find()
        {
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            IDictionary a = data.Find<IDictionary>("objectA");
        }

        private bool Filter(object v)
        {
            return true;
        }

        [Test]
        public void Collect()
        {
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            data.Collect(typeof(IDictionary));
            data.Collect<IDictionary>(Filter);
            data.Collect<IDictionary>("");
            data.Collect<IDictionary>("test");
            data.CollectKeys();
        }

        [Test]
        public void DeepUpdateParents()
        {
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            data.DeepUpdateParents();
            IDictionaryExtension.DeepUpdateParents(null);
            data.ComputeHashCode();
        }

        [Test]
        public void GetLengthMeters()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"X" ,"2 m" }
            };
            Assert.That(data.GetLengthMeters("X"),Is.EqualTo(2.0));
        }

        [Test]
        public void GetLocalToParent()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"X", "10 m" },
                {"Y","1 m" }
            };

            Matrix3D localToParent = data.GetLocalToParent();
            Assert.That(localToParent, Is.Not.SameAs(data.GetLocalToParent()));
            Matrix3D localToWorld = data.GetLocalToWorld();
            Assert.That(localToWorld, Is.Not.SameAs(data.GetLocalToWorld()));
            Point3D origin = localToParent.Transform(new System.Windows.Media.Media3D.Point3D(0, 0, 0));
            Assert.That(origin.X,Is.EqualTo(10));
            Assert.That(origin.Y,Is.EqualTo(1));

            string mstring = localToParent.ToString();
            Assert.That(mstring.Length,Is.EqualTo(32));

            Matrix3D m = Matrix3D.Parse(mstring);
            Point3D origin2 = m.Transform(new System.Windows.Media.Media3D.Point3D(0, 0, 0));
            Assert.That(origin2.X, Is.EqualTo(10));
            Assert.That(origin2.Y, Is.EqualTo(1));

            Matrix3D i = Matrix3D.Parse("Identity");
            Assert.That(i.IsIdentity,Is.True);

            Assert.That(Round(localToParent.GetOrientation(), 3), Is.EqualTo(0), "orientation");
            Assert.That(Round(localToParent.GetTilt(), 3), Is.EqualTo(0), "Tilt");
            Assert.That(Round(localToParent.GetSpin(), 3), Is.EqualTo(0), "Spin");

            data["Orientation"] = "135.0 deg";
            data["Tilt"] = "55 deg";
            localToParent = data.GetLocalToParent();
            Assert.That(Round(localToParent.GetOrientation(), 3), Is.EqualTo(135.0), "orientation");
            Assert.That(Round(localToParent.GetTilt(), 3), Is.EqualTo(55.0), "tilt");

            localToWorld = data.GetLocalToWorld();
            Assert.That(Round(localToWorld.GetOrientation(), 3), Is.EqualTo(135.0), "orientation");
            Assert.That(Round(localToWorld.GetTilt(), 3), Is.EqualTo(55.0), "tilt");
        }

        [Test]
        public void Get()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"Name","test" }
            };

            Assert.That(data.Get<string>("Name"),Is.EqualTo("test"), "Name");
            Assert.That(data.Get<string>("Name,Description"), Is.EqualTo("test"), "Name,Description");
            Assert.That(data.Get<string>("Description,Name"), Is.EqualTo("test"), "Description,Name");

            Dictionary<string, object> data2 = new Dictionary<string, object>
            {
                {"Description","example" }
            };

            Assert.That(data2.Get<string>("Description"), Is.EqualTo("example"), "Description");
            Assert.That(data2.Get<string>("Name,Description"), Is.EqualTo("example"), "Name,Description");
            Assert.That(data2.Get<string>("Description,Name"), Is.EqualTo("example"), "Description,Name");
        }

        [Test]
        public void SettingRotationsXYZ()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.SetRotations(new Vector3D(0, 0, 45));

            Matrix3D localToWorld = data.GetLocalToWorld();
            Point3D point = localToWorld.Transform(new Point3D(1, 0, 0));
            Assert.That(Round(point.X, 2), Is.EqualTo(0.71), "point.X");
            Assert.That(Round(point.Y, 2), Is.EqualTo(0.71), "point.Y");
        }

        [Test]
        public void SettingRotationsOTS()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.SetRotationsOTS(new Vector3D(45, 0, 0));

            Matrix3D localToWorld = data.GetLocalToWorld();
            Point3D point = localToWorld.Transform(new Point3D(1, 0, 0));
            Assert.That(Round(point.X, 2), Is.EqualTo(0.71), "point.X");
            Assert.That(Round(point.Y, 2), Is.EqualTo(0.71), "point.Y");

            data.SetRotationsOTS(new Vector3D(45, 30, 0));
            localToWorld = data.GetLocalToWorld();
            point = localToWorld.Transform(new Point3D(1, 0, 0));
            Assert.That(Round(point.X, 2), Is.EqualTo(0.71), "point.X");
            Assert.That(Round(point.Y, 2), Is.EqualTo(0.71), "point.Y");
        }

        [Test]
        public void Rotate()    // See Matrix3D.Test.Rotate
        {
            Dictionary<string, object> d1 = new Dictionary<string, object>
            {
                {"Orientation" , "15 deg" }
            };
            Matrix3D m1 = d1.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Vector3D xvec1 = m1.Transform(new Vector3D(1, 0, 0));
            Assert.That(Round(xvec1.X, 3), Is.EqualTo(0.966), "xvec1.X");
            Assert.That(Round(xvec1.Y, 3), Is.EqualTo(0.259), "xvec1.Y");
            Assert.That(Round(xvec1.Z, 3), Is.EqualTo(0.000), "xvec1.Z");
            Vector3D rotXYZ1 = m1.GetRotationsXYZ();
            Assert.That(Round(rotXYZ1.X, 3), Is.EqualTo(0.0), "rotXYZ1.X");
            Assert.That(Round(rotXYZ1.Y, 3), Is.EqualTo(0.0), "rotXYZ1.Y");
            Assert.That(Round(rotXYZ1.Z, 3), Is.EqualTo(15.0), "rotXYZ1.Z");

            Dictionary<string, object> d2 = new Dictionary<string, object>
            {
                {"Orientation", "15 deg" },
                {"Tilt","-60 deg" }
            };
            Matrix3D m2 = d2.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, -60, 0));
            Vector3D zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            Assert.That(Round(zvec2.X, 3),Is.EqualTo(-0.224), "zvec2.X");
            Assert.That(Round(zvec2.Y, 3), Is.EqualTo(0.837), "zvec2.Y");
            Assert.That(Round(zvec2.Z, 3), Is.EqualTo(0.500), "zvec2.Z");
            Vector3D yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            Assert.That(Round(yvec2.X, 3),Is.EqualTo(-0.129), "yvec2.X");
            Assert.That(Round(yvec2.Y, 3), Is.EqualTo(0.483), "yvec2.Y");
            Assert.That(Round(yvec2.Z, 3), Is.EqualTo(-0.866),"yvec2.Z");
            Vector3D rotXYZ2 = m2.GetRotationsXYZ();
            Assert.That(Round(rotXYZ2.X, 3), Is.EqualTo(-60), "rotXYZ2.X");
            Assert.That(Round(rotXYZ2.Y, 3), Is.EqualTo(0.0), "rotXYZ2.Y");
            Assert.That(Round(rotXYZ2.Z, 3), Is.EqualTo(15.0), "rotXYZ2.Z");

            Dictionary<string, object> d3 = new Dictionary<string, object>
            {
                {"Orientation", "15.0 deg" },
                {"Tilt","-60.0 deg" },
                {"Spin", "5.0 deg"}
            };
            Matrix3D m3 = d3.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            Vector3D zvec3 = m3.Transform(new Vector3D(0, 0, 1));
            // TODO update Matrix3DFactory CreateFromIDictionary(IDictionary dictionary)
            /*
            Assert.AreEqual(-0.224, Round(zvec3.X, 3), "zvec3.X");
            Assert.AreEqual(0.837, Round(zvec3.Y, 3), "zvec3.Y");
            Assert.AreEqual(0.500, Round(zvec3.Z, 3), "zvec3.Z");
            var yvec3 = m3.Transform(new Vector3D(0, 1, 0));
            Assert.AreEqual(-0.213, Round(yvec3.X, 3), "yvec3.X");
            Assert.AreEqual(0.459, Round(yvec3.Y, 3), "yvec3.Y");
            Assert.AreEqual(-0.863, Round(yvec3.Z, 3), "yvec3.Z");
            */
        }

        [Test]
        public void ConvertRotationsXYZtoOTS()
        {
            Dictionary<string, object> d1 = new Dictionary<string, object>
            {
                {"RotationZ" , "15 deg" }
            };
            Matrix3D m1 = d1.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Vector3D xvec1 = m1.Transform(new Vector3D(1, 0, 0));
            Assert.That(Round(xvec1.X, 3), Is.EqualTo(0.966), "xvec1.X");
            Assert.That(Round(xvec1.Y, 3), Is.EqualTo(0.259), "xvec1.Y");
            Assert.That(Round(xvec1.Z, 3), Is.EqualTo(0.000), "xvec1.Z");

            IDictionary d1c = d1.ConvertRotationsXYZtoOTS() as IDictionary;
            //Assert.AreEqual(d1.Count, d1c.Count);
            Assert.That(d1c.Contains("XDirection"), Is.False,"d1c.Contains(\"XDirection\"");
            Assert.That(Round(d1c.GetAngleDegrees("Orientation"), 3), Is.EqualTo(15.0), "d1c Orientation");

            Dictionary<string, object> d2 = new Dictionary<string, object>
            {
                {"RotationZ" , "15 deg" },
                {"RotationX","-60 deg" }
            };
            Matrix3D m2 = d2.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, -60, 0));
            Vector3D zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            Assert.That(Round(zvec2.X, 3),Is.EqualTo(-0.224), "zvec2.X");
            Assert.That(Round(zvec2.Y, 3), Is.EqualTo(0.837), "zvec2.Y");
            Assert.That(Round(zvec2.Z, 3), Is.EqualTo(0.500), "zvec2.Z");
            Vector3D yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            Assert.That(Round(yvec2.X, 3), Is.EqualTo(-0.129), "yvec2.X");
            Assert.That(Round(yvec2.Y, 3), Is.EqualTo(0.483), "yvec2.Y");
            Assert.That(Round(yvec2.Z, 3), Is.EqualTo(-0.866), "yvec2.Z");

            IDictionary d2c = d2.ConvertRotationsXYZtoOTS() as IDictionary;
            Assert.That(d2.Count, Is.EqualTo(d2c.Count));
            Assert.That(Round(d2c.GetAngleDegrees("Orientation"), 3), Is.EqualTo(15.0), "d2c Orientation");
            //Assert.AreEqual(-60.0, Round(d2c.GetAngleDegrees("Tilt"), 3), "d2c Tilt");
        }

        [Test]
        public void SetRotationsOTS()
        {
            Dictionary<string, object> d1 = new Dictionary<string, object> { };
            d1.SetRotationsOTS(new Vector3D(45.0, 124.0, 0.0));
            Matrix3D m1 = d1.GetLocalToWorld();
            Assert.That(Round(m1.GetOrientation(), 3), Is.EqualTo(45.0), "Orientation");
            Assert.That(Round(m1.GetTilt(), 3), Is.EqualTo(124.0), "Tilt");

            Vector3D ots = d1.GetRotationsOTS();
            Assert.That(Round(ots.X, 3), Is.EqualTo(45.0), "ots.X");
            Assert.That(Round(ots.Y, 3), Is.EqualTo(124.0), "ots.Y");
            Assert.That(Round(ots.Z, 3), Is.EqualTo(0.0), "ots.Z");

            d1.SetRotationsOTS(new Vector3D(60.0, 15, 0));
            m1 = d1.GetLocalToWorld();
            Assert.That(Round(m1.GetOrientation(), 3), Is.EqualTo(60.0), "Orientation");
            Assert.That(Round(m1.GetTilt(), 3), Is.EqualTo(15.0), "Tilt");
        }
    }
}