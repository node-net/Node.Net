using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Node.Net.Collections
{
    public sealed class Items<T> : ObservableCollection<T>
    {
        public Items(IEnumerable<T> source)
        {
            Source = source;
        }

        public IEnumerable<T> Source
        {
            get { return _source; }
            set
            {
                if(!object.ReferenceEquals(_source,value))
                {
                    _source = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Source)));
                    Update();
                }
            }
        }

        private IEnumerable<T> _source = new List<T>();

        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if(!object.ReferenceEquals(_selectedItem,value))
                {
                    _selectedItem = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(SelectedItem)));
                }
            }
        }

        private T _selectedItem;

        public string Search
        {
            get { return _search; }
            set
            {
                if(_search != value)
                {
                    _search = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Search)));
                    Update();
                }
            }
        }

        private string _search = string.Empty;

        public Func<T, string,bool> SearchFilter { get; set; } = DefaultSearchFilter;

        public static bool DefaultSearchFilter(T item,string search)
        {
            if (search.Length == 0) return true;
            if(item is IDictionary dictionary)
            {
                return dictionary.MatchesSearch(search);
            }
            return false;
        }

        private void Update()
        {
            var selectedItem = SelectedItem;
            var newItems = new List<T>();
            var removeItems = new List<T>();
            foreach(var item in Source)
            {
                if (SearchFilter(item,Search))
                {
                    newItems.Add(item);
                }
            }

            foreach(var newItem in newItems)
            {
                if (!this.Contains(newItem)) { this.Add(newItem); }
            }
            foreach(var existingItem in this)
            {
                if(!newItems.Contains(existingItem))
                {
                    removeItems.Add(existingItem);
                }
            }
            foreach(var removeItem in removeItems) { this.Remove(removeItem); }
        }
    }
}
