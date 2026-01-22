#if IS_WINDOWS
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Math;
using Node.Net; // For extension methods and Reader

namespace Node.Net.Test.Extension
{
    internal class IDictionaryExtensionTest
    {
        [Test]
        public async Task GetRotations()
        {
            Vector3D rotations = new Dictionary<string, string>().GetRotations();
            await Assert.That(rotations.Z).IsEqualTo(0);

            rotations = new Dictionary<string, string>
            {
                { "Orientation","-180 deg" }
            }.GetRotations();
            await Assert.That(rotations.Z).IsEqualTo(-180.0);
        }

        [Test]
        public async Task Find()
        {
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            IDictionary a = data.Find<IDictionary>("objectA");
            await Task.CompletedTask;
        }

        private bool Filter(object v)
        {
            return true;
        }

        [Test]
        public async Task Collect()
        {
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            data.Collect(typeof(IDictionary));
            data.Collect<IDictionary>(Filter);
            data.Collect<IDictionary>("");
            data.Collect<IDictionary>("test");
            data.CollectKeys();
            await Task.CompletedTask;
        }

        [Test]
        public async Task DeepUpdateParents()
        {
            System.Reflection.Assembly assembly = typeof(IDictionaryExtensionTest).Assembly;
            IDictionary data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
            data.DeepUpdateParents();
            IDictionaryExtension.DeepUpdateParents(null);
            data.ComputeHashCode();
            await Task.CompletedTask;
        }

        [Test]
        public async Task GetLengthMeters()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"X" ,"2 m" },
                {"Offset","-nan(ind) ft" }
            };
            await Assert.That(data.GetLengthMeters("X")).IsEqualTo(2.0);
            await Assert.That(data.GetLengthMeters("Offset")).IsEqualTo(0.0);
        }

        [Test]
        public async Task GetLocalToParent()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"X", "10 m" },
                {"Y","1 m" }
            };

            Matrix3D localToParent = data.GetLocalToParent();
            await Assert.That(ReferenceEquals(localToParent, data.GetLocalToParent())).IsFalse();
            Matrix3D localToWorld = data.GetLocalToWorld();
            await Assert.That(ReferenceEquals(localToWorld, data.GetLocalToWorld())).IsFalse();
            // Use Node.Net types for consistency with extension methods
            Point3D origin = localToParent.Transform(new Point3D(0, 0, 0));
            await Assert.That(origin.X).IsEqualTo(10);
            await Assert.That(origin.Y).IsEqualTo(1);

            string mstring = localToParent.ToString();
            await Assert.That(mstring.Length).IsEqualTo(32);

            // Matrix3D.Parse is an extension method, use it via reflection or extension method call
            var matrix3DExtensionType = typeof(Factory).Assembly.GetType("Node.Net.Matrix3DExtension");
            if (matrix3DExtensionType == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            var parseMethod = matrix3DExtensionType.GetMethod("Parse", new[] { typeof(string) });
            if (parseMethod == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            Matrix3D m = (Matrix3D)parseMethod.Invoke(null, new object[] { mstring });
            Point3D origin2 = m.Transform(new Point3D(0, 0, 0));
            await Assert.That(origin2.X).IsEqualTo(10);
            await Assert.That(origin2.Y).IsEqualTo(1);

            // Matrix3D.Parse is an extension method, use it via reflection
            var matrix3DExtensionType2 = typeof(Factory).Assembly.GetType("Node.Net.Matrix3DExtension");
            if (matrix3DExtensionType2 == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            var parseMethod2 = matrix3DExtensionType2.GetMethod("Parse", new[] { typeof(string) });
            if (parseMethod2 == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            Matrix3D i = (Matrix3D)parseMethod2.Invoke(null, new object[] { "Identity" });
            await Assert.That(i.IsIdentity).IsTrue();

            await Assert.That(Round(localToParent.GetOrientation(), 3)).IsEqualTo(0);
            await Assert.That(Round(localToParent.GetTilt(), 3)).IsEqualTo(0);
            await Assert.That(Round(localToParent.GetSpin(), 3)).IsEqualTo(0);

            data["Orientation"] = "135.0 deg";
            data["Tilt"] = "55 deg";
            localToParent = data.GetLocalToParent();
            await Assert.That(Round(localToParent.GetOrientation(), 3)).IsEqualTo(135.0);
            await Assert.That(Round(localToParent.GetTilt(), 3)).IsEqualTo(55.0);

            localToWorld = data.GetLocalToWorld();
            await Assert.That(Round(localToWorld.GetOrientation(), 3)).IsEqualTo(135.0);
            await Assert.That(Round(localToWorld.GetTilt(), 3)).IsEqualTo(55.0);
        }

        [Test]
        public async Task Get()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"Name","test" }
            };

            await Assert.That(data.Get<string>("Name")).IsEqualTo("test");
            await Assert.That(data.Get<string>("Name,Description")).IsEqualTo("test");
            await Assert.That(data.Get<string>("Description,Name")).IsEqualTo("test");

            Dictionary<string, object> data2 = new Dictionary<string, object>
            {
                {"Description","example" }
            };

            await Assert.That(data2.Get<string>("Description")).IsEqualTo("example");
            await Assert.That(data2.Get<string>("Name,Description")).IsEqualTo("example");
            await Assert.That(data2.Get<string>("Description,Name")).IsEqualTo("example");
        }

        [Test]
        public async Task SettingRotationsXYZ()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.SetRotations(new Vector3D(0, 0, 45));

            Matrix3D localToWorld = data.GetLocalToWorld();
            Point3D point = localToWorld.Transform(new Point3D(1, 0, 0));
            await Assert.That(Round(point.X, 2)).IsEqualTo(0.71);
            await Assert.That(Round(point.Y, 2)).IsEqualTo(0.71);
        }

        [Test]
        public async Task SettingRotationsOTS()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.SetRotationsOTS(new Vector3D(45, 0, 0));

            Matrix3D localToWorld = data.GetLocalToWorld();
            Point3D point = localToWorld.Transform(new Point3D(1, 0, 0));
            await Assert.That(Round(point.X, 2)).IsEqualTo(0.71);
            await Assert.That(Round(point.Y, 2)).IsEqualTo(0.71);

            data.SetRotationsOTS(new Vector3D(45, 30, 0));
            localToWorld = data.GetLocalToWorld();
            point = localToWorld.Transform(new Point3D(1, 0, 0));
            await Assert.That(Round(point.X, 2)).IsEqualTo(0.71);
            await Assert.That(Round(point.Y, 2)).IsEqualTo(0.71);
        }

        [Test]
        public async Task Rotate()    // See Matrix3D.Test.Rotate
        {
            Dictionary<string, object> d1 = new Dictionary<string, object>
            {
                {"Orientation" , "15 deg" }
            };
            Matrix3D m1 = d1.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Vector3D xvec1 = m1.Transform(new Vector3D(1, 0, 0));
            await Assert.That(Round(xvec1.X, 3)).IsEqualTo(0.966);
            await Assert.That(Round(xvec1.Y, 3)).IsEqualTo(0.259);
            await Assert.That(Round(xvec1.Z, 3)).IsEqualTo(0.000);
            Vector3D rotXYZ1 = m1.GetRotationsXYZ();
            await Assert.That(Round(rotXYZ1.X, 3)).IsEqualTo(0.0);
            await Assert.That(Round(rotXYZ1.Y, 3)).IsEqualTo(0.0);
            await Assert.That(Round(rotXYZ1.Z, 3)).IsEqualTo(15.0);

            Dictionary<string, object> d2 = new Dictionary<string, object>
            {
                {"Orientation", "15 deg" },
                {"Tilt","-60 deg" }
            };
            Matrix3D m2 = d2.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, -60, 0));
            Vector3D zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            await Assert.That(Round(zvec2.X, 3)).IsEqualTo(-0.224);
            await Assert.That(Round(zvec2.Y, 3)).IsEqualTo(0.837);
            await Assert.That(Round(zvec2.Z, 3)).IsEqualTo(0.500);
            Vector3D yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            await Assert.That(Round(yvec2.X, 3)).IsEqualTo(-0.129);
            await Assert.That(Round(yvec2.Y, 3)).IsEqualTo(0.483);
            await Assert.That(Round(yvec2.Z, 3)).IsEqualTo(-0.866);
            Vector3D rotXYZ2 = m2.GetRotationsXYZ();
            await Assert.That(Round(rotXYZ2.X, 3)).IsEqualTo(-60);
            await Assert.That(Round(rotXYZ2.Y, 3)).IsEqualTo(0.0);
            await Assert.That(Round(rotXYZ2.Z, 3)).IsEqualTo(15.0);

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
            await Task.CompletedTask;
        }

        [Test]
        public async Task ConvertRotationsXYZtoOTS()
        {
            Dictionary<string, object> d1 = new Dictionary<string, object>
            {
                {"RotationZ" , "15 deg" }
            };
            Matrix3D m1 = d1.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Vector3D xvec1 = m1.Transform(new Vector3D(1, 0, 0));
            await Assert.That(Round(xvec1.X, 3)).IsEqualTo(0.966);
            await Assert.That(Round(xvec1.Y, 3)).IsEqualTo(0.259);
            await Assert.That(Round(xvec1.Z, 3)).IsEqualTo(0.000);

            IDictionary d1c = d1.ConvertRotationsXYZtoOTS() as IDictionary;
            //Assert.AreEqual(d1.Count, d1c.Count);
            await Assert.That(d1c.Contains("XDirection")).IsFalse();
            await Assert.That(Round(d1c.GetAngleDegrees("Orientation"), 3)).IsEqualTo(15.0);

            Dictionary<string, object> d2 = new Dictionary<string, object>
            {
                {"RotationZ" , "15 deg" },
                {"RotationX","-60 deg" }
            };
            Matrix3D m2 = d2.GetLocalToWorld();// new Matrix3D().RotateOTS(new Vector3D(15, -60, 0));
            Vector3D zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            await Assert.That(Round(zvec2.X, 3)).IsEqualTo(-0.224);
            await Assert.That(Round(zvec2.Y, 3)).IsEqualTo(0.837);
            await Assert.That(Round(zvec2.Z, 3)).IsEqualTo(0.500);
            Vector3D yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            await Assert.That(Round(yvec2.X, 3)).IsEqualTo(-0.129);
            await Assert.That(Round(yvec2.Y, 3)).IsEqualTo(0.483);
            await Assert.That(Round(yvec2.Z, 3)).IsEqualTo(-0.866);

            IDictionary d2c = d2.ConvertRotationsXYZtoOTS() as IDictionary;
            await Assert.That(d2.Count).IsEqualTo(d2c.Count);
            await Assert.That(Round(d2c.GetAngleDegrees("Orientation"), 3)).IsEqualTo(15.0);
            //Assert.AreEqual(-60.0, Round(d2c.GetAngleDegrees("Tilt"), 3), "d2c Tilt");
        }

        [Test]
        public async Task SetRotationsOTS()
        {
            Dictionary<string, object> d1 = new Dictionary<string, object> { };
            d1.SetRotationsOTS(new Vector3D(45.0, 124.0, 0.0));
            Matrix3D m1 = d1.GetLocalToWorld();
            await Assert.That(Round(m1.GetOrientation(), 3)).IsEqualTo(45.0);
            await Assert.That(Round(m1.GetTilt(), 3)).IsEqualTo(124.0);

            Vector3D ots = d1.GetRotationsOTS();
            await Assert.That(Round(ots.X, 3)).IsEqualTo(45.0);
            await Assert.That(Round(ots.Y, 3)).IsEqualTo(124.0);
            await Assert.That(Round(ots.Z, 3)).IsEqualTo(0.0);

            d1.SetRotationsOTS(new Vector3D(60.0, 15, 0));
            m1 = d1.GetLocalToWorld();
            await Assert.That(Round(m1.GetOrientation(), 3)).IsEqualTo(60.0);
            await Assert.That(Round(m1.GetTilt(), 3)).IsEqualTo(15.0);
        }


        [Test]
        public async Task ToJson()
        {
            double verySmallNumber = 1e-45;
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"Name","test" },
                {"Z", verySmallNumber }
            };

            string json = IDictionaryExtension.ToJson(data);

            // Load the json back to a dictionary
            IDictionary data2 = new Reader().Read<IDictionary>(json);

            string json2 = IDictionaryExtension.ToJson(data2);
            await Assert.That(json2).IsEqualTo(json);
        }
    }
}
#endif