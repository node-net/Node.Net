using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Node.Net.Deprecated.Controls
{
    [TestFixture,Category("Controls.View3D")]
    class View3DTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void View3D_Usage()
        {
            ViewTester.ShowDialog(NodeNetTests.Resources, new View3D());
        }
    }
}
