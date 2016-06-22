using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class CollapseDecorator2 : StackPanel
    {
        public CollapseDecorator2()
        {
            ExpandButton = new Button { Content = "+", Width = 30 };
            CollapseButton = new Button { Content = "-", Width = 30, Visibility = Visibility.Collapsed };
            Header = new Label { Content = "Header" };
            DataContextChanged += _DataContextChanged;
        }
        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
        private ScrollViewer _scrollViewer;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var horzPanel = new StackPanel { Orientation = Orientation.Horizontal };
            horzPanel.Children.Add(ExpandButton);
            horzPanel.Children.Add(CollapseButton);
            horzPanel.Children.Add(Header);
            ExpandButton.Visibility = Visibility.Visible;
            CollapseButton.Visibility = Visibility.Hidden;

            ExpandButton.Click += ExpandButton_Click;
            CollapseButton.Click += CollapseButton_Click;

            Children.Add(horzPanel);

            /*
            _scrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            Children.Add(_scrollViewer);
            */
            _frame = new Frame() { JournalOwnership = System.Windows.Navigation.JournalOwnership.UsesParentJournal };
            Children.Add(_frame);

            //_frame.Content = _scrollViewer;
            //_scrollViewer.Content = _frame;
            //Grid.SetRow(_frame, 1);
            //Grid.SetColumn(_frame, 1);
        }
        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseButton.Visibility = Visibility.Collapsed;
            ExpandButton.Visibility = Visibility.Visible;
            //RowDefinitions[1].Height = new GridLength(0);
            //RowDefinitions[1].Height = GridLength.Auto;
            _frame.Content = null;
            //_scrollViewer.Content = null;
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandButton.Visibility = Visibility.Collapsed;
            CollapseButton.Visibility = Visibility.Visible;
            //RowDefinitions[1].Height = GridLength.Auto;
            //_frame.Content = Child;
            //_scrollViewer.Content = Child;
            _frame.Content = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = Child
            };
        }
    }
}
