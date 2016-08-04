using System.Windows;

namespace Node.Net.Deprecated.Controls
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

        protected override void OnDataContextChanged()
        {
            Children.Clear();
            Children.Add(propertyControl);
            if (!ReferenceEquals(null, PropertyControl))
            {
                PropertyControl.DataContext = DataContext;
            }
        }
    }
}
