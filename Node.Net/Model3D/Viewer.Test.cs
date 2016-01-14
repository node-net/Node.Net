using NUnit.Framework;
using System.Threading;

namespace Node.Net.Model3D
{
    [TestFixture,Category("Node.Net.Model3D.Viewer")]
    class ViewerTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Viewer_Usage()
        {
            Viewer view = new Viewer();
        }
    }
}
