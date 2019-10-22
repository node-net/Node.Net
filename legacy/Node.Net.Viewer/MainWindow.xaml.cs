using System;
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

namespace Node.Net.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            var doc = Document.Open() as Document;
            DataContext = doc;
            if (doc == null) Title = "Node.Net.Viewer";
            else Title = $"Node.Net.Viewer - {doc.FileName}";
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var doc = DataContext as Document;
            if(doc != null)
            {
                doc.SelectedItem = (treeView.SelectedItem as TreeElement).Element;
            }
        }
    }
}
