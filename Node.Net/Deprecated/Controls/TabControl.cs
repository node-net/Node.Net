using System.Windows;

namespace Node.Net.Deprecated.Controls
{
    public class TabControl : System.Windows.Controls.TabControl
    {
        public TabControl()
        {
            DataContextChanged += TabControl_DataContextChanged;
        }

        private void TabControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {
            foreach(FrameworkElement element in Items)
            {
                element.DataContext = DataContext;
            }
        }
    }
}
