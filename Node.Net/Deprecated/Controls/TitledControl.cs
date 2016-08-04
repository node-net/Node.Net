using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls
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
        protected override void OnDataContextChanged()
        {
            Children.Clear();
            Children.Add(new Label
            {
                Background = Brushes.LightGray,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = Node.Net.Collections.KeyValuePair.GetKey(DataContext)
            });

            var element = (UIElement)Node.Net.Collections.KeyValuePair.GetValue(DataContext);
            if (!ReferenceEquals(null, element))
            {
                Children.Add(element);
                Grid.SetRow(element, 1);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());
        }
    }
}
