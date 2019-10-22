using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class IDictionaryValueControl : StackPanel
    {
        public IDictionaryValueControl()
        {
            Orientation = Orientation.Horizontal;
            DataContextChanged += _DataContextChanged;
        }

        public bool ShowKey { get; set; } = true;
        public bool ShowValue { get; set; } = true;
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            Children.Clear();

            var dictionaryValue = DataContext as IDictionaryValue;
            if(dictionaryValue != null)
            {
                if(ShowKey)
                {
                    Children.Add(new Label { Content = dictionaryValue.Key });
                }
                if(ShowValue)
                {
                    Children.Add(new Label { Content = dictionaryValue.IDictionary[dictionaryValue.Key] });
                }
            }
        }
    }
}
