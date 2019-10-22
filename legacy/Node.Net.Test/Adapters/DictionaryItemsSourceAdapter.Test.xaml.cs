using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NUnit.Framework;

namespace Node.Net.Tests.Adapters
{
    /// <summary>
    /// Interaction logic for DictionaryItemsSourceAdapter.xaml
    /// </summary>
    [TestFixture,Category("DictionaryItemsSourceAdapter"),Apartment(ApartmentState.STA)]
    public partial class DictionaryItemsSourceAdapterTest : UserControl
    {
        public DictionaryItemsSourceAdapterTest()
        {
            InitializeComponent();

            var data = new Element
            {
                { "A", new Element{ {"Type" , "Widget"} } },
                { "B", new Element{ {"Type","Widget"} } },
                { "C", new Element{ { "Type", "Widget" } } }
            };

            data.DeepUpdateParents();
            var elementA = data["A"] as Element;
            Assert.AreEqual("A", elementA.Name);
            comboBox.DataContext = new Node.Net.Adapters.DictionaryItemsSourceAdapter<IDictionary> { Model = data };
        }
        [Test,Explicit]
        public void Usage()
        {
            var window = new Window
            {
                Content = new DictionaryItemsSourceAdapterTest(),
                Title = "IDictionaryItemsSourceAdapter Test",
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
