namespace Node.Net.View
{
    public class PropertiesExplorer : SplitterGrid
    {
        private System.Windows.Controls.TreeView treeView = null;
        private System.Windows.FrameworkElement selectionView = null;
        public PropertiesExplorer()
        {
            treeView = new Controls.TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = new Controls.PropertyControl();
            DataContextChanged += Explorer_DataContextChanged;
        }

        public System.Windows.Controls.TreeView TreeView => treeView;
        public PropertiesExplorer(System.Windows.Controls.TreeView tree_view, System.Windows.FrameworkElement selection_view, System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Horizontal)
        {
            Orientation = orientation;
            treeView = tree_view;
            if (object.ReferenceEquals(null, treeView)) treeView = new Controls.TreeView();
            treeView.SelectedItemChanged +=treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Controls.PropertyControl();
            DataContextChanged += Explorer_DataContextChanged;
        }

        void Explorer_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        public override void Update()
        {
            base.Update();
            treeView.DataContext = DataContext;
            selectionView.DataContext = DataContext;
        }

        void treeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var tvi = treeView.SelectedItem as System.Windows.Controls.TreeViewItem;
            if (!object.ReferenceEquals(null,tvi))
            {
                selectionView.DataContext = tvi.DataContext;
            }
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            System.Windows.FrameworkElement[] elements = { treeView, selectionView };
            Elements = elements;
        }
    }
}
