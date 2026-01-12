extern alias NodeNet;
using NUnit.Framework;
using NodeNet::System.Windows.Media.Media3D;
using NodeNet::Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test.Extension
{
    [TestFixture]
    internal class Rect3DExtensionTest
    {
        [Test]
        public void Usage()
        {
            Rect3D rect = new Rect3D();
            rect.GetCenter();
            rect.Scale(1.1);
        }
    }
}