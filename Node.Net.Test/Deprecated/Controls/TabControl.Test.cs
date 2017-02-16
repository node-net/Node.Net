﻿using NUnit.Framework;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls
{
    [TestFixture,Category("Node.Net.Controls.TabControl")]
    class TabControlTest
    {
        [TestCase,Apartment(ApartmentState.STA)]
        public void TabControl_Usage()
        {
            var tabControl = new TabControl();
            tabControl.Items.Add(new TabItem { Header = "Blue", Content = new Grid { Background = Brushes.Blue } });
            tabControl.Items.Add(new TabItem { Header = "Red", Content = new Grid { Background = Brushes.Red } });
        }
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void TabControl_Usage_ShowDialog()
        {
            var tabControl = new TabControl();
            tabControl.Items.Add(new TabItem { Header = "Blue", Content = new Grid { Background = Brushes.Blue } });
            tabControl.Items.Add(new TabItem { Header = "Red", Content = new Grid { Background = Brushes.Red } });

            var w = new Window { Title = nameof(TabControl_Usage_ShowDialog), Content = tabControl };
            w.ShowDialog();
        }
    }
}
