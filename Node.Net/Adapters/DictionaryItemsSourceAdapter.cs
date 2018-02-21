using System.Collections;
using System.ComponentModel;

namespace Node.Net.Adapters
{
    public sealed class DictionaryItemsSourceAdapter<T> : INotifyPropertyChanged
    {
        public IEnumerable ItemsSource
        {
            get
            {
                if (itemsSource == null && Model != null)
                {
                    var items  = Model.Collect<T>();
                    itemsSource = items;
                    if (items.Count > 0) selectedItem = items[0];
                }
                return itemsSource;
            }
        }
        private IEnumerable itemsSource = null;
        public object SelectedItem
        {
            get{return selectedItem;}
            set{
                if(selectedItem != value)
                {
                    selectedItem = value;
                    NotifyPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        private object selectedItem = null;
        
        public IDictionary Model
        {
            get{return model;}
            set{
                if(model != value)
                {
                    model = value;
                    itemsSource = null;
                    selectedItem = null;
                    NotifyPropertyChanged(nameof(Model));
                    NotifyPropertyChanged(nameof(ItemsSource));
                    NotifyPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        private IDictionary model = null;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }
    }
}