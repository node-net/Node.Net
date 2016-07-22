using System.Windows;
namespace Node.Net.View
{
    public class TabControl : System.Windows.Controls.TabControl
    {
        public TabControl()
        {
            this.DataContextChanged += TabControl_DataContextChanged;
        }

        void TabControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            update();
        }

        private void update()
        {
            foreach(FrameworkElement element in Items)
            {
                element.DataContext = DataContext;
            }
            /*
            Items.Clear();
            if (!object.ReferenceEquals(null, DataContext))
            {
                System.Collections.IEnumerable enumerable = DataContext as System.Collections.IEnumerable;
                if (!object.ReferenceEquals(null, enumerable) && DataContext.GetType() != typeof(string))
                {
                    foreach (object item in enumerable)
                    {
                        if (KeyValuePair.IsKeyValuePair(item))
                        {
                            System.Windows.Controls.TabItem tabItem = new System.Windows.Controls.TabItem();
                            tabItem.Header = KeyValuePair.GetKey(item);
                            tabItem.Content = KeyValuePair.GetValue(item);
                            Items.Add(tabItem);
                        }
                    }
                }
            }*/
        }
    }
}