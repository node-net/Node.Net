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
        private SplitView horizontalSplitView = null;
        private SplitView verticalSplitView = null;
        private System.Windows.Controls.TreeView treeView = null;// new View.TreeView();
        private View.PropertyView propertyView = new PropertyView();
        private FrameworkElement contentView = null;

        public ExplorerView()
        {
            DataContextChanged += ExplorerView_DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            verticalSplitView = new SplitView() { Orientation = Orientation.Vertical };
            horizontalSplitView = new SplitView()
            {
                Orientation = Orientation.Horizontal,
                ElementA = verticalSplitView,
                ElementASize = 300
            };


            if(object.ReferenceEquals(null, treeView))
            {
                TreeView = new Controls.TreeView();
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
            if(!object.ReferenceEquals(null, contentView))
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
                    if(!object.ReferenceEquals(null,treeView))
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
            if(!object.ReferenceEquals(null, tvi))
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
