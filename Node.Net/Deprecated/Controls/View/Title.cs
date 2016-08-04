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
            if (!ReferenceEquals(null, KeyValuePair.GetValue(DataContext)))
            {
                var label = new System.Windows.Controls.Label
                {
                    Content = KeyValuePair.GetValue(DataContext).GetType().Name
                };
                Content = label;
            }
        }
    }
}
