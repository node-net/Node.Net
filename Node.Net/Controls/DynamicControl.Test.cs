using NUnit.Framework;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls
{
    [TestFixture,Category("Controls.DynamicControl")]
    class DynamicControlTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void DynamicControl_Usage()
        {
            DynamicControl dynamicControl = new DynamicControl();
            Window w = new Window()
            {
                Title = "DynamicControl Usage",
                WindowState = WindowState.Maximized,
                Content = dynamicControl
            };
            w.ShowDialog();
        }
    }
}
