using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Node.Net.Controls
{
    public class SDILayoutGrid : Grid
    {
        public SDILayoutGrid()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => Update();

        private void Update() {
            NavigationFrame.DataContext = DataContext;
            SelectionFrame.DataContext = DataContext;
            ViewFrame.DataContext = DataContext;
        }

        public ViewFrame NavigationFrame = new ViewFrame();
        public ViewFrame SelectionFrame = new ViewFrame();
        public ViewFrame ViewFrame = new ViewFrame();
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());

            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
            ColumnDefinitions.Add(new ColumnDefinition());

            Children.Add(NavigationFrame);
            Children.Add(SelectionFrame);
            Grid.SetRow(SelectionFrame, 1);
            Children.Add(ViewFrame);
            var gridSplitter = new GridSplitter { Width = 5, HorizontalAlignment = HorizontalAlignment.Stretch };
            Children.Add(gridSplitter);
            Grid.SetColumn(gridSplitter, 1);
            Grid.SetColumn(ViewFrame, 2);
            Grid.SetRowSpan(ViewFrame, 2);
            Update();
        }
    }
}
