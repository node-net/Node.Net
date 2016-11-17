namespace Node.Net.Deprecated.Controls.TreeViewItems
{
    public class TreeViewItem : System.Windows.Controls.TreeViewItem
    {
        public TreeViewItem()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {

        }
    }
}
