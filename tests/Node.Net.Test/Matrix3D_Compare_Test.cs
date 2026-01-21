using NUnit.Framework;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal class Matrix3DCompareTest
    {
        [Test]
        public void CompareRotationMatrix()
        {
            // Simple test: 15 degree rotation around Z axis
            Matrix3D m1 = new Matrix3D();
            Quaternion q1 = new Quaternion(new Vector3D(0, 0, 1), 15);
            m1.Rotate(q1);
            
            Vector3D result = m1.Transform(new Vector3D(1, 0, 0));
            
            System.Console.WriteLine($"Quaternion(0,0,1, 15): X={q1.X:F6}, Y={q1.Y:F6}, Z={q1.Z:F6}, W={q1.W:F6}");
            System.Console.WriteLine($"Matrix M11={m1.M11:F6}, M12={m1.M12:F6}, M13={m1.M13:F6}");
            System.Console.WriteLine($"Matrix M21={m1.M21:F6}, M22={m1.M22:F6}, M23={m1.M23:F6}");
            System.Console.WriteLine($"Matrix M31={m1.M31:F6}, M32={m1.M32:F6}, M33={m1.M33:F6}");
            System.Console.WriteLine($"Transform(1,0,0): X={result.X:F6}, Y={result.Y:F6}, Z={result.Z:F6}");
            
            // Expected from Windows: Y should be 0.259
            // This will help us see what our implementation produces
            Assert.That(result.X, Is.GreaterThan(0.9)); // Should be ~0.966
        }
    }
}

