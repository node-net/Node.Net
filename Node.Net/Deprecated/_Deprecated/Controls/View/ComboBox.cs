﻿namespace Node.Net.View
{
    public class ComboBox : System.Windows.Controls.ComboBox
    {
        public ComboBox()
        {
            DataContextChanged += ComboBox_DataContextChanged;
        }

        void ComboBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            Items.Clear();
            var ienumerable = DataContext as System.Collections.IEnumerable;
            if (!ReferenceEquals(null,ienumerable))
            {
                foreach(object item in ienumerable)
                {
                    Items.Add(new ComboBoxItem(item));
                }
            }
        }
    }
}