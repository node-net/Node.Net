using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Controls
{
    [TestFixture,Category("Controls")]
    public class CollapseDecoratorTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void CollapseDecorator_Usage()
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
                    var label = new Label { Content = $"{c},{r}" };
                    grid.Children.Add(label);
                    Grid.SetColumn(label, c);
                    Grid.SetRow(label, r);
                }
            }
            /*
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical,CanVerticallyScroll=true };
            for(int i =0; i < 250; ++i)
            {
                stackPanel.Children.Add(new Label { Content = i.ToString() });
            }*/

            var collapser = new CollapseDecorator
            {
                Child = grid
            };

            var window = new Window
            {
                Content = collapser,
                Title = "CollapseDecorator_Usage",
                WindowState = WindowState.Maximized
            };

            window.ShowDialog();
        }
    }
}
