namespace Node.Net.View
{
    public class ComboBoxItem : System.Windows.Controls.ComboBoxItem
    {
        public ComboBoxItem()
        {
            DataContextChanged += ComboBoxItem_DataContextChanged;
        }
        public ComboBoxItem(object dataContext)
        {
            DataContext = dataContext;
            DataContextChanged += ComboBoxItem_DataContextChanged;
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        void ComboBoxItem_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
     
        }

   

        private void Update()
        {
            if (!object.ReferenceEquals(null, DataContext))
            {
                Content = KeyValuePair.GetKey(DataContext).ToString();
            }
        }
    }
}
