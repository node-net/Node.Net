using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Deprecated.Model
{
    [TestFixture, Category(nameof(Model))]
    class SpatialElementTest
    {
        [Test]
        public void SpatialElement_Parent_Child()
        {
            var root = new SpatialElement();
            var child = new SpatialElement();
            root["child0"] = child;
            root.GetChildren();
            Assert.AreSame(root, child.Parent);
        }
        [Test]
        public void SpatialElement_Translation3D()
        {
            var root = new SpatialElement
            {
                X = "10 m",
                Y = "20 m",
                Z = "30 m"
            };
            Assert.AreEqual(10, root.Translation3D.X);
            Assert.AreEqual(20, root.Translation3D.Y);
            Assert.AreEqual(30, root.Translation3D.Z);

            var localToParent = root.LocalToParent;
            Assert.False(localToParent.IsIdentity);
            var parentOrigin = root.TransformLocalToParent(new Point3D(0, 0, 0));
            Assert.AreEqual(10, parentOrigin.X);
            Assert.AreEqual(20, parentOrigin.Y);
            Assert.AreEqual(30, parentOrigin.Z);

            var parentToWorld = root.GetParentToWorld();
            Assert.True(parentToWorld.IsIdentity, "parentToWorld was not identity");

            Assert.False(root.GetLocalToWorld().IsIdentity);
            var worldOrigin = root.TransformLocalToWorld(new Point3D(0, 0, 0));
            Assert.AreEqual(10, worldOrigin.X);
            Assert.AreEqual(20, worldOrigin.Y);
            Assert.AreEqual(30, worldOrigin.Z);

            var localOrigin = root.TransformWorldToLocal(worldOrigin);
            Assert.True(Abs(localOrigin.X) < 0.000001);
            Assert.True(Abs(localOrigin.Y) < 0.000001);
            Assert.True(Abs(localOrigin.Z) < 0.000001);
        }

        [Test]
        public void SpatialElement_Rotation3D()
        {
            var root = new SpatialElement
            {
                ZAxisRotation = "90 deg"
            };

            var localToParent = root.LocalToParent;
            Assert.False(localToParent.IsIdentity);
            var parentOrigin = root.TransformLocalToParent(new Point3D(1, 0, 0));
            Assert.True(Abs(parentOrigin.X) < 0.00001);
            Assert.True(Abs(parentOrigin.Y - 1) < 0.00001);
            Assert.True(Abs(parentOrigin.Z) < 0.00001);

            var parentToWorld = root.GetParentToWorld();
            Assert.True(parentToWorld.IsIdentity, "parentToWorld was not identity");

            Assert.False(root.GetLocalToWorld().IsIdentity);
            var worldOrigin = root.TransformLocalToWorld(new Point3D(1, 0, 0));
            Assert.True(Abs(worldOrigin.X) < 0.00001);
            Assert.True(Abs(worldOrigin.Y - 1) < 0.00001);
            Assert.True(Abs(worldOrigin.Z) < 0.00001);

        }
        [Test]
        public void SpatialElement_WorldZAxisRotation()
        {
            var root = new SpatialElement();

            Assert.AreEqual(0, root.GetWorldZAxisRotation());

            for (double orientation = -175; orientation < 181; orientation += 15)
            {
                root = new SpatialElement { ZAxisRotation = $"{orientation} deg" };
                Assert.AreEqual(orientation, Round(root.GetWorldZAxisRotation(), 1));
                Assert.AreEqual(0, Round(root.GetWorldTilt(), 1), $"tilt is non zero at orientation {orientation}");
            }
        }
        [Test]
        public void SpatialElement_WorldTilt()
        {
            var root = new SpatialElement();
            Assert.AreEqual(0, root.GetWorldTilt());

            for (double tilt = -175; tilt < 180; tilt += 15)
            {
                root = new SpatialElement { YAxisRotation = $"{tilt} deg" };
                Assert.AreEqual(0, Round(root.GetWorldZAxisRotation(), 1), "Orientation");
                Assert.AreEqual(tilt, Round(root.GetWorldTilt(), 1));
            }
            /*
            for (double tilt = -175; tilt < 180; tilt += 15)
            {
                root = new SpatialElement { YAxisRotation = $"{tilt} deg" };
                Assert.AreEqual(tilt, Round(root.GetWorldTilt(), 1));
            }*/
        }
        [Test]
        public void SpatialElement_WorldSpin()
        {
            /*
            var root = new SpatialElement();

            Assert.AreEqual(0, root.GetWorldSpin());

            root.XAxisRotation = "-175 deg";
            Assert.AreEqual(-175.0, Round(root.GetWorldSpin(), 1));

            for (double angle = 0; angle < 180; angle += 15)
            {
                root = new SpatialElement { XAxisRotation = $"{angle} deg" };
                Assert.AreEqual(angle, Round(root.GetWorldSpin(), 1));
            }

            root.XAxisRotation = "105 deg";
            Assert.AreEqual(105.0, Round(root.GetWorldSpin(), 1));

            for (double angle = -175; angle < 180; angle += 15)
            {
                root = new SpatialElement { XAxisRotation = $"{angle} deg" };
                Assert.AreEqual(angle, Round(root.GetWorldSpin(), 1));
            }*/
        }
        [Test]
        public void SpatialElement_WorldOrientationAndTilt()
        {
            var root = new SpatialElement
            {
                ZAxisRotation = "-175 deg",
                YAxisRotation = "-75 deg"
            };

            Assert.AreEqual(-175, Round(root.GetWorldZAxisRotation(), 1), $"[Orientation is incorrect] orientation -175 tilt -75");
            Assert.AreEqual(-75, Round(root.GetWorldTilt(), 1), $"[Tilt is incorrect] orientation -175 tilt -75");

            root = new SpatialElement
            {
                ZAxisRotation = "105 deg",
                YAxisRotation = "15 deg"
            };

            Assert.AreEqual(105, Round(root.GetWorldZAxisRotation(), 1), $"[Orientation is incorrect] orientation 105 tilt 15");
            Assert.AreEqual(15, Round(root.GetWorldTilt(), 1), $"[Tilt is incorrect] orientation 105 tilt 15");

            for (double orientation = -175; orientation < 181; orientation += 15)
            {
                for (double tilt = -75; tilt < 90; tilt += 15)
                {
                    root = new SpatialElement { ZAxisRotation = $"{orientation} deg", YAxisRotation = $"{tilt} deg" };
                    Assert.AreEqual(orientation, Round(root.GetWorldZAxisRotation(), 1), $"[Orientation is incorrect] orientation {orientation} tilt {tilt}");
                    Assert.AreEqual(tilt, Round(root.GetWorldTilt(), 1), $"[Tilt is incorrect] orientation {orientation} tilt {tilt}");
                    Assert.AreEqual(0, Round(root.GetWorldSpin(), 1), $"[Spin is incorrect] orientation {orientation} tilt {tilt}");
                    var worldRotations = root.GetWorldRotations();
                    Assert.AreEqual(0, Round(worldRotations.X, 1), $"worldRotations.X in not zero for orientation {orientation} tilt {tilt}");
                }

            }

            root = new SpatialElement { ZAxisRotation = "0 deg", YAxisRotation = "60 deg" };
            Assert.AreEqual(0, Round(root.GetWorldZAxisRotation(), 1), "Orientation is incorrect for YAxisRotation of 60 deg");
            Assert.AreEqual(60, Round(root.GetWorldTilt(), 1), "Tilt is incorrect for ZAxisRotation of 0 deg");

            root = new SpatialElement { ZAxisRotation = "180 deg", YAxisRotation = "60 deg" };
            Assert.AreEqual(180, Round(root.GetWorldZAxisRotation(), 1), "Orientation is incorrect for YAxisRotation of 60 deg");
            Assert.AreEqual(60, Round(root.GetWorldTilt(), 1), "Tilt is incorrect for ZAxisRotation of 180 deg");
        }

        [Test]
        public void SpatialElement_WorldOrientationAndTiltNested()
        {
            var root1 = new SpatialElement();
            var root2 = new SpatialElement();

            var orient1 = new SpatialElement{ ZAxisRotation = "0 deg" };
            var orient2 = new SpatialElement{ ZAxisRotation = "180 deg" };

            var tilt1 = new SpatialElement{ YAxisRotation = "60 deg" };
            var tilt2 = new SpatialElement{ YAxisRotation = "60 deg" };

            orient1["tilt"] = tilt1;
            orient2["tilt"] = tilt2;

            root1["orient"] = orient1;
            root2["orient"] = orient2;

            root1.DeepCollect<SpatialElement>();
            root2.DeepCollect<SpatialElement>();

            Assert.AreEqual(0, Round(orient1.GetWorldZAxisRotation(), 1), "orient1.GetWorldOrientation");
            Assert.AreEqual(180, Round(orient2.GetWorldZAxisRotation(), 1), "orient2.GetWorldOrientation");

            Assert.AreEqual(0, Round(tilt1.GetWorldZAxisRotation(), 1), "tilt1.GetWorldOrientation");
            Assert.AreEqual(180, Round(tilt2.GetWorldZAxisRotation(), 1), "tilt2.GetWorldOrientation");

            Assert.AreEqual(60, Round(tilt1.GetWorldTilt(), 1), "tilt1.GetWorldTilt");
            Assert.AreEqual(60, Round(tilt2.GetWorldTilt(), 1), "tilt2.GetWorldTilt");
        }
    }
}
