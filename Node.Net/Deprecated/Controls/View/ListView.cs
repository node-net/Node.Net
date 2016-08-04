namespace Node.Net.View
{
    public class ListView : System.Windows.Controls.ListView
    {
        public ListView()
        {
            this.DataContextChanged += ListView_DataContextChanged;
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);
            update();
        }
        public void Refresh() { update(); }
        public delegate void UpdateListViewItemHandler(System.Windows.Controls.ListViewItem item);
        public event UpdateListViewItemHandler UpdateListViewItem;
        void ListView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            update();
        }
        private void update()
        {
            Items.Clear();
            if (!ReferenceEquals(null, DataContext))
            {
                var enumerable = DataContext as System.Collections.IEnumerable;
                if (!ReferenceEquals(null,enumerable) && DataContext.GetType() != typeof(string))
                {
                    foreach(object item in enumerable)
                    {

                        if (!ReferenceEquals(null, UpdateListViewItem))
                        {
                            var lvi = new ListViewItem(null);
                            lvi.Update += lvi_Update;
                            lvi.DataContext = item;
                            Items.Add(lvi);
                        }
                        else
                        {
                            Items.Add(new ListViewItem(item));
                        }
                    }
                }
            }
        }

        void lvi_Update(object sender, System.EventArgs e)
        {
            var lvi = sender as System.Windows.Controls.ListViewItem;
            if (!ReferenceEquals(null,lvi) )
            {
                if (UpdateListViewItem != null)
                {
                    UpdateListViewItem(lvi);
                }
            }
        }
    }
}
