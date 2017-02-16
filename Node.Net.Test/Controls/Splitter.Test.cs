using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Test
{
    [TestFixture]
    class SplitterTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void Splitter_Usage_Horizontal()
        {
            var window = new Window
            {
                Title = "Splitter_Usage_Horizontal",
                Content = new Splitter
                {
                    Orientation = Orientation.Horizontal,
                    ChildA = new Label { Content = "ChildA", Background = Brushes.Green},
                    ChildB = new Label { Content = "ChildB",Background = Brushes.Yellow}
                }
            };
            window.ShowDialog();
        }
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Splitter_Usage_Vertical()
        {
            var window = new Window
            {
                Title = "Splitter_Usage_Vertical",
                Content = new Splitter
                {
                    Orientation = Orientation.Vertical,
                    ChildA = new Label { Content = "ChildA", Background = Brushes.Green },
                    ChildB = new Label { Content = "ChildB",Background = Brushes.Yellow }
                }
            };
            window.ShowDialog();
        }
    }
}
