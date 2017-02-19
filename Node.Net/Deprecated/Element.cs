using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Node.Net.Deprecated
{
    public class Element :  INotifyPropertyChanged, IElement
    {
        private IDictionary Data { get; } = new Dictionary<string, dynamic>();
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
        protected bool SetField<T>(ref T field,T value, [CallerMemberName] string propertyName=null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /*
        public string Name
        {
            get
            {
                var key = data.GetKey();
                if (key != null) return key.ToString();
                return string.Empty;
            }
        }*/
        /*
        public void Clear() { Data.Clear(); }
        public bool Contains(string name) { return Data.ContainsKey(name); }
        public dynamic Get(string name) { return Data[name]; }
        public T Get<T>(string name) { return IElementExtension.Get<T>(this,name); }
        public void Set(string name, dynamic value)
        {
            Data[name] = value;
            var element = value as Element;
            if (element != null && element.Parent != this)
            {
                element.Parent = this;
                element.Document = IElementExtension.GetDocument(element);
                element.Name = null;
                element.FullName = null;
            }
        }*/
        [Browsable(false)]
        public string JSON { get { return Data.GetJSON(); } }

        
        public IDocument Document { get; private set; }
        /*
        [Browsable(false)]
        public int Count { get { return Data.Count; } }
        [Browsable(false)]
        public ICollection<string> Keys { get { return Data.Keys; } }
        */

        public IList Find(Type target_type, string pattern = "") { return IElementExtension.Find(this, target_type, pattern); }
        public string Name
        {
            get
            {
                if (name == null) name = IElementExtension.GetName(this);
                return name;
            }
            private set { name = value; }
        }
        private string name = null;
        public string FullName
        {
            get
            {
                if (fullName == null) fullName = IElementExtension.GetFullName(this);
                return fullName;
            }
            private set { fullName = value; }
        }
        private string fullName = null;

        public object Parent
        {
            get { return Node.Net.Deprecated.Collections.ObjectExtension.GetParent(this); }
            set { Node.Net.Deprecated.Collections.ObjectExtension.SetParent(this, value); }
        }
        public void DeepUpdateParents()
        {
            Node.Net.Deprecated.Collections.IDictionaryExtension.DeepUpdateParents(this);
        }

        #region ICollection Members
        public bool IsSynchronized { get { return Data.IsSynchronized; } }
        public object SyncRoot { get { return Data.SyncRoot; } }
        public int Count { get { return Data.Count; } }
        public void CopyTo(Array array, int index) { foreach (var key in Data.Keys) { array.SetValue(Data[key], index); ++index; } }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator() { return ((IDictionary)this).GetEnumerator(); }
        #endregion

        #region IDictionary Members
        public virtual bool IsReadOnly { get { return Data.IsReadOnly; } }
        public bool Contains(object key) { return Data.Contains(key.ToString()); }
        public virtual bool IsFixedSize { get { return Data.IsFixedSize; } }
        public virtual void Remove(object key) { Data.Remove(key); }
        public virtual void Clear() { Data.Clear(); }
        public virtual void Add(object key, object value) { Data.Add(key, value); }
        public ICollection Keys { get { return Data.Keys; } }
        public ICollection Values { get { return Data.Values; } }
        public virtual object this[object key]
        {
            get { return Data[key.ToString()]; }
            set {
                Data[key] = value;
                var element = value as Element;
                if (element != null) element.Parent = this;
            }
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
    }
}
