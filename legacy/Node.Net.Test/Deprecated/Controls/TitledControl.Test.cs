using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls
{
    [TestFixture,Category("Node.Net.Controls.TitledControl")]
    class TitledControlTest
    {
        [TestCase,Apartment(ApartmentState.STA)]
        public void TitledControl_Usage()
        {
            var tc = new TitledControl();
        }

        [TestCase, Apartment(ApartmentState.STA),Explicit]
        public void TitledControl_Usage_ShowDialog()
        {
            var w = new Window
            {
                Title = nameof(TitledControl_Usage_ShowDialog),
                Content = new TitledControl
                {
                    DataContext =
                      new KeyValuePair<string, object>(
                        "Blue", new Grid { Background = Brushes.Blue })
                }
            };
            w.ShowDialog();
        }
    }
}
