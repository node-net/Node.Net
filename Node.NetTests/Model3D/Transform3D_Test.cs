

namespace Node.Net.Model3D
{
    [NUnit.Framework.TestFixture, NUnit.Framework.Category("Transform3D")]
    public class Transform3D_Test 
        : System.Collections.Generic.Dictionary<string,Transform3D>
    {
        [NUnit.Framework.SetUp]
        public void SetUp()
        {
            Clear();
            Add("default", new Transform3D());
        }

        [NUnit.Framework.TestCase]
        public void Transform3D_Usage()
        {
            Transform3D transform = new Transform3D();
            System.Windows.Media.Media3D.Point3D local = new System.Windows.Media.Media3D.Point3D(0,0,0);
            System.Windows.Media.Media3D.Point3D parent = new System.Windows.Media.Media3D.Point3D(0, 0, 0);
            System.Windows.Media.Media3D.Point3D world = new System.Windows.Media.Media3D.Point3D(0,0,0);
            System.Windows.Media.Media3D.Point3D tmp =  transform.Transform(local,Transform3D.TransformType.LocalToWorld);
            NUnit.Framework.Assert.AreEqual(0,tmp.X);
            NUnit.Framework.Assert.AreEqual(0,tmp.Y);
            NUnit.Framework.Assert.AreEqual(0,tmp.Z);
            tmp = transform.Transform(world,Transform3D.TransformType.LocalToWorld);
            NUnit.Framework.Assert.AreEqual(0, tmp.X);
            NUnit.Framework.Assert.AreEqual(0, tmp.Y);
            NUnit.Framework.Assert.AreEqual(0, tmp.Z);

            transform.Translation = new System.Windows.Media.Media3D.Point3D(10, 20, 0);
            parent = transform.Transform(local,Transform3D.TransformType.LocalToParent);
            NUnit.Framework.Assert.True(System.Math.Abs(parent.X - 10) < 0.001);
            NUnit.Framework.Assert.True(System.Math.Abs(parent.Y - 20) < 0.001);
            NUnit.Framework.Assert.True(System.Math.Abs(parent.Z) < 0.001);
            local = transform.Transform(new System.Windows.Media.Media3D.Point3D(10, 20, 0),Transform3D.TransformType.ParentToLocal);
            NUnit.Framework.Assert.True(System.Math.Abs(local.X) < 0.001);

            Transform3D transform2 = new Transform3D();
            transform2.Translation = new System.Windows.Media.Media3D.Point3D(1, 2, 0);
            System.Windows.Media.Media3D.Point3D translation = transform2.Translation;
            NUnit.Framework.Assert.True(System.Math.Abs(translation.X - 1) < 0.001);
            transform2.Parent = transform;
            NUnit.Framework.Assert.AreSame(transform2.Parent, transform);
            world = transform2.Transform(new System.Windows.Media.Media3D.Point3D(0, 0, 0),Transform3D.TransformType.LocalToWorld);
            NUnit.Framework.Assert.True(System.Math.Abs(world.X - 11) < 0.001);

            local = transform2.Transform(new System.Windows.Media.Media3D.Point3D(11, 22, 0),Transform3D.TransformType.WorldToLocal);
            NUnit.Framework.Assert.True(System.Math.Abs(local.X) < 0.001);
            NUnit.Framework.Assert.True(System.Math.Abs(local.Y) < 0.001);
        }

        [NUnit.Framework.TestCase]
        public void Transform3D_IDictionary_Constructor()
        {
            System.Collections.Generic.Dictionary<string, string> dictionary
                = new System.Collections.Generic.Dictionary<string, string>();
            dictionary["X"] = "1 ft";
            dictionary["Y"] = "20 ft";
            dictionary["Z"] = "3 ft";
            dictionary["Orientation"] = "5 deg";
            Transform3D transform = new Transform3D(dictionary);
        }

        [NUnit.Framework.TestCase]
        public void Transform3D_Project_Point_To_Plane()
        {
            System.Windows.Media.Media3D.Point3D pointOnPlane
                = Transform3D.ProjectPointToPlane(
                     new System.Windows.Media.Media3D.Point3D(10, 10, 10),
                     new System.Windows.Media.Media3D.Point3D(0, 0, 0),
                     new System.Windows.Media.Media3D.Vector3D(0, 0, 1));
            NUnit.Framework.Assert.AreEqual(10, pointOnPlane.X);
            NUnit.Framework.Assert.AreEqual(10, pointOnPlane.Y);
            NUnit.Framework.Assert.AreEqual(0, pointOnPlane.Z);
        }

        [NUnit.Framework.TestCase]
        public void Transform3D_World_Spin()
        {
            Transform3D transform = new Transform3D();
            transform.RotationOTS = new System.Windows.Media.Media3D.Point3D(15, 15, 0);
            NUnit.Framework.Assert.AreEqual(0,
                System.Math.Round(transform.WorldRotationOTS.Z, 3));
            /*
            for (double spin = 0; spin < 180; spin += 15)
            {
                for (double tilt = 15; tilt < 16; tilt += 15)
                {
                    for (double orientation = 0; orientation < 90; orientation += 15)
                    {
                        transform.RotationOTS = new System.Windows.Media.Media3D.Point3D(orientation, tilt, spin);
                        NUnit.Framework.Assert.AreEqual(spin,
                            System.Math.Round(transform.WorldRotationOTS.Z, 0),
                            "Spin " + spin.ToString() + " Tilt " + tilt.ToString() +
                            " Orientation " + orientation.ToString());
                    }
                }
            }*/
        }

        [NUnit.Framework.TestCase]
        public void Transform3D_World_Tilt()
        {
            Transform3D transform = new Transform3D();
            for (double orientation = -180; orientation < 180; orientation += 15)
            {
                for (double tilt = -165; tilt < 181; tilt += 15)
                {
                    for (double spin = 0; spin < 1; spin += 15)
                    {
                        Transform3D transformA = new Transform3D();
                        transformA.RotationOTS = new System.Windows.Media.Media3D.Point3D(orientation, tilt, spin);
                        NUnit.Framework.Assert.AreEqual(tilt, 
                            System.Math.Round(transformA.WorldRotationOTS.Y,4),
                            orientation.ToString() + "," +
                            tilt.ToString() + "," + spin.ToString());
                    }
                }
            }
        }

        [NUnit.Framework.TestCase]
        public void Transform3D_World_Orientation()
        {
            Transform3D transform = new Transform3D();
            transform.RotationOTS = new System.Windows.Media.Media3D.Point3D(15,105, 0);
            NUnit.Framework.Assert.AreEqual(15, System.Math.Round(transform.WorldRotationOTS.X,4));

            for (double orientation = -165; orientation < 180; orientation += 15)
            {
                for (double tilt = -180; tilt < 181; tilt += 15)
                {
                    for (double spin = 0; spin < 1; spin += 15)
                    {
                        Transform3D transformA = new Transform3D();
                        transformA.RotationOTS = new System.Windows.Media.Media3D.Point3D(orientation, tilt, spin);
                        NUnit.Framework.Assert.AreEqual(orientation, 
                            System.Math.Round(transformA.WorldRotationOTS.X,4),
                            orientation.ToString() + "," +
                            tilt.ToString() + "," + spin.ToString());
                    }
                }
            }
        }

        
        [NUnit.Framework.TestCase, NUnit.Framework.Category("Stress")]
        public void Transform3D_Stress_LocalToWorld()
        {
            Transform3D transform = new Transform3D();
            transform.Translation = new System.Windows.Media.Media3D.Point3D(100, 200, 300);
            transform.RotationOTS = new System.Windows.Media.Media3D.Point3D(45, 10, 1);

            System.Windows.Media.Media3D.Point3D source = new System.Windows.Media.Media3D.Point3D();
            System.Windows.Media.Media3D.Point3D result = new System.Windows.Media.Media3D.Point3D();
            double sum = 0;
            for(int i = 0; i < 1000000;++i)
            {
                result = transform.Transform(source, Transform3D.TransformType.LocalToWorld);
                sum += result.X;
            }
        }
        [NUnit.Framework.TestCase, NUnit.Framework.Category("Stress")]
        public void Transform3D_Stress_WorldToLocal()
        {
            Transform3D transform = new Transform3D();
            transform.Translation = new System.Windows.Media.Media3D.Point3D(100, 200, 300);
            transform.RotationOTS = new System.Windows.Media.Media3D.Point3D(45, 10, 1);

            System.Windows.Media.Media3D.Point3D source = new System.Windows.Media.Media3D.Point3D();
            System.Windows.Media.Media3D.Point3D result = new System.Windows.Media.Media3D.Point3D();
            double sum = 0;
            for (int i = 0; i < 1000000; ++i)
            {
                result = transform.Transform(source, Transform3D.TransformType.WorldToLocal);
                sum += result.X;
            }
        }
    }
}
