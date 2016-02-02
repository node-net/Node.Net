using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class PropertyView : Grid
    {
        private FrameworkElement propertyControl = new PropertyControl();
        public FrameworkElement PropertyControl
        {
            get { return propertyControl; }
            set
            {
                propertyControl = value;
                OnDataContextChanged();
            }
        }
        public PropertyView()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {
            Children.Clear();
            Children.Add(propertyControl);
            if (!object.ReferenceEquals(null, PropertyControl))
            {
                PropertyControl.DataContext = DataContext;
            }
        }
    }
}
