using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Viewer
{
    public class Document : Node.Net.Document
    {
        public static Document Open()
        {
            using (var reader = new Node.Net.Reader { DefaultDocumentType = typeof(Document) })
            {
                return reader.Open() as Document;
            }
        }
        public object SelectedItem
        {
            get { return selectedItem; }
            set { SetField<object>(ref selectedItem, value); }
        }
        private object selectedItem = null;
        
        public ObservableCollection<object> TreeViewItemsSource
        {
            get
            {
                if(treeViewItemsSource == null)
                {
                    treeViewItemsSource = new ObservableCollection<object>();
                    foreach(string key in Keys)
                    {
                        var element = this[key] as Node.Net.Element;
                        if(element != null)
                        {
                            treeViewItemsSource.Add(new TreeElement { Element = element });
                        }
                    }
                }
                return treeViewItemsSource;
            }
            set { SetField <ObservableCollection<object>>(ref treeViewItemsSource, value); }
        }
        private ObservableCollection<object> treeViewItemsSource;
    }
}
