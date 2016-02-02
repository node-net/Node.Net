using NUnit.Framework;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls
{
    [TestFixture,Category("Node.Net.Controls.ExplorerControl")]
    class ExplorerControlTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void ExplorerControl_ShowDialog()
        {
            Window w = new Window()
            {
                Title = "ExplorerControl_ShowDialog",
                Content = new ExplorerControl()
            };
            w.ShowDialog();
        }
    }
}
