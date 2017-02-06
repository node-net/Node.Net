using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Node.Net
{
    public class Element : INotifyPropertyChanged, IElement, Node.Net.Readers.IElement
    {
        private Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
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
        public void Clear() { data.Clear(); }
        public bool Contains(string name) { return data.ContainsKey(name); }
        public dynamic Get(string name) { return data[name]; }
        public T Get<T>(string name) { return IElementExtension.Get<T>(this,name); }
        public void Set(string name, dynamic value)
        {
            data[name] = value;
            var element = value as Element;
            if (element != null && element.Parent != this)
            {
                element.Parent = this;
                element.Document = IElementExtension.GetDocument(element);
                element.Name = null;
                element.FullName = null;
            }
        }
        [Browsable(false)]
        public string JSON { get { return this.GetJSON(); } }

        public object Parent { get; set; }
        public IDocument Document { get; private set; }

        [Browsable(false)]
        public int Count { get { return data.Count; } }
        [Browsable(false)]
        public ICollection<string> Keys { get { return data.Keys; } }

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

        public void DeepUpdateParents()
        {

        }
    }
}
