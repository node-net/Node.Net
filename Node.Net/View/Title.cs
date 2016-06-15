namespace Node.Net.View
{
    public class Title : System.Windows.Controls.UserControl
    {
        public Title()
        {
            DataContextChanged += Title_DataContextChanged;
        }

        void Title_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (!object.ReferenceEquals(null, KeyValuePair.GetValue(DataContext)))
            {
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label.Content = KeyValuePair.GetValue(DataContext).GetType().Name;
                Content = label;
            }
        }
    }
}
