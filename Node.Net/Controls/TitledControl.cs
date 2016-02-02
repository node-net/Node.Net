using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public class TitledControl : Grid
    {
        public TitledControl()
        {
            DataContextChanged += TitledControl_DataContextChanged;
        }

        private void TitledControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }
        protected virtual void OnDataContextChanged()
        {
            Children.Clear();
            Children.Add(new Label()
            {
                Background = Brushes.LightGray,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = Json.KeyValuePair.GetKey(DataContext)
            });

            UIElement element = (UIElement)Json.KeyValuePair.GetValue(DataContext);
            if (!object.ReferenceEquals(null, element))
            {
                Children.Add(element);
                Grid.SetRow(element, 1);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());
        }
    }
}
