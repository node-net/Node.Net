using System;
using System.Threading;
using System.Windows;
using NUnit.Framework;

namespace Node.Net.Controls
{
    [TestFixture,Category("Node.Net.Controls.Grid")]
    class GridTest
    {
        public static Grid GetSampleGrid()
        {
            Grid grid = new Grid();
            return grid;
        }
        [TestCase,Apartment(ApartmentState.STA)]
        public void Grid_Usage()
        {
            Grid grid = GetSampleGrid();
        }

        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Grid_Usage_ShowDialog()
        {
            Grid grid = new Grid();
            Window w = new Window() { Title = "Grid_Usage_ShowDialog", Content = GetSampleGrid() };
            w.ShowDialog();
        }
    }
}
