using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Controls
{
    public class ExplorerControl : Grid
    {
        private SplitControl horizontalSplitView;
        private SplitControl verticalSplitView;
        private System.Windows.Controls.TreeView treeView;// new View.TreeView();
        private readonly PropertyView propertyView = new PropertyView();
        private FrameworkElement contentView;
        private INotifyPropertyChanged inotifyPropertyChanged;

        public ExplorerControl()
        {
            DataContextChanged += ExplorerView_DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            verticalSplitView = new SplitControl { Orientation = Orientation.Vertical };
            horizontalSplitView = new SplitControl
            {
                Orientation = Orientation.Horizontal,
                ElementA = verticalSplitView,
                ElementASize = 300,
                ElementB = new Controls.ReadOnlyTextBox()
            };


            if (ReferenceEquals(null, treeView))
            {
                TreeView = new Controls.TreeView();
            }
        }
        private void ExplorerView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected override void OnDataContextChanged()
        {

            if (ReferenceEquals(null, verticalSplitView)) return;
            var currentInotifyPropertyChanged = Node.Net.Collections.KeyValuePair.GetValue(DataContext) as INotifyPropertyChanged;
            if (!ReferenceEquals(null,inotifyPropertyChanged))
            {
                inotifyPropertyChanged.PropertyChanged -= _PropertyChanged;
            }
            if(!ReferenceEquals(null,currentInotifyPropertyChanged))
            {
                currentInotifyPropertyChanged.PropertyChanged += _PropertyChanged;
                inotifyPropertyChanged = currentInotifyPropertyChanged;
            }
            Children.Clear();
            Children.Add(horizontalSplitView);
            verticalSplitView.ElementA = TreeView;
            verticalSplitView.ElementB = propertyView;
            TreeView.DataContext = DataContext;
            if (!ReferenceEquals(null, contentView))
            {
                horizontalSplitView.ElementB = contentView;
                contentView.DataContext = DataContext;
            }
        }

        private void _PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        public System.Windows.Controls.TreeView TreeView
        {
            get { return treeView; }
            set
            {
                if (!ReferenceEquals(treeView, value))
                {
                    if (!ReferenceEquals(null, treeView))
                    {
                        treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
                    }
                    treeView = value;
                    treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
                    OnDataContextChanged();
                }
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tvi = treeView.SelectedItem as TreeViewItem;
            if (!ReferenceEquals(null, tvi))
            {
                PropertyControl.DataContext = tvi.DataContext;
            }
        }

        public FrameworkElement PropertyControl
        {
            get { return propertyView.PropertyControl; }
            set
            {
                propertyView.PropertyControl = value;
                OnDataContextChanged();
            }
        }

        public FrameworkElement ContentView
        {
            get { return contentView; }
            set
            {
                contentView = value;
                OnDataContextChanged();
            }
        }

        public MenuItem GetViewMenuItem()
        {
            MenuItem viewMenuItem = null;
            if (!ReferenceEquals(null, ContentView))
            {
                var method = ContentView.GetType().GetMethod(nameof(GetViewMenuItem));
                if (!ReferenceEquals(null, method))
                {
                    viewMenuItem = method.Invoke(ContentView, null) as MenuItem;
                }
            }

            return viewMenuItem;
        }
    }
}
