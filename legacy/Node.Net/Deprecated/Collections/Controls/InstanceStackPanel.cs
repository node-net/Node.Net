using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections.Controls
{
    public class InstanceStackPanel : System.Windows.Controls.StackPanel
    {
        public InstanceStackPanel()
        {
            DataContextChanged += _DataContextChanged;
        }

        public bool Searchable { get; set; } = true;
        public string Key { get; set; } = "Type";
        public string ValuePattern { get; set; } = "";
        public bool Deep { get; set; } = true;
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private bool initialized = false;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            initialized = true;
            Update();
        }

        private System.Windows.Controls.TextBox searchTextBox;
        private void Update()
        {
            if (!initialized) return;
            if (Deep) UpdateDeep();
            else UpdateShallow();
        }
        private void UpdateDeep()
        {
            Children.Clear();
            if(Searchable)
            {
                searchTextBox = new System.Windows.Controls.TextBox
                {
                    Text = ValuePattern
                };
                searchTextBox.TextChanged += SearchTextBox_TextChanged;
                Children.Add(searchTextBox);
            }
            var dictionary = KeyValuePair.GetValue(DataContext) as IDictionary;
            if (dictionary != null)
            {
                string[] unique_types = IDictionaryExtension.CollectUniqueStrings(dictionary, Key);
                Array.Sort(unique_types, (x, y) => String.Compare(x, y));
                foreach (var type in unique_types)
                {
                    if (type.Contains(ValuePattern))
                    {
                        var instances = IDictionaryExtension.DeepCollect<IDictionary>(dictionary,
                                new KeyValueFilter { Key = Key, Values = { type } }.Include);
                        if (instances.Count > 0)
                        {
                            var expander = new InstanceExpander
                            {
                                Key = Key,
                                ValuePattern = type,
                                DataContext = instances
                            };
                            expander.ListBox.SelectionChanged += ListBox_SelectionChanged;
                            Children.Add(expander);
                        }
                    }
                }
            }
        }
        private void UpdateShallow()
        {
            Children.Clear();
            if (Searchable)
            {
                searchTextBox = new System.Windows.Controls.TextBox
                {
                    Text = ValuePattern
                };
                searchTextBox.TextChanged += SearchTextBox_TextChanged;
                Children.Add(searchTextBox);
            }
            var dictionary = KeyValuePair.GetValue(DataContext) as IDictionary;
            if (dictionary != null)
            {
                var unique_types = new List<string>();
                foreach(var k in dictionary.Keys)
                {
                    var d = dictionary[k] as IDictionary;
                    if(d != null)
                    {
                        if(d.Contains(Key))
                        {
                            var value = d[Key].ToString();
                            if(!unique_types.Contains(value))
                            {
                                unique_types.Add(value);
                            }
                        }
                    }
                }
                foreach (var type in unique_types)
                {
                    if (type.Contains(ValuePattern))
                    {
                        var instances = IDictionaryExtension.Collect<IDictionary>(dictionary,
                                new KeyValueFilter { Key = Key, Values = { type } }.Include);
                        
                        if (instances.Count > 0)
                        {
                            var expander = new InstanceExpander
                            {
                                Key = Key,
                                ValuePattern = type,
                                DataContext = instances
                            };
                            expander.ListBox.SelectionChanged += ListBox_SelectionChanged;
                            Children.Add(expander);
                        }
                    }
                }
            }
        }

        public event System.Windows.Controls.SelectionChangedEventHandler SelectionChanged;
        public object SelectedItem { get; set; }
        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox != null)
            {
                SelectedItem = listbox.SelectedItem as System.Windows.Controls.ListBoxItem;
            }
            if (SelectionChanged != null) SelectionChanged(this, e);
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ValuePattern = searchTextBox.Text;
            Update();
        }
    }
}
