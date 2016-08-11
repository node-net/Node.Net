using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public class Explorer : System.Windows.Controls.Grid
    {
        public Explorer()
        {
            DataContextChanged += _DataContextChanged;
        }

        private Label _header = new Label { Background = Brushes.LightGray };
        private TreeView _treeView;
        private ValuesGrid _valuesGrid;
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Children.Add(_header);

            RowDefinitions.Add(new RowDefinition());
            var horizontalSplitterGrid = new System.Windows.Controls.Grid();
            Children.Add(horizontalSplitterGrid);
            Grid.SetRow(horizontalSplitterGrid, 1);


            var verticalSplitterGrid = new System.Windows.Controls.Grid();
            verticalSplitterGrid.RowDefinitions.Add(new RowDefinition());
            verticalSplitterGrid.RowDefinitions.Add(new RowDefinition());
            _treeView = new TreeView();
            verticalSplitterGrid.Children.Add(_treeView);
            _valuesGrid = new ValuesGrid { Orientation = Orientation.Vertical };
            verticalSplitterGrid.Children.Add(_valuesGrid);
            Grid.SetRow(_valuesGrid, 1);

            horizontalSplitterGrid.Children.Add(verticalSplitterGrid);
            Update();
        }

        private void Update()
        {
            _header.Content = null;
            
            if (DataContext == null) return;
            
            var dictionary = DataContext.GetValue() as IDictionary;
            if(dictionary != null)
            {
                _header.Content = $"{DataContext.GetKey().ToString()}";
            }

            if(_treeView != null) _treeView.DataContext = DataContext;
        }
    }
}
