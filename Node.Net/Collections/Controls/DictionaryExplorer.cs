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

        public string Key
        {
            get { return instances.Key; }
            set { instances.Key = value; }
        }
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private bool initialized = false;
        InstanceCounts instances = new InstanceCounts { Key = "Type" };
        Properties properties = null;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            initialized = true;
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
            ColumnDefinitions.Add(new ColumnDefinition());

            Children.Add(instances);
            instances.SelectionChanged += Instances_SelectionChanged;

            var splitter = new GridSplitter { Width = 5, HorizontalAlignment = HorizontalAlignment.Stretch };
            Children.Add(splitter);
            Grid.SetColumn(splitter, 1);
            Grid.SetRowSpan(splitter, 2);

            properties = new Properties { DataContext = null };
            var scrollViewer = new ScrollViewer {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = properties
            };
            Children.Add(scrollViewer);
            Grid.SetColumn(scrollViewer, 2);
            Update();
        }

        private void Instances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as System.Windows.Controls.ListBox;
            if(listBox != null)
            {
                var listBoxItem = listBox.SelectedItem as ListBoxItem;
                if(listBoxItem != null)
                {
                    properties.DataContext = listBoxItem.DataContext;
                }
            }
        }

        private void Update()
        {
            if (!initialized) return;
        }
    }
}
