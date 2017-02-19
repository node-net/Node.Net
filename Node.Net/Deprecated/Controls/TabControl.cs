namespace Node.Net.Controls
{
    public class TabControl : System.Windows.Controls.TabControl
    {
        public TabControl()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            Items.Clear();
            //var dictionary = KeyValuePa
        }
    }
}
