namespace Node.Net.View
{
    public class Explorer : SplitterGrid
    {
        private readonly System.Windows.Controls.TreeView treeView;
        private System.Windows.FrameworkElement selectionViewVertical;
        private readonly System.Windows.FrameworkElement selectionView;
        public Explorer()
        {
            treeView = new Deprecated.Controls.TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = new Deprecated.Controls.PropertyControl();
            DataContextChanged += Explorer_DataContextChanged;
        }
        public Explorer(System.Windows.FrameworkElement selection_view)
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal;
            if (object.ReferenceEquals(null, treeView)) treeView = new Deprecated.Controls.TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Deprecated.Controls.PropertyControl();
            DataContextChanged += Explorer_DataContextChanged;
        }

        public Explorer(System.Windows.FrameworkElement selection_view, System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Horizontal)
        {
            Orientation = orientation;
            if (object.ReferenceEquals(null, treeView)) treeView = new Deprecated.Controls.TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Deprecated.Controls.PropertyControl();
            DataContextChanged += Explorer_DataContextChanged;
        }
        
        public Explorer(System.Windows.Controls.TreeView tree_view,System.Windows.FrameworkElement selection_view,System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Horizontal)
        {
            Orientation = orientation;
            treeView = tree_view;
            if (object.ReferenceEquals(null, treeView)) treeView = new Deprecated.Controls.TreeView();
            treeView.SelectedItemChanged +=treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Deprecated.Controls.PropertyControl();
            DataContextChanged += Explorer_DataContextChanged;
        }

        public Explorer(System.Windows.Controls.TreeView tree_view, System.Windows.FrameworkElement selection_view_vertical,System.Windows.FrameworkElement selection_view, System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Horizontal)
        {
            Orientation = orientation;
            treeView = tree_view;
            selectionViewVertical = selection_view_vertical;
            if (object.ReferenceEquals(null, treeView)) treeView = new Deprecated.Controls.TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Deprecated.Controls.PropertyControl();
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

            if(object.ReferenceEquals(null,selectionViewVertical))
            {
                selectionViewVertical = new Deprecated.Controls.PropertyControl();

                var valueChangedEvent = selectionViewVertical.GetType().GetEvent("ValueChanged");
                if (!object.ReferenceEquals(null, valueChangedEvent))
                {
                    var handlerInfo = GetType().GetMethod(nameof(properties_ValueChanged));
                    var handler =
                        System.Delegate.CreateDelegate(valueChangedEvent.EventHandlerType,this,handlerInfo);
                    valueChangedEvent.AddEventHandler(selectionViewVertical, handler);
                }
            }
            
            var propertiesExplorer = new PropertiesExplorer(treeView,selectionViewVertical,System.Windows.Controls.Orientation.Vertical);
            System.Windows.FrameworkElement[] elements = { propertiesExplorer, selectionView };
            Elements = elements;
            gridLengths.Add(new System.Windows.GridLength(200));
        }

        public void properties_ValueChanged(object sender, System.EventArgs e)
        {
            selectionView.DataContext = null;
            selectionView.DataContext = KeyValuePair.GetValue(selectionViewVertical.DataContext);
        }
    }
}
