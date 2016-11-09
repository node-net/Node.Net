using System;
using System.Collections;

namespace Node.Net.Collections.Controls
{
    public class InstanceExpander : System.Windows.Controls.Expander
    {
        public InstanceExpander()
        {
            DataContextChanged += _DataContextChanged;
        }

        public string Key { get; set; } = "Type";
        public string ValuePattern { get; set; } = "";
        public ListBox ListBox { get; set; }
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }

        private void Update()
        {
            var instances = GetInstances();
            Header = $"{Key} {ValuePattern} {instances.Count}";
            ListBox = new ListBox { DataContext = instances };
            Content = ListBox;
        }

        private IDictionary GetInstances()
        {
            return IDictionaryExtension.DeepCollect<IDictionary>(
                KeyValuePair.GetValue(DataContext) as IDictionary,
                new Node.Net.Collections.KeyValueFilter
                {
                    Key = Key,
                    Values = { ValuePattern }
                });
        }
    }
}
