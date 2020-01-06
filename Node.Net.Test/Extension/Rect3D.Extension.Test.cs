using NUnit.Framework;
using System.Windows.Media.Media3D;

namespace Node.Net.Test.Extension
{
    [TestFixture]
    internal class Rect3DExtensionTest
    {
        [Test]
        public void Usage()
        {
            var rect = new Rect3D();
            rect.GetCenter();
            rect.Scale(1.1);
        }
    }
}