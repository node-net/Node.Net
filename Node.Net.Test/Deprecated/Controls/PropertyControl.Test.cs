using NUnit.Framework;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Controls
{
    public class WidgetPC : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name = "widget";
        public string Name
        {
            get { return name; }
            set
            {
                if(name != value)
                {
                    name = value;
                    if(PropertyChanged!=null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                    }
                }
            }
        }
    }

    [TestFixture,NUnit.Framework.Category("Node.Net.Controls.PropertyControl"), NUnit.Framework.Category("Explicit")]
    class PropertyControlTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void PropertyControl_Synchronization()
        {
            var widget = new WidgetPC();

            var grid = new System.Windows.Controls.Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(new Deprecated.Controls.PropertyControl { DataContext = widget });
            var pc2 = new Deprecated.Controls.PropertyControl { DataContext = widget };
            grid.Children.Add(pc2);
            Grid.SetColumn(pc2, 1);
            var w = new Window
            {
                Title = nameof(PropertyControl_Synchronization),
                Content = grid
            };
            w.ShowDialog();
        }
    }
}
