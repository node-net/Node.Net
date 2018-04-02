using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Node.Net
{
    public class Element : Dictionary<string, dynamic>, INotifyPropertyChanged
    {
        public Element()
        {
        }

        public Element(IDictionary dictionary)
        {
            foreach (var key in dictionary.Keys) { this.Add(key.ToString(), dictionary[key]); }
        }

        public void Save(string filename)
        {
            Node.Net.Writer.Default.Write(filename, this);
        }

        public void Save(Stream stream)
        {
            Node.Net.Writer.Default.Write(stream, this);
        }

        [Browsable(false)]
        public object Parent
        {
            get { return Node.Net.ObjectExtension.GetParent(this); }
            set { Node.Net.ObjectExtension.SetParent(this, value); }
        }

        [Category("Identity")]
        public virtual string Name
        {
            get
            {
                if (ContainsKey("Name"))
                {
                    var name = this.Get<string>("Name");
                    if (name.Length > 0) return name;
                }
                return Node.Net.ObjectExtension.GetName(this);
            }
            set { Set("Name", value); }
        }

        #region IResources

        [Browsable(false)]
        public ResourceDictionary Resources
        {
            get { return resources; }
            set { SetField<ResourceDictionary>(ref resources, value); }
        }

        private ResourceDictionary resources;

        public object FindResource(object key)
        {
            if (Resources != null && Resources.Contains(key)) return Resources[key];
            return null;
        }

        #endregion IResources

        public IList Search(string search)
        {
            var result = Node.Net.IDictionaryExtension.Collect(this, typeof(IDictionary), search);
            var list = new List<object>();
            foreach (var item in result) { list.Add(item); }
            return list.OrderBy(o => Node.Net.ObjectExtension.GetName(o)).ToList();
        }

        public IList<T> Search<T>(string search) => Node.Net.IDictionaryExtension.Collect<T>(this, search);

        public IList<T> Search<T>(Func<object, bool> filter) => new List<T>(Node.Net.IDictionaryExtension.Collect<T>(this).Where(x => filter(x)));//,new FilterAdapter( filter).Include);

        [Browsable(false)]
        public IList Properties
        {
            get
            {
                var properties = new List<KeyValuePair<string, object>>();
                foreach (var property in this.GetType().GetProperties())
                {
                    if (property.GetIndexParameters().Length == 0)
                    {
                        if (property.Name != "Properties")
                        {
                            properties.Add(new KeyValuePair<string, object>(property.Name, property.GetValue(this)));
                        }
                    }
                }
                return properties;
            }
        }

        [Browsable(false)]
        public virtual object[] Children
        {
            get
            {
                var children = new List<object>();
                foreach (var key in Keys)
                {
                    var value = this[key] as IDictionary;
                    if (value != null) children.Add(value);
                }
                return children.ToArray();
            }
        }

        public virtual void Update()
        {
        }

        public new void Add(string key, object value)
        {
            base.Add(key, value);
            Node.Net.ObjectExtension.SetParent(value, this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ConcurrentCache.Clear();
        }

        public void Forward_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null) return;
            OnPropertyChanged(e.PropertyName);
        }

        public void RaisePropertyChanged(string name)
        {
            OnPropertyChanged(name);
        }

        public T Get<T>(string name, T defaultValue = default(T))
        {
            return IDictionaryExtension.Get<T>(this, name, defaultValue);
        }

        [Browsable(false)]
        public IDictionary Document
        {
            get
            {
                if (document == null)
                {
                    var ancestor = this.GetFurthestAncestor<IDictionary>();
                    if (!object.ReferenceEquals(this, ancestor))
                    {
                        document = ancestor;
                    }
                }
                return document;
            }
            set
            {
                if (SetField<IDictionary>(ref document, value))
                {
                    OnDocumentChanged();
                }
            }
        }

        private IDictionary document;

        protected virtual void OnDocumentChanged()
        {
        }

        public void Set(string name, object value)
        {
            Node.Net.IDictionaryExtension.Set(this, name, value);
            OnPropertyChanged(name);
        }

        protected bool SetField<V>(ref V field, V value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<V>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        [Browsable(false)]
        public object Key { get { return Node.Net.ObjectExtension.GetKey(this); } }

        [Browsable(false)]
        public string TypeName
        {
            get { return Node.Net.IDictionaryExtension.GetTypeName(this); }
            set { Node.Net.IDictionaryExtension.Set(this as IDictionary, "Type", value); }
        }

        [Browsable(false)]
        public string FileName
        {
            get { return ObjectExtension.GetFileName(this); }
            set { ObjectExtension.SetFileName(this, value); }
        }

        [Browsable(false)]
        public string ShortFileName
        {
            get
            {
                var filename = FileName;
                try
                {
                    var finfo = new FileInfo(filename);
                    filename = finfo.Name;
                }
                catch { filename = FileName; }
                return filename;
            }
        }

        public IList Collect(string type) => IDictionaryExtension.Collect(this, type);

        public IList Collect(Type type, string search) => IDictionaryExtension.Collect(this, type, search);

        public IList<T> Collect<T>() => IDictionaryExtension.Collect<T>(this);

        public IList<T> Collect<T>(Func<object, bool> filter) => new List<T>(Node.Net.IDictionaryExtension.Collect<T>(this).Where(x => filter(x)));//,new FilterAdapter( filter));

        public T Find<T>(string name, bool exact = false) => IDictionaryExtension.Find<T>(this, name, exact);

        [Browsable(false)]
        public ConcurrentDictionary<string, dynamic> ConcurrentCache { get { return concurrentCache; } }

        private ConcurrentDictionary<string, dynamic> concurrentCache = new ConcurrentDictionary<string, dynamic>();

        [Browsable(false)]
        public new ICollection<string> Keys { get { return base.Keys; } }

        [Browsable(false)]
        public new ICollection<dynamic> Values { get { return base.Values; } }

        [Browsable(false)]
        public new int Count { get { return base.Count; } }

        [Browsable(false)]
        public new IEqualityComparer<string> Comparer { get { return base.Comparer; } }
    }
}
