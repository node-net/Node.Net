using NUnit.Framework;
using System.Threading;
using System.Windows;

namespace Node.Net.View
{
    [TestFixture,Category("Node.Net.View.PropertyView.Test")]
    class PropertyViewTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void PropertyView_Usage()
        {
            var dictionary = new Deprecated.Collections.Dictionary();
            dictionary["string"] = "abc";
            dictionary["bool"] = false;
            dictionary["number"] = 5.5;
            dictionary["null"] = null;

            var window = new System.Windows.Window
            {
                Title = nameof(PropertyView_Usage),
                Content = new Deprecated.Controls.PropertyView
                {
                    DataContext = dictionary
                }
            };
            window.ShowDialog();
        }
    }
}
