using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace Node.Net.Collections
{
    public class Element : INotifyPropertyChanged, IDictionary
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IDictionary Model
        {
            get
            {
                if (model == null) model = new Dictionary<string, dynamic>();
                return model;
            }
            set
            {
                if (model != value)
                {
                    model = value;
                    if (model != null) model.DeepUpdateParents();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Model)));
                    itemsSource = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsSource)));
                }
            }
        }
        private IDictionary model = new Dictionary<string, dynamic>();

        #region ICollection Members
        public bool IsSynchronized { get { return Model.IsSynchronized; } }
        public object SyncRoot { get { return Model.SyncRoot; } }
        public int Count { get { return Model.Count; } }
        public void CopyTo(Array array, int index) { foreach (var key in Model.Keys) { array.SetValue(Model[key], index); ++index; } }
        #endregion
        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator() { return ((IDictionary)this).GetEnumerator(); }
        #endregion

        #region IDictionary Members
        public virtual bool IsReadOnly { get { return true; } }
        public bool Contains(object key) { return Model.Contains(key.ToString()); }
        public virtual bool IsFixedSize { get { return Model.IsFixedSize; } }
        public virtual void Remove(object key) { Model.Remove(key); }
        public virtual void Clear() { Model.Clear(); }
        public virtual void Add(object key, object value) { Model.Add(key, value); }
        public ICollection Keys { get { return Model.Keys; } }
        public ICollection Values { get { return Model.Values; } }
        public virtual object this[object key]
        {
            get { return Model[key.ToString()]; }
            set { Model[key]=value; }
        }
        public IDictionaryEnumerator GetEnumerator() { return new ElementEnumerator(this); }
        private class ElementEnumerator : IDictionaryEnumerator
        {
            readonly Element element;
            readonly Dictionary<int, string> indexedKeys;
            Int32 index = -1;

            public ElementEnumerator(Element value)
            {
                element = value;

                indexedKeys = new Dictionary<int, string>();
                var i = 0;
                foreach (var key in element.Keys)
                {
                    indexedKeys.Add(i, key.ToString());
                    ++i;
                }
            }

            public Object Current { get { return new DictionaryEntry(Key, Value); } }
            public DictionaryEntry Entry { get { return (DictionaryEntry)Current; } }
            public Object Key { get { ValidateIndex(); return indexedKeys[index]; } }
            public Object Value { get { ValidateIndex(); return element[indexedKeys[index]]; } }
            public Boolean MoveNext()
            {
                if (index < indexedKeys.Count - 1) { index++; return true; }
                return false;
            }

            private void ValidateIndex()
            {
                if (index < 0 || index >= indexedKeys.Count)
                    throw new InvalidOperationException("Enumerator is before or after the collection.");
            }
            public void Reset() { index = -1; }
        }
        #endregion
        public string Key
        {
            get
            {
                var key = Model.GetKey();
                if (key != null) return key.ToString();
                return "?";
            }
        }
        public override string ToString()
        {
            return $"{Key}";
        }
        public bool HasKey(string name) => Model.Contains(name);
        public object Get(string name) => Model.Get<object>(name);
        public T Get<T>(string name) => Model.Get<T>(name);
        public void Set(string name, object value)
        {
            var hadKey = HasKey(name);
            if (Get(name) != value)
            {
                Model.Set(name, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                if (value != null)
                {
                    if (value.GetType() != typeof(string))
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                        {
                            itemsSource = null;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                        }
                    }
                }
            }
        }
        public void Remove(string name)
        {
            if (HasKey(name))
            {
                var value = Get(name);
                Model.Remove(name);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                if (value != null)
                {
                    if (value.GetType() != typeof(string))
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                        {
                            itemsSource = null;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                        }
                    }
                }
            }
        }

        public ObservableCollection<Element> ItemsSource
        {
            get
            {
                if (itemsSource == null)
                {
                    itemsSource = new ObservableCollection<Element>();
                    foreach (string key in Model.Keys)
                    {
                        var child = Model[key] as IDictionary;
                        if (child != null)
                        {
                            itemsSource.Add(new Element { Model = child });
                        }
                    }
                }
                return itemsSource;
            }
        }
        private ObservableCollection<Element> itemsSource;

        public ImageSource ImageSource
        {
            get
            {
                return null;
            }
        }
    }
}
