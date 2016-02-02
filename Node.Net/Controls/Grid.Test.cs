using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NUnit.Framework;

namespace Node.Net.Controls
{
    [TestFixture,Category("Node.Net.Controls.Grid")]
    class GridTest
    {
        public static Grid GetSampleGrid()
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
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
