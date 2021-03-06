﻿using NUnit.Framework;
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
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            Assert.NotNull(data, nameof(data));
            //data.DeepUpdateParents();
            IDictionary a = data.Find<IDictionary>("objectA");
            Assert.NotNull(a, nameof(a));
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
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            Assert.NotNull(data, nameof(data));
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
            Assert.AreEqual(2.0, data.GetLengthMeters("X"));
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
            Assert.AreNotSame(localToParent, data.GetLocalToParent());
            Matrix3D localToWorld = data.GetLocalToWorld();
            Assert.AreNotSame(localToWorld, data.GetLocalToWorld());
            Point3D origin = localToParent.Transform(new System.Windows.Media.Media3D.Point3D(0, 0, 0));
            Assert.AreEqual(10, origin.X);
            Assert.AreEqual(1, origin.Y);

            string mstring = localToParent.ToString();
            Assert.AreEqual(32, mstring.Length);

            Matrix3D m = Matrix3D.Parse(mstring);
            Point3D origin2 = m.Transform(new System.Windows.Media.Media3D.Point3D(0, 0, 0));
            Assert.AreEqual(10, origin2.X);
            Assert.AreEqual(1, origin2.Y);

            Matrix3D i = Matrix3D.Parse("Identity");
            Assert.True(i.IsIdentity);

            Assert.AreEqual(0, Round(localToParent.GetOrientation(), 3), "orientation");
            Assert.AreEqual(0, Round(localToParent.GetTilt(), 3), "Tilt");
            Assert.AreEqual(0, Round(localToParent.GetSpin(), 3), "Spin");

            data["Orientation"] = "135.0 deg";
            data["Tilt"] = "55 deg";
            localToParent = data.GetLocalToParent();
            Assert.AreEqual(135.0, Round(localToParent.GetOrientation(), 3), "orientation");
            Assert.AreEqual(55.0, Round(localToParent.GetTilt(), 3), "tilt");

            localToWorld = data.GetLocalToWorld();
            Assert.AreEqual(135.0, Round(localToWorld.GetOrientation(), 3), "orientation");
            Assert.AreEqual(55.0, Round(localToWorld.GetTilt(), 3), "tilt");
        }

        [Test]
        public void Get()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"Name","test" }
            };

            Assert.AreEqual("test", data.Get<string>("Name"), "Name");
            Assert.AreEqual("test", data.Get<string>("Name,Description"), "Name,Description");
            Assert.AreEqual("test", data.Get<string>("Description,Name"), "Description,Name");

            Dictionary<string, object> data2 = new Dictionary<string, object>
            {
                {"Description","example" }
            };

            Assert.AreEqual("example", data2.Get<string>("Description"), "Description");
            Assert.AreEqual("example", data2.Get<string>("Name,Description"), "Name,Description");
            Assert.AreEqual("example", data2.Get<string>("Description,Name"), "Description,Name");
        }

        [Test]
        public void SettingRotationsXYZ()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.SetRotations(new Vector3D(0, 0, 45));

            Matrix3D localToWorld = data.GetLocalToWorld();
            Point3D point = localToWorld.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(0.71, Round(point.X, 2), "point.X");
            Assert.AreEqual(0.71, Round(point.Y, 2), "point.Y");
        }

        [Test]
        public void SettingRotationsOTS()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.SetRotationsOTS(new Vector3D(45, 0, 0));

            Matrix3D localToWorld = data.GetLocalToWorld();
            Point3D point = localToWorld.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(0.71, Round(point.X, 2), "point.X");
            Assert.AreEqual(0.71, Round(point.Y, 2), "point.Y");

            data.SetRotationsOTS(new Vector3D(45, 30, 0));
            localToWorld = data.GetLocalToWorld();
            point = localToWorld.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(0.71, Round(point.X, 2), "point.X");
            Assert.AreEqual(0.71, Round(point.Y, 2), "point.Y");
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
            Assert.AreEqual(0.966, Round(xvec1.X, 3), "xvec1.X");
            Assert.AreEqual(0.259, Round(xvec1.Y, 3), "xvec1.Y");
            Assert.AreEqual(0.000, Round(xvec1.Z, 3), "xvec1.Z");
            Vector3D rotXYZ1 = m1.GetRotationsXYZ();
            Assert.AreEqual(0.0, Round(rotXYZ1.X, 3), "rotXYZ1.X");
            Assert.AreEqual(0.0, Round(rotXYZ1.Y, 3), "rotXYZ1.Y");
            Assert.AreEqual(15.0, Round(rotXYZ1.Z, 3), "rotXYZ1.Z");

            Dictionary<string, object> d2 = new Dictionary<string, object>
            {
                {"Orientation", "15 deg" },
                {"Tilt","-60 deg" }
            };
            Matrix3D m2 = d2.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, -60, 0));
            Vector3D zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            Assert.AreEqual(-0.224, Round(zvec2.X, 3), "zvec2.X");
            Assert.AreEqual(0.837, Round(zvec2.Y, 3), "zvec2.Y");
            Assert.AreEqual(0.500, Round(zvec2.Z, 3), "zvec2.Z");
            Vector3D yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            Assert.AreEqual(-0.129, Round(yvec2.X, 3), "yvec2.X");
            Assert.AreEqual(0.483, Round(yvec2.Y, 3), "yvec2.Y");
            Assert.AreEqual(-0.866, Round(yvec2.Z, 3), "yvec2.Z");
            Vector3D rotXYZ2 = m2.GetRotationsXYZ();
            Assert.AreEqual(-60, Round(rotXYZ2.X, 3), "rotXYZ2.X");
            Assert.AreEqual(0.0, Round(rotXYZ2.Y, 3), "rotXYZ2.Y");
            Assert.AreEqual(15.0, Round(rotXYZ2.Z, 3), "rotXYZ2.Z");

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
            Assert.AreEqual(0.966, Round(xvec1.X, 3), "xvec1.X");
            Assert.AreEqual(0.259, Round(xvec1.Y, 3), "xvec1.Y");
            Assert.AreEqual(0.000, Round(xvec1.Z, 3), "xvec1.Z");

            IDictionary d1c = d1.ConvertRotationsXYZtoOTS() as IDictionary;
            //Assert.AreEqual(d1.Count, d1c.Count);
            Assert.False(d1c.Contains("XDirection"), "d1c.Contains(\"XDirection\"");
            Assert.AreEqual(15.0, Round(d1c.GetAngleDegrees("Orientation"), 3), "d1c Orientation");

            Dictionary<string, object> d2 = new Dictionary<string, object>
            {
                {"RotationZ" , "15 deg" },
                {"RotationX","-60 deg" }
            };
            Matrix3D m2 = d2.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, -60, 0));
            Vector3D zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            Assert.AreEqual(-0.224, Round(zvec2.X, 3), "zvec2.X");
            Assert.AreEqual(0.837, Round(zvec2.Y, 3), "zvec2.Y");
            Assert.AreEqual(0.500, Round(zvec2.Z, 3), "zvec2.Z");
            Vector3D yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            Assert.AreEqual(-0.129, Round(yvec2.X, 3), "yvec2.X");
            Assert.AreEqual(0.483, Round(yvec2.Y, 3), "yvec2.Y");
            Assert.AreEqual(-0.866, Round(yvec2.Z, 3), "yvec2.Z");

            IDictionary d2c = d2.ConvertRotationsXYZtoOTS() as IDictionary;
            Assert.AreEqual(d2.Count, d2c.Count);
            Assert.AreEqual(15.0, Round(d2c.GetAngleDegrees("Orientation"), 3), "d2c Orientation");
            //Assert.AreEqual(-60.0, Round(d2c.GetAngleDegrees("Tilt"), 3), "d2c Tilt");
        }

        [Test]
        public void SetRotationsOTS()
        {
            Dictionary<string, object> d1 = new Dictionary<string, object> { };
            d1.SetRotationsOTS(new Vector3D(45.0, 124.0, 0.0));
            Matrix3D m1 = d1.GetLocalToWorld();
            Assert.AreEqual(45.0, Round(m1.GetOrientation(), 3), "Orientation");
            Assert.AreEqual(124.0, Round(m1.GetTilt(), 3), "Tilt");

            Vector3D ots = d1.GetRotationsOTS();
            Assert.AreEqual(45.0, Round(ots.X, 3), "ots.X");
            Assert.AreEqual(124.0, Round(ots.Y, 3), "ots.Y");
            Assert.AreEqual(0.0, Round(ots.Z, 3), "ots.Z");

            d1.SetRotationsOTS(new Vector3D(60.0, 15, 0));
            m1 = d1.GetLocalToWorld();
            Assert.AreEqual(60.0, Round(m1.GetOrientation(), 3), "Orientation");
            Assert.AreEqual(15.0, Round(m1.GetTilt(), 3), "Tilt");
        }
    }
}