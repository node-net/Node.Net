using System;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Controls
{
    public class SplitControl : Grid
    {
        public SplitControl()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        private double elementASize = -1;
        public double ElementASize
        {
            get { return elementASize; }
            set { elementASize = value; OnDataContextChanged(); }
        }
        protected override void OnDataContextChanged()
        {
            Children.Clear();
            if (!object.ReferenceEquals(null, gridSplitter))
            {
                foreach (FrameworkElement element in Children)
                {
                    if (!object.ReferenceEquals(null, element))
                    {
                        element.DataContext = DataContext;
                    }
                }

                RowDefinitions.Clear();
                ColumnDefinitions.Clear();
                switch (Orientation)
                {
                    case Orientation.Vertical:
                        {
                            if (elementASize < 0) RowDefinitions.Add(new RowDefinition());
                            else RowDefinitions.Add(new RowDefinition { Height = new GridLength(elementASize) });
                            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            RowDefinitions.Add(new RowDefinition());
                            Children.Add(ElementA);
                            gridSplitter = new GridSplitter
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Center,
                                ShowsPreview = true,
                                Height = 5
                            };
                            Children.Add(gridSplitter);
                            Grid.SetRow(gridSplitter, 1);
                            Children.Add(ElementB);
                            Grid.SetRow(ElementB, 2);
                            break;
                        }
                    default:
                        {
                            if (elementASize < 0) ColumnDefinitions.Add(new ColumnDefinition());
                            else ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(elementASize) });
                            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                            ColumnDefinitions.Add(new ColumnDefinition());
                            Children.Add(ElementA);
                            gridSplitter = new GridSplitter
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                ShowsPreview = true,
                                Width = 5
                            };
                            Children.Add(gridSplitter);
                            Grid.SetColumn(gridSplitter, 1);
                            Children.Add(ElementB);
                            Grid.SetColumn(ElementB, 2);
                            break;
                        }
                }
            }
        }

        private GridSplitter gridSplitter = null;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            gridSplitter = new GridSplitter
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                ShowsPreview = true,
                Width = 5
            };


            if (object.ReferenceEquals(null, elementA))
            {
                elementA = new Label { Content = nameof(ElementA) };
            }
            if (object.ReferenceEquals(null, elementB))
            {
                elementB = new Label { Content = nameof(ElementB) };
            }

            OnDataContextChanged();

        }
        private Orientation orientation = Orientation.Horizontal;
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; OnDataContextChanged(); }
        }

        private UIElement elementA = null;
        public UIElement ElementA
        {
            get { return elementA; }
            set { elementA = value; OnDataContextChanged(); }
        }

        private UIElement elementB = null;
        public UIElement ElementB
        {
            get { return elementB; }
            set { elementB = value; OnDataContextChanged(); }
        }
    }
}
