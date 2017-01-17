using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class Properties : System.Windows.Controls.Grid
    {
        public Properties()
        {
            DataContextChanged += _DataContextChanged;
        }

        public bool ShowHeader
        {
            get { return (_header.Visibility == Visibility.Visible) ? true : false; }
            set
            {
                _header.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }
        private Header _header = new Header();
        private ValuesGrid _values = new ValuesGrid();
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Children.Add(_header);
            Children.Add(_values);
            Grid.SetRow(_values, 1);
        }
        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update(this);
        }

        private void Update(System.Windows.Controls.Grid grid)
        {
            _header.DataContext = DataContext;
            _values.DataContext = DataContext;
        }
    }
}
