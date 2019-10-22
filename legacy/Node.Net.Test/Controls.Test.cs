using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net
{
    [TestFixture]
    class ControlsTest
    {
        [Test,Apartment(ApartmentState.STA),Explicit]
        public void GridLayers_With_Bitmap_ShowDialog()
        {
            var grid = Node.Net.Factory.Default.Create<Grid>("Grid.Layers.xaml");
            Assert.NotNull(grid, nameof(grid));

            var grid2 = new Grid();
            grid2.ColumnDefinitions.Add(new ColumnDefinition());
            grid2.ColumnDefinitions.Add(new ColumnDefinition());
            grid2.Children.Add(grid);
            var image = new Image { Source = grid.CreateBitmapSource(100, 100) ,Width=100,Height=100};
            grid2.Children.Add(image);
            Grid.SetColumn(image, 1);

            new Window
            {
                Title = "GridLayers With Bitmap ShowDialog",
                Width = 200,
                Height = 100,
                Content = grid2
            }.ShowDialog();
        }
    }
}
