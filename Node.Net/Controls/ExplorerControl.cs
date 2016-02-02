using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class ExplorerControl : Grid
    {
        private SplitControl horizontalSplitView = null;
        private SplitControl verticalSplitView = null;
        private System.Windows.Controls.TreeView treeView = null;// new View.TreeView();
        private PropertyView propertyView = new PropertyView();
        private FrameworkElement contentView = null;

        public ExplorerControl()
        {
            DataContextChanged += ExplorerView_DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            verticalSplitView = new SplitControl() { Orientation = Orientation.Vertical };
            horizontalSplitView = new SplitControl()
            {
                Orientation = Orientation.Horizontal,
                ElementA = verticalSplitView,
                ElementASize = 300
            };


            if (object.ReferenceEquals(null, treeView))
            {
                TreeView = new View.TreeView();
            }
        }
        private void ExplorerView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {

            if (object.ReferenceEquals(null, verticalSplitView)) return;

            Children.Clear();
            Children.Add(horizontalSplitView);
            verticalSplitView.ElementA = TreeView;
            verticalSplitView.ElementB = propertyView;
            TreeView.DataContext = DataContext;
            if (!object.ReferenceEquals(null, contentView))
            {
                horizontalSplitView.ElementB = contentView;
                contentView.DataContext = DataContext;
            }
        }


        public System.Windows.Controls.TreeView TreeView
        {
            get { return treeView; }
            set
            {
                if (!object.ReferenceEquals(treeView, value))
                {
                    if (!object.ReferenceEquals(null, treeView))
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
            TreeViewItem tvi = treeView.SelectedItem as TreeViewItem;
            if (!object.ReferenceEquals(null, tvi))
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
            if (!object.ReferenceEquals(null, ContentView))
            {
                MethodInfo method = ContentView.GetType().GetMethod("GetViewMenuItem");
                if (!object.ReferenceEquals(null, method))
                {
                    viewMenuItem = method.Invoke(ContentView, null) as MenuItem;
                }
            }

            return viewMenuItem;
        }
    }
}
