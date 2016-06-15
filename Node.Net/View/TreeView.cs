namespace Node.Net.View
{
    public class TreeView : System.Windows.Controls.TreeView
    {
        public TreeView()
        {
            DataContextChanged += onDataContextChanged;
            Update();
        }

        public TreeView(System.Type treeViewItemType)
        {
            TreeViewItemType = treeViewItemType;
            DataContextChanged += onDataContextChanged;
            Update();
        }
        public TreeView(object value)
        {
            DataContext = value;
            DataContextChanged += onDataContextChanged;
            Update();
        }

        private System.Type treeViewItemType = typeof(TreeViewItem);
        public System.Type TreeViewItemType
        {
            get
            {
                return treeViewItemType;
            }
            set
            {
                System.Type[] types = { typeof(object)};
                var ci = value.GetConstructor(types);
                if (object.ReferenceEquals(null,ci))
                {
                    throw new System.InvalidOperationException("TreeViewItemType does not have a constructor accepting an object");
                }
                treeViewItemType = value;
                Update();
            }
        }
        void onDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        System.Windows.Controls.TreeViewItem CreateTreeViewItem(object value)
        {
            object[] args = {value};
            var tvi
                = System.Activator.CreateInstance(treeViewItemType, args)
                    as System.Windows.Controls.TreeViewItem;
            return tvi;
        }
        public virtual void Update()
        {
            Items.Clear();
            var dictionary = DataContext as System.Collections.IDictionary;
            var ienumerable = DataContext as System.Collections.IEnumerable;
            if (!object.ReferenceEquals(null,ienumerable) && object.ReferenceEquals(null,dictionary))
            {
                foreach(object item in ienumerable)
                {
                    //Items.Add(new TreeViewItem(item));
                    Items.Add(CreateTreeViewItem(item));
                }
            }
            else Items.Add(CreateTreeViewItem(DataContext));//new TreeViewItem(DataContext));
        }
    }
}
