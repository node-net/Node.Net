using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls.Test
{
    [TestFixture]
    class PropertiesTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void Properties_Usage()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 ft";
            dictionary["Y"] = "20 ft";
            dictionary["Orientation"] = "0 degrees";
            double[] angles = { 0, 10, 15, 20, 25, 30, 35, 45,46,47,48,49,50,51,52,53,54,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71 };
            double[,] values = { { 1, 2 }, { 3, 4 } };
            dictionary["Angles"] = angles;
            dictionary["Values"] = values;
            var window = new Window
            {
                Title = "Properties_Usage",
                Content = new Node.Net.Controls.Properties { DataContext = dictionary },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
