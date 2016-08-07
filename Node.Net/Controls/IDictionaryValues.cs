using System;
using System.Collections;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class IDictionaryValues : Grid
    {
        public IDictionaryValues()
        {
            DataContextChanged += _DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            ColumnDefinitions.Clear();
            Children.Clear();
            var idictionary = DataContext as IDictionary;
            if (idictionary != null)
            {
                var index = 0;
                foreach (string key in idictionary.Keys)
                {
                    var dictionaryValue = new DictionaryValue { IDictionary = idictionary, Key = key };
                    ColumnDefinitions.Add(new ColumnDefinition());
                    var dictionaryValueControl = new IDictionaryValueControl { DataContext = dictionaryValue };
                    Children.Add(dictionaryValueControl);
                    Grid.SetColumn(dictionaryValueControl, index);
                    ++index;
                }
            }
        }
    }
}
