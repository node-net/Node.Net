using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Internal.GridUpdaters
{
    abstract class ValuesGridUpdater
    {
        public ValuesGridUpdater(System.Windows.Controls.Grid grid)
        {
            grid.DataContextChanged += Grid_DataContextChanged;
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateGrid(sender as System.Windows.Controls.Grid);
        }

        public bool ShowKeys { get; set; } = true;
        protected string[] GetKeys(IDictionary dictionary)
        {
            var keys = new List<string>();
            foreach (string key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value == null || value.GetType().IsValueType || value.GetType() == typeof(string))
                {
                    if (!keys.Contains(key)) keys.Add(key);
                }
            }
            return keys.ToArray();
        }

        public abstract void UpdateGrid(System.Windows.Controls.Grid grid);
        protected UIElement GetKeyElement(IDictionary dictionary, string key)
        {
            return new Label { Content = key, Background = Brushes.LightGray };
        }
        protected UIElement GetValueElement(IDictionary dictionary, string key)
        {
            var value = dictionary[key];
            if (value == null) return new Label();
            else return new Label { Content = value.ToString() };
        }
    }
}
