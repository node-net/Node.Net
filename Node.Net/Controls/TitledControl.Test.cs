using NUnit.Framework;
using System.Collections.Generic;
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
                Title = "TitledControl_Usage_ShowDialog",
                Content = new TitledControl()
                {
                    DataContext =
                      new KeyValuePair<string, object>(
                        "Blue", new Grid() { Background = Brushes.Blue })
                }
            };
            w.ShowDialog();
            //TitledControl tc = new TitledControl();
        }
    }
}
