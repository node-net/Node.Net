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
    [TestFixture, Category("Controls")]
    class CollapseDecorator2Test
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void CollapseDecorator2_Usage()
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            for (int i = 0; i < 125; ++i)
            {
                stackPanel.Children.Add(new Label { Content = i.ToString() });
            }

            var collapser = new CollapseDecorator2
            {
                Child = stackPanel
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
