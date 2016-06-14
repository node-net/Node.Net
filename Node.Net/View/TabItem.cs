﻿using System.Windows.Controls;
using System.Windows;

namespace Node.Net.View
{
    public class TabItem : System.Windows.Controls.TabItem
    {
        public TabItem()
        {
            DataContextChanged += TabItem_DataContextChanged;
        }

        private void TabItem_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {
            var element = Content as FrameworkElement;
            if (!object.ReferenceEquals(null,element))
            {
                element.DataContext = DataContext;
            }
        }
    }
}
