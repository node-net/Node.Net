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
            if (!object.ReferenceEquals(null, Update))
            {
                Update(this, new System.EventArgs());
            }
            else
            {
                if(object.ReferenceEquals(null,DataContext))
                {
                    Content = DataContext;
                }
                else{
                    System.Collections.IDictionary dictionary = KeyValuePair.GetValue(DataContext) as System.Collections.IDictionary;
                    if (!object.ReferenceEquals(null, dictionary))
                    {
                        System.Windows.Controls.StackPanel sp = new System.Windows.Controls.StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal };
                        foreach(object key in dictionary.Keys)
                        {
                            
                            sp.Children.Add(new System.Windows.Controls.Label(){Content = dictionary[key].ToString()});
                            
                        }
                        Content = sp;
                    }
                    else
                    {
                        System.Reflection.PropertyInfo keyProperty = DataContext.GetType().GetProperty("Key");
                        if (!object.ReferenceEquals(null, keyProperty))
                        {
                            Content = keyProperty.GetValue(DataContext, null).ToString();
                        }
                        else { Content = DataContext; }
                    }
                }
                
            }
        }
    }
}
