using NUnit.Framework;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
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
                    if(!object.ReferenceEquals(null,PropertyChanged))
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
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
            WidgetPC widget = new WidgetPC();

            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(new PropertyControl() { DataContext = widget });
            PropertyControl pc2 = new PropertyControl() { DataContext = widget };
            grid.Children.Add(pc2);
            Grid.SetColumn(pc2, 1);
            Window w = new Window()
            {
                Title = "PropertyControl_Synchronization",
                Content = grid
            };
            w.ShowDialog();
        }
    }
}
