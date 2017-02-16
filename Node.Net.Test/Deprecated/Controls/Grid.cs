using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class Grid : System.Windows.Controls.Grid
    {
        public Grid()
        {
            DataContextChanged += Grid_DataContextChanged;
        }

        private void Grid_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {
            foreach(FrameworkElement element in Children)
            {
                element.DataContext = DataContext;
            }
        }
    }
}
