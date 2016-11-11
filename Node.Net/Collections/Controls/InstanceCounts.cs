using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Node.Net.Collections.Controls
{
    public class InstanceCounts : Grid
    {
        public InstanceCounts() { DataContextChanged += _DataContextChanged; }

        public string Key { get; set; } = "Type";
        public string SearchText
        {
            get { return searchTextBox.Text; }
            set { searchTextBox.Text = value; }
        }
        public bool ShowInstanceKeys { get; set; } = true;

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => Update();

        //private Label TotalLabel = new Label { Content = "0 instances" };
        private Frame controlsFrame = new Frame { JournalOwnership = JournalOwnership.UsesParentJournal };
        private ComboBox keyComboBox = new ComboBox { IsEditable = true };
        private TextBox searchTextBox = new TextBox();
        private CheckBox showInstancesCheckBox = new CheckBox { Content = "Show Instances", IsChecked = false };
        private Grid controlGrid = null;
        private ScrollViewer scrollViewer = new ScrollViewer
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());
            Children.Add(controlsFrame);
            //Children.Add(searchTextBox);
            //Grid.SetRow(searchTextBox, 1);
            Children.Add(scrollViewer);
            Grid.SetRow(scrollViewer, 1);
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            showInstancesCheckBox.Checked += ShowInstancesCheckBox_Checked;
            showInstancesCheckBox.Unchecked += ShowInstancesCheckBox_Unchecked;
            //keyComboBox.SelectionChanged += KeyComboBox_SelectionChanged;
            Update();
        }

        private void KeyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (keyComboBox.SelectedItem != null)
            {
                var key = keyComboBox.SelectedItem.ToString();
                if (key.Length > 0 && Key != key) {
                    Key = key;
                    Update();
                 }
            }
        }

        private void ShowInstancesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowInstanceKeys = false;
            Update();
        }

        private void ShowInstancesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShowInstanceKeys = true;
            Update();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (Children.Count < 1) return;
            var orderedInstances = GetOrderedInstances();
            controlsFrame.Content = GetControls(orderedInstances);
            scrollViewer.Content = GetInstanceGrid(orderedInstances);
            if (keyComboBox.Text != Key) keyComboBox.Text = Key;
            keyComboBox.ItemsSource = IDictionaryExtension.DeepCollectKeys(Node.Net.Collections.KeyValuePair.GetValue(DataContext) as IDictionary);
        }

        private FrameworkElement GetControls(List<Dictionary<string, IDictionary>> orderedInstances)
        {
            if (controlGrid == null)
            {
                controlGrid = new Grid();
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition());
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition());
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition());
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition());
                controlGrid.Children.Add(new Label { Content = "Key" });
                controlGrid.Children.Add(keyComboBox);
                Grid.SetColumn(keyComboBox, 1);
                controlGrid.Children.Add(searchTextBox);
                Grid.SetColumn(searchTextBox, 2);
                controlGrid.Children.Add(showInstancesCheckBox);
                Grid.SetColumn(showInstancesCheckBox, 3);
            }
            return controlGrid;
        }
        private Grid GetInstanceGrid(List<Dictionary<string, IDictionary>> orderedInstances)
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            foreach (var instances in orderedInstances)
            {
                var count = instances.Count;
                //total += count;
                var countLabel = new Label { Content = instances.Count.ToString() };
                var typeName = instances.First().Value[Key].ToString();
                var instanceElement = GetInstancesElement(instances);// new Label { Content = typeName };
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Children.Add(countLabel);
                grid.Children.Add(instanceElement);
                Grid.SetRow(countLabel, grid.RowDefinitions.Count);
                Grid.SetRow(instanceElement, grid.RowDefinitions.Count);
                Grid.SetColumn(instanceElement, 1);
            }
            grid.RowDefinitions.Add(new RowDefinition());
            return grid;
        }
        private FrameworkElement GetInstancesElement(Dictionary<string, IDictionary> instances)
        {
            var typeName = instances.First().Value[Key].ToString();
            if (ShowInstanceKeys)
            {
                return new Expander
                {
                    Header = typeName,
                    Content = new ListBox { DataContext = instances }
                };
            }
            else
            {
                return new Label { Content = typeName };
            }
        }

        private List<Dictionary<string, IDictionary>> GetOrderedInstances()
        {
            var dictionary = KeyValuePair.GetValue(DataContext) as IDictionary;
            if (dictionary != null)
            {
                var uniqueTypeNames = IDictionaryExtension.CollectUniqueStrings(dictionary, Key);
                var typeNames = new List<string>();
                foreach (var utn in uniqueTypeNames)
                {
                    if (utn.Contains(SearchText)) typeNames.Add(utn);
                }
                var typeInstances = new Dictionary<string, Dictionary<string, IDictionary>>();
                foreach (var typeName in typeNames)
                {
                    var instances = IDictionaryExtension.DeepCollect<IDictionary>(
                        dictionary,
                        new Node.Net.Collections.KeyValueFilter
                        {
                            Key = Key,
                            Values = { typeName },
                            ExactMatch = true
                        }.Include);
                    if (instances.Count > 0)
                    {
                        typeInstances.Add(typeName, instances);
                    }
                }

                List<Dictionary<string, IDictionary>> orderedInstances = typeInstances.OrderBy(data => data.Key)
                    .Select(data => data.Value).ToList();
                orderedInstances = orderedInstances.OrderByDescending(data => data.Count)
                .Select(data => data).ToList();
                return orderedInstances;
            }
            return new List<Dictionary<string, IDictionary>>();
        }
    }
}
