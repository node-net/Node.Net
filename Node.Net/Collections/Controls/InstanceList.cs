using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections.Controls
{
    public class InstanceList : System.Windows.Controls.Frame
    {
        public InstanceList()
        {
            DataContextChanged += _DataContextChanged;
            JournalOwnership = System.Windows.Navigation.JournalOwnership.UsesParentJournal;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        public string Key { get; set; } = "Type";
        public string ValuePattern { get; set; } = "";
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            var stackPanel = new System.Windows.Controls.StackPanel
            {
                Orientation = System.Windows.Controls.Orientation.Vertical
            };
            Content = stackPanel;
            //RowDefinitions.Clear();
            //Children.Clear();
            var dictionary = KeyValuePair.GetValue(DataContext) as IDictionary;
            if(dictionary != null)
            {
                string[] unique_types = IDictionaryExtension.CollectUniqueStrings(dictionary, Key);
                Array.Sort(unique_types,(x,y) => String.Compare(x,y));

                var items = IDictionaryExtension.DeepCollect<IDictionary>(dictionary);
                foreach(var type in unique_types)
                {
                    if (type.Contains(ValuePattern))
                    {
                        var instances = new Dictionary<string, dynamic>();
                        foreach (var key in items.Keys)
                        {
                            var child_item = items[key] as IDictionary;
                            if (child_item != null && child_item.Contains(Key) && child_item[Key] == type)
                            {
                                instances.Add(key, child_item);
                            }
                        }
                        if (instances.Count > 0)
                        {
                            var expander = new System.Windows.Controls.Expander
                            {
                                Header = $"{type} {instances.Count}",
                                Content = new Node.Net.Collections.Controls.ListBox { DataContext = instances }
                            };
                            //RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = System.Windows.GridLength.Auto });
                            stackPanel.Children.Add(expander);
                            //System.Windows.Controls.Grid.SetRow(expander, RowDefinitions.Count);
                        }
                    }
                }
                //RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
            }
        }
    }
}
