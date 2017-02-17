using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Node.Net.Controls
{
    public sealed class Splitter : Grid
    {
        private Orientation orientation = Orientation.Horizontal;
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    Update();
                }
            }
        }
        private FrameworkElement childA = null;
        private FrameworkElement childB = null;
        public FrameworkElement ChildA
        {
            get { return childA; }
            set { childA = value; Update(); }
        }
        public FrameworkElement ChildB
        {
            get { return childB; }
            set { childB = value; Update(); }
        }

        public GridLength GridLengthA { get; set; } = GridLength.Auto;
        public GridLength GridLengthB { get; set; } = GridLength.Auto;

        private Frame frameA = new Frame { JournalOwnership = JournalOwnership.UsesParentJournal };
        private GridSplitter gridSplitter = new GridSplitter();
        private Frame frameB = new Frame { JournalOwnership = JournalOwnership.UsesParentJournal };
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Children.Add(frameA);
            Children.Add(gridSplitter);
            Children.Add(frameB);
            Update();
        }
        private void Update()
        {
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            if (Orientation == Orientation.Horizontal)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = GridLengthA });
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
                ColumnDefinitions.Add(new ColumnDefinition { Width = GridLengthB });
                if (childA != null)
                {
                    frameA.Content = childA;
                }
                gridSplitter.Width = 5;
                gridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                Grid.SetColumn(gridSplitter, 1);
                if (childB != null)
                {
                    frameB.Content = childB;
                }
                Grid.SetColumn(frameB, 2);

            }
            else
            {
                RowDefinitions.Add(new RowDefinition { Height = GridLengthA });
                RowDefinitions.Add(new RowDefinition { Height = new GridLength(5) });
                RowDefinitions.Add(new RowDefinition { Height = GridLengthB });
                if (childA != null)
                {
                    frameA.Content = childA;
                }
                gridSplitter.Height = 5;
                gridSplitter.Width = Double.NaN;
                gridSplitter.HorizontalAlignment = HorizontalAlignment.Center;
                gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
                Grid.SetColumn(gridSplitter, 0);
                Grid.SetRow(gridSplitter, 1);
                if (childB != null)
                {
                    frameB.Content = childB;
                }
                Grid.SetColumn(frameB, 0);
                Grid.SetRow(frameB, 2);
            }
        }
    }
}
