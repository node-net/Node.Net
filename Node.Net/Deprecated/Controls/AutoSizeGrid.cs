using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Controls
{
    public class AutoSizeGrid : Grid
    {
        public AutoSizeGrid()
        {
            DataContextChanged += AutoSizeGrid_DataContextChanged;
        }

        private void AutoSizeGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (element != null) element.DataContext = DataContext;
        }

        private FrameworkElement element = null;
        public System.Windows.FrameworkElement Element
        {
            get { return element; }
            set {
                element = value;
                frame.Content = element;
            }
        }
        private Frame frame = new Frame { JournalOwnership = System.Windows.Navigation.JournalOwnership.UsesParentJournal };
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
            RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
            ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            Children.Add(frame);
            Grid.SetColumn(frame, 1);
            Grid.SetRow(frame, 1);
        }
    }
}
