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
    class ListBoxTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void ListBox_Usage()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary.Add("A", "a");
            dictionary.Add("B", "b");
            dictionary.Add("C", "c");

            var w = new Window
            {
                Title = "ListBox_Usage",
                Content = new Node.Net.Controls.ListBox { DataContext = new KeyValuePair<string, dynamic>("test", dictionary) }
            };
            w.ShowDialog();
        }
    }
}
