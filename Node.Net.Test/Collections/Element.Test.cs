using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Node.Net.Collections
{
    [TestFixture]
    class ElementTest
    {
        [Test]
        public void Element_Default_Constructor()
        {
            var element = new Element();
            Assert.False(element.HasKey("wrong"));
        }

        [Test]
        public void Element_Key()
        {

        }
        [Test]
        public void Element_Primitive_Keys()
        {
            var element = new Element
            {
                Model = new Dictionary<string, dynamic>
                {
                    { "boolean", false }, {"name","test" }, {"number", 1.23 }
                }
            };
            Assert.True(element.HasKey("boolean"));
            Assert.NotNull(element.Get("name"));
            Assert.AreEqual(false, element.Get<bool>("boolean"));

            element.Set("new_value", "abc");
            Assert.AreEqual("abc", element.Get<string>("new_value"));
            element.Remove("new_value");
            Assert.False(element.HasKey("new_value"));
        }

        [Test]
        public void Element_ItemsSource()
        {
            var element = new Element();
            Assert.AreEqual(0, element.ItemsSource.Count);
        }

        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Element_Views()
        {
            new Window
            {
                Title = "Element_Views",
                WindowState = WindowState.Maximized,
                Content = new Test.ElementViews()
            }.ShowDialog();
        }
    }
}
