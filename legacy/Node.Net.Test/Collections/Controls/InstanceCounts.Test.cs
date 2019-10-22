using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Node.Net.Collections.Test.Controls
{
    class InstanceCountsTest : Fixture
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void InstanceCounts_Usage()
        {
            var window = new Window
            {
                Title = "InstanceCounts_Usage",
                Content = new Node.Net.Collections.Controls.InstanceCounts
                {
                    DataContext = new KeyValuePair<string, dynamic>("States.json", Read("States.json")),
                    Key = "Type"
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
