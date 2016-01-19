using System;
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
        private System.Windows.Controls.TreeView treeView = new View.TreeView();
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
                ElementA = verticalSplitView
            };
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
            set { treeView = value; OnDataContextChanged(); }
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
    }
}
