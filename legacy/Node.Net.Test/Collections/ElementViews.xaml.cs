using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Node.Net.Collections.Test
{
    /// <summary>
    /// Interaction logic for ElementViews.xaml
    /// </summary>
    public partial class ElementViews : UserControl
    {
        public ElementViews()
        {
            InitializeComponent();
            var model = Node.Net.Readers.JsonReader.Default.Read(typeof(ElementViews).Assembly.GetManifestResourceStream("Node.Net.Collections.Test.Resources.States.json")) as IDictionary;
            DataContext = new Element { Model = model };
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
