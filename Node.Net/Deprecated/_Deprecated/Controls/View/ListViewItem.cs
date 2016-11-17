namespace Node.Net.View
{
    public class ListViewItem : System.Windows.Controls.ListViewItem
    {
        public ListViewItem(object value)
        {
            DataContext = value;
            DataContextChanged += ListViewItem_DataContextChanged;
            update();
        }

        void ListViewItem_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            update();
        }

        public event System.EventHandler Update;
        private void update()
        {
            if (Update!=null)
            {
                Update(this, new System.EventArgs());
            }
            else
            {
                if(ReferenceEquals(null,DataContext))
                {
                    Content = DataContext;
                }
                else{
                    var dictionary = KeyValuePair.GetValue(DataContext) as System.Collections.IDictionary;
                    if (!ReferenceEquals(null, dictionary))
                    {
                        var sp = new System.Windows.Controls.StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal };
                        foreach (object key in dictionary.Keys)
                        {

                            sp.Children.Add(new System.Windows.Controls.Label{Content = dictionary[key].ToString()});

                        }
                        Content = sp;
                    }
                    else
                    {
                        var keyProperty = DataContext.GetType().GetProperty("Key");
                        Content = !ReferenceEquals(null, keyProperty) ? keyProperty.GetValue(DataContext, null).ToString() : DataContext;
                    }
                }

            }
        }
    }
}
