using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Collections.Controls
{
    public class DictionaryExplorer : Grid
    {
        public DictionaryExplorer()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        public string Key
        {
            get { return instanceStackPanel.Key; }
            set { instanceStackPanel.Key = value; }
        }
        private bool initialized = false;
        Label keyLabel = null;
        InstanceStackPanel instanceStackPanel = new InstanceStackPanel();
        InstanceStackPanel parentInstanceStackPanel = null;
        Properties properties = null;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            initialized = true;
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
            ColumnDefinitions.Add(new ColumnDefinition());

            keyLabel = new Label { Background = Brushes.LightGray };
            Children.Add(keyLabel);
            Grid.SetColumnSpan(keyLabel, 3);

            Children.Add(instanceStackPanel);
            Grid.SetRow(instanceStackPanel, 1);
            instanceStackPanel.SelectionChanged += InstanceStackPanel_SelectionChanged;

            var splitter = new GridSplitter { Width = 5, HorizontalAlignment = HorizontalAlignment.Stretch };
            Children.Add(splitter);
            Grid.SetColumn(splitter, 1);
            Grid.SetRowSpan(splitter, 2);

            var selectionStackPanel = new StackPanel { Orientation = Orientation.Vertical };
            var scrollViewer = new System.Windows.Controls.ScrollViewer
            {
                Content = selectionStackPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            parentInstanceStackPanel = new InstanceStackPanel { DataContext = null, Deep = false };
            var parentsExpander = new System.Windows.Controls.Expander
            {
                Header = "Parents",
                Content = parentInstanceStackPanel,
                IsExpanded = true
            };
            selectionStackPanel.Children.Add(parentsExpander);

            properties = new Properties { DataContext = null };
            var propertiesExpander = new System.Windows.Controls.Expander
            {
                Header = "Properties",
                Content = properties,
                IsExpanded = true
            };
            selectionStackPanel.Children.Add(propertiesExpander);

            Children.Add(scrollViewer);
            Grid.SetColumn(scrollViewer, 2);
            Grid.SetRow(scrollViewer, 1);
            Update();
        }

        private void InstanceStackPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = instanceStackPanel.SelectedItem as FrameworkElement;
            if (item != null)
            {
                var dictionary = KeyValuePair.GetValue(item.DataContext);
                if (dictionary != null)
                {
                    var ancestors = ObjectExtension.GetAncestors(dictionary);
                    parentInstanceStackPanel.DataContext = ancestors;
                    properties.DataContext = dictionary;
                }
            }
            else
            {
                parentInstanceStackPanel.DataContext = null;
            }
        }

        private void Update()
        {
            if (!initialized) return;
            if (DataContext == null) keyLabel.Content = "";
            else
            {
                int count = 0;
                var d = KeyValuePair.GetValue(DataContext) as IDictionary;
                if (d != null) count = IDictionaryExtension.DeepCount<IDictionary>(d);
                keyLabel.Content = $"{KeyValuePair.GetKey(DataContext)} {count}";
            }
            instanceStackPanel.DataContext = DataContext;
            parentInstanceStackPanel.Key = instanceStackPanel.Key;
        }
    }
}
