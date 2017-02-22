using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Viewer
{
    public class TreeElement
    {

        public Node.Net.Deprecated.Element Element;
        public string Name { get { return Element.Name; } }
        public ObservableCollection<object> ItemsSource
        {
            get
            {
                if(itemsSource == null)
                {
                    itemsSource = new ObservableCollection<object>();
                    foreach(string key in Element.Keys)
                    {
                        var child = Element[key] as Node.Net.Deprecated.Element;
                        if (child != null) itemsSource.Add(new TreeElement { Element = child });
                    }
                }
                return itemsSource;
            }
        }
        private ObservableCollection<object> itemsSource;
    }
}
