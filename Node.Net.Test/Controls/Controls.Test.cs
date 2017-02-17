using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Node.Net.Controls
{
    [TestFixture]
    class ControlsTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Controls_Usage()
        {
            Test.ControlTester.ShowDialog();
        }
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Controls_Prototypes()
        {
            Test.ControlTester.ShowDialog(typeof(ControlsTest).Assembly);
        }

        [Test,Apartment(ApartmentState.STA)]
        public void Controls_Coverage()
        {
            Test.ControlTester.ShowCoverage();
        }
    }
}
