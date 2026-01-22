using System.Threading.Tasks;
using static System.Math;
using Node.Net; // This brings the extension methods into scope (they're in Node.Net namespace, not Node.Net.Extension)

namespace Node.Net.Extension
{
    internal class Matrix3DExtensionTest
    {
        [Test]
        [Arguments(0, 0, 0)]
        [Arguments(45, 30, 0)]
        [Arguments(45, 30, 0)]
        public async Task RotateOTS(double orientation, double tilt, double spin)
        {
            System.Windows.Media.Media3D.Matrix3D matrix = new System.Windows.Media.Media3D.Matrix3D();
            matrix = matrix.RotateOTS(new System.Windows.Media.Media3D.Vector3D(orientation, tilt, spin)); // Extension method from Node.Net.Extension
            System.Windows.Media.Media3D.Vector3D ots = matrix.GetRotationsOTS();
            await Assert.That(Round(ots.X, 2)).IsEqualTo(orientation);
            await Assert.That(Round(ots.Y, 2)).IsEqualTo(tilt);
            await Assert.That(Round(ots.Z, 2)).IsEqualTo(spin);
        }
    }
}