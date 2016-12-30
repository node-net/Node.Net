using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Node.Net.Collections
{
    public class Dictionary : Dictionary<string, dynamic>
    {
        public Dictionary() { }
        public Dictionary(string type) { Add(nameof(Type), type); }
        public string Type
        {
            get
            {
                var value = IDictionaryExtension.Get<string>(this, nameof(Type));
                return (value == null) ? string.Empty : value.ToString();
            }
        }
        public string Key
        {
            get
            {
                var key = ObjectExtension.GetKey(this);
                return (key == null) ? string.Empty : key.ToString();
            }
        }
        public string FullKey => ObjectExtension.GetFullKey(this);
        public string[] Types => IDictionaryExtension.CollectValues<string>(this, nameof(Type));
        public void UpdateParentReferences() => IDictionaryExtension.DeepUpdateParents(this);
        public ObservableCollection<Node.Net.Collections.Dictionary> ItemsSource
        {
            get
            {
                var itemsSource = new ObservableCollection<Dictionary>();
                var items = this.GetItemsSource();
                foreach(var item in items)
                {
                    var d = item as Node.Net.Collections.Dictionary;
                    if(d != null)
                    {
                        itemsSource.Add(d);
                    }
                }
                return itemsSource;
            }
        }

        public string JSON
        {
            get
            {
                return "TODO insert JSON....";
            }
        }
    }
}
