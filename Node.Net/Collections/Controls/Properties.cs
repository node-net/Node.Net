using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Collections.Controls
{
    public class Properties : Grid
    {
        public Properties()
        {
            DataContextChanged += Properties_DataContextChanged;
        }

        private void Properties_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (initialized) Update();
        }

        private bool initialized = false;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            initialized = true;
            Update();
        }

        private void Update()
        {
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            Children.Clear();

            //if (grid.DataContext == null) return;
            var dictionary = KeyValuePair.GetValue(DataContext) as IDictionary;
            if (dictionary != null)
            {
                var keys = new List<string>(GetKeys(dictionary));
                if (keys.Count > 0)
                {
                    var rowIndex = 0;
                    ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    if (ShowKeys) ColumnDefinitions.Add(new ColumnDefinition());
                    foreach (string key in keys)
                    {
                        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        if (ShowKeys)
                        {
                            var keyLabel = new Label { Content = key, Background = Brushes.LightGray };
                            Children.Add(keyLabel);
                            Grid.SetRow(keyLabel, rowIndex);
                        }
                        var value = dictionary[key];
                        if (value != null)
                        {
                            //var keyValue = new Label { Content = GetStringValue(value) };
                            var keyValue = new TextBox { Text = GetStringValue(value), TextWrapping = TextWrapping.Wrap, VerticalContentAlignment = VerticalAlignment.Center, BorderBrush = null };
                            Children.Add(keyValue);
                            Grid.SetColumn(keyValue, ShowKeys ? 1 : 0);
                            Grid.SetRow(keyValue, rowIndex);
                        }
                        ++rowIndex;
                    }
                }
            }
        }

        public bool ShowKeys { get; set; } = true;
        protected string[] GetKeys(IDictionary dictionary)
        {
            var keys = new List<string>();
            foreach (string key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value == null || value.GetType().IsValueType || value.GetType() == typeof(string)
                    || value.GetType() == typeof(string[]) || value.GetType() == typeof(double[])
                    || value.GetType() == typeof(int[]) || value.GetType() == typeof(double[,]))
                {
                    if (!keys.Contains(key)) keys.Add(key);
                }
            }
            return keys.ToArray();
        }
        private string GetStringValue(object value)
        {
            if (value == null) return "";
            if (value.GetType() != typeof(string) && typeof(IEnumerable).IsAssignableFrom(value.GetType()))
            {
                var ienumerable = value as IEnumerable;
                var stringBuilder = new StringBuilder();
                foreach (var item in ienumerable)
                {
                    if (stringBuilder.Length > 0) stringBuilder.Append(",");
                    stringBuilder.Append(item.ToString());
                }
                return stringBuilder.ToString();
            }
            return value.ToString();
        }
    }
}
