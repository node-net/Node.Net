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
    [TestFixture]
    class InstanceStackPanelTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void InstanceStackPanel_Usage()
        {
            var data = new Dictionary<string, dynamic>();
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo0/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo1/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo2/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Bars/bar1/Type", "Bar");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Widgets/widget0/Type", "Widget");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Widgets/widget1/Type", "Widget");
            Assert.True(data.ContainsKey("Bars"));
            Assert.AreEqual("Bar", IDictionaryExtension.Get<string>(data, "Bars/bar1/Type"));

            var window = new Window
            {
                Title = "InstanceStackPanel_Usage",
                Content = new Node.Net.Collections.Controls.InstanceStackPanel { Key = "Type", DataContext = data },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
