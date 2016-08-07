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

        
        //private Button _header = new Button { Background = Brushes.LightGray };
        private TreeView _treeView;
        private Properties _properties;
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //_header = new Button { Background = Brushes.LightGray, HorizontalAlignment =HorizontalAlignment.Stretch , BorderBrush = Brushes.Transparent, HorizontalContentAlignment = HorizontalAlignment.Left};
            //_header.Click += _header_Click;
            _treeView = new TreeView();
            _treeView.SelectedItemChanged += _treeView_SelectedItemChanged;
            _properties = new Controls.Properties();

            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
            ColumnDefinitions.Add(new ColumnDefinition());

            Children.Add(_treeView);

            var splitter = new GridSplitter { HorizontalAlignment = HorizontalAlignment.Stretch, Width = 5 };
            Children.Add(splitter);
            Grid.SetColumn(splitter, 1);

            Children.Add(_properties);
            Grid.SetColumn(_properties, 2);
            /*
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Children.Add(_header);

            RowDefinitions.Add(new RowDefinition());
            var horizontalSplitterGrid = new System.Windows.Controls.Grid();
            horizontalSplitterGrid.ColumnDefinitions.Add(new ColumnDefinition());
            horizontalSplitterGrid.ColumnDefinitions.Add(new ColumnDefinition());
            Children.Add(horizontalSplitterGrid);
            Grid.SetRow(horizontalSplitterGrid, 1);
            _childrenGrid = new ChildrenGrid();
            horizontalSplitterGrid.Children.Add(_childrenGrid);
            Grid.SetColumn(_childrenGrid, 1);

            var verticalSplitterGrid = new System.Windows.Controls.Grid();
            verticalSplitterGrid.RowDefinitions.Add(new RowDefinition());
            verticalSplitterGrid.RowDefinitions.Add(new RowDefinition());
            
            verticalSplitterGrid.Children.Add(_treeView);
            
            verticalSplitterGrid.Children.Add(_valuesGrid);
            Grid.SetRow(_valuesGrid, 1);

            horizontalSplitterGrid.Children.Add(verticalSplitterGrid);
            */
            Update();
        }
        /*
        private void _header_Click(object sender, RoutedEventArgs e)
        {
            SetSelection((_treeView == null) ? null:_treeView.DataContext);

            //if (_valuesGrid != null && _treeView != null) _valuesGrid.DataContext = _treeView.DataContext;
        }*/

        private void _treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            object dataContext = null;
            if(_treeView != null)
            {
                var tvi = _treeView.SelectedItem as TreeViewItem;
                if (tvi != null)
                {
                    dataContext = tvi.DataContext;
                }
            }
            SetSelection(dataContext);
            //if (_valuesGrid != null) _valuesGrid.DataContext = dataContext;
        }

        private void SetSelection(object value)
        {
            if (_properties != null) _properties.DataContext = value;
            //if (_childrenGrid != null) _childrenGrid.DataContext = value;
        }

        private void Update()
        {
            //_header.Content = null;
            
            if (DataContext == null) return;
            
            /*
            var dictionary = DataContext.GetValue() as IDictionary;
            if(dictionary != null)
            {
                _header.Content = $"{DataContext.GetKey().ToString()}";
            }*/

            if(_treeView != null) _treeView.DataContext = DataContext;
            //if (proeprties != null) _properties.DataContext = DataContext;
        }
    }
}
