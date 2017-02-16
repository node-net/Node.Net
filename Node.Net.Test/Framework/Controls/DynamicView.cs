using System.Collections;
using System.Collections.Generic;
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

        public DynamicView(FrameworkElement[] elements)
        {
            DataContextChanged += DynamicView_DataContextChanged;
            foreach (FrameworkElement element in elements)
            {
                NamedViews.Add(element.GetType().Name, element);
            }
        }

        public DynamicView(IDictionary elements)
        {
            DataContextChanged += DynamicView_DataContextChanged;
            foreach (string key in elements.Keys)
            {
                FrameworkElement element = elements[key] as FrameworkElement;
                if (!object.ReferenceEquals(null, element))
                {
                    NamedViews.Add(key, element);
                }
            }
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
