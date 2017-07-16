using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NUnit.Framework;

namespace Node.Net.Controls
{
    [TestFixture,Category(nameof(GridView))]
    class GridViewTest
    {
        [Test,Apartment(ApartmentState.STA),Explicit]
        public void ShowDialog()
        {
            var data = new Dictionary<string, dynamic>
            {
                {"elementA", new SpatialElement{ X="0 ft", Y="0 ft", Z="0 ft"} },
                {"elementB", new SpatialElement{X="1 ft",Y="0 ft",Z="0 ft"} }
            };
            var window = new Window
            {
                Title = "GridView",
                Content = new GridView { DataContext = data.Collect<SpatialElement>() },
                WindowState = WindowState.Maximized
            }.ShowDialog();
        }
    }
}
