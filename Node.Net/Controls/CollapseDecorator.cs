using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public class CollapseDecorator : Grid
    {
        public CollapseDecorator()
        {
            ExpandButton = new Button { Content = "+" ,Width=30};
            CollapseButton = new Button { Content = "-",Width=30, Visibility=Visibility.Collapsed };
            Header = new Label { Content = "Header" };
        }

        public FrameworkElement Header { get; set; }
        public Button ExpandButton { get; set; }
        public Button CollapseButton { get; set; }
        public FrameworkElement Child { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //Background = Brushes.DarkGray;

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(0) });

            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition());

            Children.Add(ExpandButton);
            Children.Add(CollapseButton);

            Children.Add(Header);
            Grid.SetColumn(Header, 1);

            var label = new Label { Background = Brushes.DarkGray };
            Children.Add(label);
            Grid.SetRow(label, 1);

            ExpandButton.Click += ExpandButton_Click;
            CollapseButton.Click += CollapseButton_Click;

            Children.Add(Child);
            Grid.SetRow(Child, 1);
            Grid.SetColumn(Child, 1);
        }

        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseButton.Visibility = Visibility.Collapsed;
            ExpandButton.Visibility = Visibility.Visible;
            RowDefinitions[1].Height = new GridLength(0);
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandButton.Visibility = Visibility.Collapsed;
            CollapseButton.Visibility = Visibility.Visible;
            RowDefinitions[1].Height = GridLength.Auto;
        }
    }
}
