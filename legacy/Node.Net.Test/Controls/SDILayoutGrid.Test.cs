using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Test
{
    [TestFixture]
    class SDILayoutGridTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void SDILayoutGrid_Usage()
        {
            var sdiLayoutGrid = new SDILayoutGrid();
            //sdiLayoutGrid.NavigationFrame.Views.Add("TreeView", new Node.Net.Controls.TreeView());
            //sdiLayoutGrid.SelectionFrame.Views.Add("Properties", new Node.Net.Controls.Properties());
            sdiLayoutGrid.ViewFrame.Views.Add("Gray", new Label { Background = Brushes.Gray });
            sdiLayoutGrid.DataContext = new KeyValuePair<string, dynamic>("SampleDictionary", GlobalFixture.GetSampleDictionary());
            var w = new Window
            {
                Title = "SDILayoutGridTest",
                Content = sdiLayoutGrid,
                WindowState = WindowState.Maximized
            };
            w.ShowDialog();
        }
    }
}
