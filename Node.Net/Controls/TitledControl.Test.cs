using NUnit.Framework;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Node.Net.Controls
{
    [TestFixture,Category("Node.Net.Controls.TitledControl")]
    class TitledControlTest
    {
        [TestCase,Apartment(ApartmentState.STA)]
        public void TitledControl_Usage()
        {
            TitledControl tc = new TitledControl();
        }

        [TestCase, Apartment(ApartmentState.STA),Explicit]
        public void TitledControl_Usage_ShowDialog()
        {
            Window w = new Window()
            {
                Content = new TitledControl()
                {
                    Title = "Blue",
                    Content = new Grid() { Background = Brushes.Blue }
                }
            };
            w.ShowDialog();
            //TitledControl tc = new TitledControl();
        }
    }
}
