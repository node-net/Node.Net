
namespace Node.Net.Controls
{
    public class TreeViewItem : System.Windows.Controls.TreeViewItem
    {
        private string headerString = "";

        public TreeViewItem()
        {
            DataContextChanged += TreeViewItem_DataContextChanged;
            MethodInfoCommand.Default.PostMethodInvoke += Default_PostMethodInvoke;
        }

        private void TreeViewItem_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update(1);
        }

        public TreeViewItem(object model)
        {
            DataContext = model;
            Update(1);
            MethodInfoCommand.Default.PostMethodInvoke += Default_PostMethodInvoke;
        }
        public TreeViewItem(object model, int childDepth = 1)
        {
            DataContext = model;
            Update(childDepth);
            MethodInfoCommand.Default.PostMethodInvoke += Default_PostMethodInvoke;
        }


        public TreeViewItem(string header, object model, int childDepth = 1)
        {
            headerString = header;
            DataContext = model;
            Update(childDepth);
            MethodInfoCommand.Default.PostMethodInvoke += Default_PostMethodInvoke;
        }

        void Default_PostMethodInvoke(object sender, System.EventArgs e)
        {
            if (IsExpanded) { Update(); }
        }

        protected override void OnExpanded(System.Windows.RoutedEventArgs e)
        {
            Update(2);
            base.OnExpanded(e);
        }
        protected override void OnContextMenuOpening(System.Windows.Controls.ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);
            ContextMenu = new ContextMenu(DataContext);
        }
        protected override void OnContextMenuClosing(System.Windows.Controls.ContextMenuEventArgs e)
        {
            base.OnContextMenuClosing(e);
        }
        private void Update(int childDepth = 1)
        {
            Header = GetHeader();
            System.Collections.IList children = GetChildren();
            if (children.Count == 0) Items.Clear();
            else
            {
                Items.Clear();
                if (childDepth == 0)
                {
                    Items.Add(new System.Windows.Controls.TreeViewItem() { Header = "dummy" });
                }
                else
                {
                    foreach (object item in children)
                    {
                        object[] args = { item, childDepth - 1 };
                        TreeViewItem tvi = (TreeViewItem)System.Activator.CreateInstance(GetType(), args);
                        Items.Add(tvi);
                    }
                }
            }
        }

        public virtual System.Collections.IList GetChildren() => GetChildren(DataContext);

        protected virtual object GetHeader()
        {
            if (object.ReferenceEquals(null, DataContext)) return "null";
            if (headerString.Length > 0) return headerString;
            if (DataContext.GetType() == typeof(System.Collections.Generic.KeyValuePair<string, dynamic>))
            {
                System.Collections.Generic.KeyValuePair<string, dynamic> kvp = (System.Collections.Generic.KeyValuePair<string, dynamic>)DataContext;
                if (!object.ReferenceEquals(null, kvp))
                {
                    return kvp.Key;
                }
            }
            System.Reflection.PropertyInfo nameInfo = Node.Net.View.KeyValuePair.GetValue(DataContext).GetType().GetProperty("Name");
            if (!object.ReferenceEquals(null, nameInfo))
            {
                return nameInfo.GetValue(Node.Net.View.KeyValuePair.GetValue(DataContext), null).ToString();
            }
            return DataContext.ToString();
        }

        public static bool IsValidChild(object item)
        {
            if (object.ReferenceEquals(null, item)) return false;
            object value = Collections.KeyValuePair.GetValue(item);
            if (object.ReferenceEquals(null, value)) return false;
            if (typeof(string).IsAssignableFrom(value.GetType())) return false;
            if (value.GetType().IsValueType) return false;
            return true;
        }
        public static System.Collections.IList GetChildren(object item)
        {
            object value = Collections.KeyValuePair.GetValue(item);
            System.Collections.Generic.List<object> children = new System.Collections.Generic.List<object>();
            if (object.ReferenceEquals(null, value)) return children;
            if (typeof(string).IsAssignableFrom(value.GetType())) return children;
            if (value.GetType().IsValueType) return children;
            if (!typeof(string).IsAssignableFrom(value.GetType()))
            {
                System.Collections.IEnumerable ienumerable = value as System.Collections.IEnumerable;
                if (!object.ReferenceEquals(null, ienumerable))
                {
                    foreach (object i in ienumerable)
                    {
                        if (IsValidChild(i)) { children.Add(i); }
                    }
                }
            }
            return children;
        }
    }
}
