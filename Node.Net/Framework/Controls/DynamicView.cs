﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Framework.Controls
{
    public class DynamicView : Grid
    {
        public DynamicView()
        {
            this.DataContextChanged += DynamicView_DataContextChanged;
        }
        public DynamicView(FrameworkElement element0, FrameworkElement element1)
        {
            DataContextChanged += DynamicView_DataContextChanged;
            NamedViews.Add(element0.GetType().Name, element0);
            NamedViews.Add(element1.GetType().Name, element1);
        }
        public DynamicView(FrameworkElement element0, FrameworkElement element1,FrameworkElement element2)
        {
            DataContextChanged += DynamicView_DataContextChanged;
            NamedViews.Add(element0.GetType().Name, element0);
            NamedViews.Add(element1.GetType().Name, element1);
            NamedViews.Add(element2.GetType().Name, element2);
        }

        public Dictionary<string, FrameworkElement> NamedViews = new Dictionary<string, FrameworkElement>();
        private string currentViewName = "";
        private void DynamicView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            update();
        }
        private void update()
        {
            Children.Clear();
            FrameworkElement current = CurrentView;
            if(!object.ReferenceEquals(null, current))
            {
                Children.Add(current);
                current.DataContext = DataContext;
            }
        }

        private FrameworkElement CurrentView
        {
            get
            {
                if(currentViewName.Length == 0 && NamedViews.Count > 0)
                {
                    currentViewName = NamedViews.Keys.First();
                }
                if (NamedViews.ContainsKey(currentViewName)) return NamedViews[currentViewName];
                return null;
            }
        }

        public void SetCurrentView(string name)
        {
            if (NamedViews.ContainsKey(name))
            {
                currentViewName = name;
                update();
            }
        }

        public MenuItem GetViewMenuItem()
        {
            MenuItem viewMenuItem = new MenuItem() { Header = "View" };
            foreach (string key in NamedViews.Keys)
            {
                MenuItem item = new MenuItem() { Header = key };
                item.Click += ViewMenuItem_Click;
                viewMenuItem.Items.Add(item);
            }
            return viewMenuItem;
        }

        private void ViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            SetCurrentView(menuItem.Header.ToString());
        }
    }
}
