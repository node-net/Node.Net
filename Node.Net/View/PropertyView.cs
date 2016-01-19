using System.Windows;
using System.Windows.Controls;

namespace Node.Net.View
{
    public class PropertyView : Grid
    {
        private FrameworkElement propertyControl = new Properties();
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
            DataContextChanged += PropertyView_DataContextChanged;
        }

        private void PropertyView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {
            Children.Clear();
            Children.Add(propertyControl);
            if(!object.ReferenceEquals(null,PropertyControl))
            {
                PropertyControl.DataContext = DataContext;
            }
        }
    }
}
