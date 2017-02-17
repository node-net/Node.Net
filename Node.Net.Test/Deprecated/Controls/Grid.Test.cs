using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NUnit.Framework;

namespace Node.Net.Deprecated.Controls
{
    [TestFixture,Category("Node.Net.Controls.Grid")]
    class GridTest
    {
        public static Grid GetSampleGrid()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            return grid;
        }
        [TestCase,Apartment(ApartmentState.STA)]
        public void Grid_Usage()
        {
            var grid = GetSampleGrid();
        }

        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Grid_Usage_ShowDialog()
        {
            var grid = new Grid();
            var w = new Window { Title = nameof(Grid_Usage_ShowDialog), Content = GetSampleGrid() };
            w.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void Grid_ScrollUsage_ShowDialog()
        {
            var grid = new Grid();
            for (var c = 0; c < 100; ++c)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                for (var r = 0; r < 100; ++r)
                {
                    if (grid.RowDefinitions.Count < r + 1)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                    var label= new Label { Content = $"{c},{r}" };
                    grid.Children.Add(label);
                    Grid.SetColumn(label, c);
                    Grid.SetRow(label, r);
                }
            }
            var scrollViewer = new ScrollViewer
            {
                Content = grid,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            var w = new Window { Title = nameof(Grid_Usage_ShowDialog), Content = scrollViewer };
            w.ShowDialog();
        }
    }
}
