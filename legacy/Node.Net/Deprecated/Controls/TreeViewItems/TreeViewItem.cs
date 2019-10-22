using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Controls.TreeViewItems
{
    class TreeViewItem : System.Windows.Controls.TreeViewItem , IHasFactory
    {
        public TreeViewItem()
        {
            DataContextChanged += _DataContextChanged;
        }

        public IFactory Factory { get; set; }
        private IFactory GetFactory()
        {
            return Factory == null ? Node.Net.Controls.Factory.Default : Factory;
        }
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        private Dictionary<object, System.Windows.Controls.TreeViewItem> treeViewItems = new Dictionary<object, System.Windows.Controls.TreeViewItem>();
        private void Update()
        {
            Items.Clear();
            if (DataContext != null)
            {
                var key = DataContext.GetKey();
                var value = DataContext.GetValue();
                if (value == null) Header = $"{DataContext.GetKey()} null";
                else if (value.GetType().IsValueType) Header = $"{DataContext.GetKey()} {value.ToString()}";
                else if (value.GetType() == typeof(string)) Header = $"{DataContext.GetKey()} {value.ToString()}";
                else {
                    Header = DataContext.GetKey();
                    UpdateChildren();
                }
            }
        }

        private void UpdateChildren()
        {
            Items.Clear();
            var dictionary = DataContext.GetValue() as IDictionary;
            if (dictionary != null)
            {
                foreach (var item in dictionary)
                {
                    System.Windows.Controls.TreeViewItem tvi = null;
                    if (treeViewItems.ContainsKey(item)) tvi = treeViewItems[item];
                    else
                    {
                        tvi = Activator.CreateInstance(GetType()) as System.Windows.Controls.TreeViewItem; 
                        tvi.DataContext = item;
                        /*
                        tvi = GetFactory().Create<System.Windows.Controls.TreeViewItem>(item) as System.Windows.Controls.TreeViewItem;
                        var hasFactory = tvi as IHasFactory;
                        if (hasFactory != null) hasFactory.Factory = GetFactory();
                        tvi.DataContext = item;
                        */
                        if (tvi != null) treeViewItems[item] = tvi;
                    }
                    if (tvi != null && !Items.Contains(tvi)) Items.Add(tvi);
                }
            }
        }
    
    }
}
