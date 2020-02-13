using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Node.Net.Collections
{
    [Serializable]
    public class Items<T> : ObservableCollection<T>, ISerializable
    {
        public Items()
        {
        }

        public Items(IEnumerable<T> source)
        {
            Source = source;
        }

        public IEnumerable<T> Source
        {
            get { return _source; }
            set
            {
                if (!object.ReferenceEquals(_source, value))
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
            get { return (T)_selectedItem!; }
            set
            {
                _selectedItem = (T)value;
                OnPropertyChanged();
            }
        }

        private object? _selectedItem = default;

        public string Search
        {
            get { return _search; }
            set
            {
                if (_search != value)
                {
                    _search = value;
                    OnPropertyChanged();
                    Update();
                }
            }
        }

        private string _search = string.Empty;

        public Func<T, string, bool> SearchFilter { get; set; } = DefaultSearchFilter;

        public static bool DefaultSearchFilter(T item, string search)
        {
            if (search.Length == 0) return true;
            if (item is IDictionary dictionary)
            {
                return dictionary.MatchesSearch(search);
            }
            return false;
        }

        public Func<IEnumerable<T>, IEnumerable<T>>? SortFunction
        {
            get { return _sortFunction; }
            set
            {
                _sortFunction = value;
                Update();
            }
        }

        private Func<IEnumerable<T>, IEnumerable<T>>? _sortFunction = null;

        private void Update()
        {
            var selectedItem = SelectedItem;
            var newItems = new List<T>();
            var removeItems = new List<T>();
            foreach (var item in Source)
            {
                if (SearchFilter(item, Search))
                {
                    newItems.Add(item);
                }
            }
            foreach (var existingItem in this)
            {
                if (!newItems.Contains(existingItem))
                {
                    removeItems.Add(existingItem);
                }
            }
            foreach (var removeItem in removeItems) { this.Remove(removeItem); }

            if (SortFunction != null)
            {
                newItems = new List<T>(SortFunction(newItems));
                Clear();
            }
            foreach (var newItem in newItems)
            {
                if (!this.Contains(newItem)) { this.Add(newItem); }
            }
        }

        private void OnPropertyChanged([CallerMemberName]string? caller = null)
        {
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(caller));
        }

        #region Serialization

        protected Items(SerializationInfo info, StreamingContext context)
        {
            int count = info.GetInt32("Count");
            for (int i = 0; i < count; ++i)
            {
                var item = info.GetValue(i.ToString(), typeof(object));
                Add((T)item);
            }
            int selected_index = info.GetInt32("SelectedIndex");
            if (selected_index != -1)
            {
                SelectedItem = this[selected_index];
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Count", Count);
            for (int i = 0; i < Count; ++i)
            {
                info.AddValue(i.ToString(), this[i]);
            }
            int selected_index = -1;
            if (SelectedItem != null)
            {
                for (int i = 0; i < Count; ++i)
                {
                    if (object.ReferenceEquals(SelectedItem, this[i]))
                    {
                        selected_index = i;
                        break;
                    }
                }
            }
            info.AddValue("SelectedIndex", selected_index);
        }

        #endregion Serialization
    }
}