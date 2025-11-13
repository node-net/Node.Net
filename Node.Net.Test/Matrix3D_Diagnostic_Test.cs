using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;
using Node.Net;

namespace Node.Net.Test
{
    [TestFixture]
    internal class Matrix3DDiagnosticTest
    {
        [Test]
        public void CaptureWindowsBehavior()
        {
            // Test 1: Simple rotation around Z axis
            Matrix3D m1 = new Matrix3D();
            Quaternion q1 = new Quaternion(new Vector3D(0, 0, 1), 15);
            m1.Rotate(q1);
            Vector3D result1 = m1.Transform(new Vector3D(1, 0, 0));

            System.Console.WriteLine($"Quaternion(0,0,1, 15): X={q1.X}, Y={q1.Y}, Z={q1.Z}, W={q1.W}");
            System.Console.WriteLine($"Matrix after Rotate: M11={m1.M11}, M12={m1.M12}, M13={m1.M13}");
            System.Console.WriteLine($"Transform(1,0,0): X={result1.X}, Y={result1.Y}, Z={result1.Z}");

            // Test 2: Check if Rotate uses Append or Prepend
            Matrix3D m2a = new Matrix3D();
            m2a.Translate(new Vector3D(10, 0, 0));
            m2a.Rotate(new Quaternion(new Vector3D(0, 0, 1), 90));
            Point3D p2a = m2a.Transform(new Point3D(1, 0, 0));
            System.Console.WriteLine($"Translate then Rotate: Point={p2a.X}, {p2a.Y}, {p2a.Z}");

            // Test 3: RotateOTS behavior
            Matrix3D m3 = new Matrix3D();
            m3.RotateOTS(new Vector3D(15, 0, 0));
            Vector3D xvec3 = m3.Transform(new Vector3D(1, 0, 0));
            System.Console.WriteLine($"RotateOTS(15,0,0) Transform(1,0,0): X={xvec3.X}, Y={xvec3.Y}, Z={xvec3.Z}");
        }
    }
}

