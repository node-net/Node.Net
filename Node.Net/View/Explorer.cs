namespace Node.Net.View
{
    public class Explorer : SplitterGrid
    {
        private System.Windows.Controls.TreeView treeView = null;
        private System.Windows.FrameworkElement selectionViewVertical = null;
        private System.Windows.FrameworkElement selectionView = null;
        public Explorer()
        {
            treeView = new TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = new Properties();
            DataContextChanged += Explorer_DataContextChanged;
        }
        public Explorer(System.Windows.FrameworkElement selection_view)
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal;
            if (object.ReferenceEquals(null, treeView)) treeView = new TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Properties();
            DataContextChanged += Explorer_DataContextChanged;
        }

        public Explorer(System.Windows.FrameworkElement selection_view, System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Horizontal)
        {
            Orientation = orientation;
            if (object.ReferenceEquals(null, treeView)) treeView = new TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Properties();
            DataContextChanged += Explorer_DataContextChanged;
        }
        
        public Explorer(System.Windows.Controls.TreeView tree_view,System.Windows.FrameworkElement selection_view,System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Horizontal)
        {
            Orientation = orientation;
            treeView = tree_view;
            if (object.ReferenceEquals(null, treeView)) treeView = new TreeView();
            treeView.SelectedItemChanged +=treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Properties();
            DataContextChanged += Explorer_DataContextChanged;
        }

        public Explorer(System.Windows.Controls.TreeView tree_view, System.Windows.FrameworkElement selection_view_vertical,System.Windows.FrameworkElement selection_view, System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Horizontal)
        {
            Orientation = orientation;
            treeView = tree_view;
            selectionViewVertical = selection_view_vertical;
            if (object.ReferenceEquals(null, treeView)) treeView = new TreeView();
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;
            selectionView = selection_view;
            if (object.ReferenceEquals(null, selectionView)) selectionView = new Properties();
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
            System.Windows.Controls.TreeViewItem tvi = treeView.SelectedItem as System.Windows.Controls.TreeViewItem;
            if(!object.ReferenceEquals(null,tvi))
            {
                selectionView.DataContext = tvi.DataContext;
            }
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            if(object.ReferenceEquals(null,selectionViewVertical))
            {
                selectionViewVertical = new Properties();

                System.Reflection.EventInfo valueChangedEvent = selectionViewVertical.GetType().GetEvent("ValueChanged");
                if (!object.ReferenceEquals(null, valueChangedEvent))
                {
                    System.Reflection.MethodInfo handlerInfo = GetType().GetMethod("properties_ValueChanged");
                    System.Delegate handler =
                        System.Delegate.CreateDelegate(valueChangedEvent.EventHandlerType,this,handlerInfo);
                    valueChangedEvent.AddEventHandler(selectionViewVertical, handler);
                }
            }
            
            PropertiesExplorer propertiesExplorer = new PropertiesExplorer(treeView,selectionViewVertical,System.Windows.Controls.Orientation.Vertical);
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
