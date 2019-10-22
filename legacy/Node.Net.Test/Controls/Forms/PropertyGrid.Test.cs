using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls.Test.Forms
{
    [TestFixture]
    class PropertyGridTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void PropertyGrid_IDictionaryCustomTypeDescriptor()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 ft";
            dictionary["Y"] = "20 ft";
            dictionary["Orientation"] = "0 degrees";

            var adapter = new IDictionaryPropertyAdapter(dictionary);
            var window = new Window
            {
                Title = "Properties_IDictionaryPropertyAdapter",
                Content = new Node.Net.Controls.Forms.PropertyGrid { DataContext = adapter },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
