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
            Collections.Dictionary dictionary = new Collections.Dictionary();
            dictionary["string"] = "abc";
            dictionary["bool"] = false;
            dictionary["number"] = 5.5;
            dictionary["null"] = null;

            System.Windows.Window window = new System.Windows.Window()
            {
                Title = "PropertyView_Usage",
                Content = new Controls.PropertyView()
                {
                    DataContext = dictionary
                }
            };
            window.ShowDialog();
        }
    }
}
