using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.View
{
    /// <summary>
    /// The ExplorerView has a TreeView,PropertyView, and DynamicView (with 0-n FrameworkElement children)
    /// </summary>
    public class ExplorerView : Grid
    {
        private SplitView horizontalSplitView;
        private SplitView verticalSplitView;
        private System.Windows.Controls.TreeView treeView;// new View.TreeView();
        private readonly Deprecated.Controls.PropertyView propertyView = new Deprecated.Controls.PropertyView();
        private FrameworkElement contentView;

        public ExplorerView()
        {
            DataContextChanged += ExplorerView_DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            verticalSplitView = new SplitView { Orientation = Orientation.Vertical };
            horizontalSplitView = new SplitView
            {
                Orientation = Orientation.Horizontal,
                ElementA = verticalSplitView,
                ElementASize = 300
            };


            if(ReferenceEquals(null, treeView))
            {
                TreeView = new Deprecated.Controls.TreeView();
            }
        }
        private void ExplorerView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {

            if (ReferenceEquals(null, verticalSplitView)) return;

            Children.Clear();
            Children.Add(horizontalSplitView);
            verticalSplitView.ElementA = TreeView;
            verticalSplitView.ElementB = propertyView;
            TreeView.DataContext = DataContext;
            if(!ReferenceEquals(null, contentView))
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
                if (!ReferenceEquals(treeView, value))
                {
                    if(!ReferenceEquals(null,treeView))
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
