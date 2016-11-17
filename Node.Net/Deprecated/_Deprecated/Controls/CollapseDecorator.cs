using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls
{
    public class CollapseDecorator : Grid
    {
        public CollapseDecorator()
        {
            ExpandButton = new Button { Content = "+" ,Width=30};
            CollapseButton = new Button { Content = "-",Width=30, Visibility=Visibility.Collapsed };
            Header = new Label { Content = "Header" };
            DataContextChanged += CollapseDecorator_DataContextChanged;
        }

        private void CollapseDecorator_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Child != null)
            {
                Child.DataContext = DataContext;
            }
        }

        public FrameworkElement Header { get; set; }
        public Button ExpandButton { get; set; }
        public Button CollapseButton { get; set; }
        public FrameworkElement Child { get; set; }
        private Frame _frame;

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

            _frame = new Frame() { JournalOwnership = System.Windows.Navigation.JournalOwnership.UsesParentJournal };
            Children.Add(_frame);
            Grid.SetRow(_frame, 1);
            Grid.SetColumn(_frame, 1);
            /*
            Children.Add(Child);
            Grid.SetRow(Child, 1);
            Grid.SetColumn(Child, 1);
            */
        }

        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseButton.Visibility = Visibility.Collapsed;
            ExpandButton.Visibility = Visibility.Visible;
            //RowDefinitions[1].Height = new GridLength(0);
            RowDefinitions[1].Height = GridLength.Auto;
            _frame.Content = null;
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandButton.Visibility = Visibility.Collapsed;
            CollapseButton.Visibility = Visibility.Visible;
            RowDefinitions[1].Height = GridLength.Auto;
            _frame.Content = Child;
        }
    }
}
