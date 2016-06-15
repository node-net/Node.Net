using System.ComponentModel;

namespace Node.Net.Collections
{
#if NET35
    public class HashBase : System.Collections.Generic.Dictionary<string, object>, System.IComparable, System.ComponentModel.INotifyPropertyChanged
#else
    public class HashBase : System.Collections.Generic.Dictionary<string, dynamic>, System.IComparable, System.ComponentModel.INotifyPropertyChanged
#endif
    {
        public HashBase() { Initialize(); }
        public HashBase(string json)
        {
            GetReader().Read(json, this);
            Initialize();
        }

        public HashBase(System.IO.Stream stream){Open(stream);}
        public HashBase(System.Collections.IDictionary source)
        {
            Hash.Copy(source, this);
            Initialize();
        }
       
        protected virtual void Initialize(){}
        protected virtual Json.Reader GetReader() => new Json.Reader();
        protected virtual Json.Writer GetWriter() => new Json.Writer();
        public override int GetHashCode() => GetHashCode(this);

        public override bool Equals(object obj)
        {
            if (CompareTo(obj) == 0) return true;
            return false;
        }

        public int CompareTo(object value)
        {
            if (object.ReferenceEquals(this, value)) return 0;
            if (object.ReferenceEquals(null, value)) return 1;

            int thisHash = GetHashCode();
            int thatHash = GetHashCode(value);
            return GetHashCode().CompareTo(GetHashCode(value));
        }

        public static void Copy(System.Collections.IDictionary source, System.Collections.IDictionary destination, IFilter filter = null)
        {
            Copier.Copy(source, destination, filter);
        }
        public static string ToJson(System.Collections.IDictionary source) => Json.Writer.ToString(source);

        public string ToJson() => ToJson(this);

        public static int GetHashCode(object value)
        {
            if (!object.ReferenceEquals(null, value))
            {
                if (value.GetType() == typeof(bool) ||
                   value.GetType() == typeof(double) ||
                    value.GetType() == typeof(string)) return value.GetHashCode();
                else
                {
                    if (typeof(System.Collections.IDictionary).IsAssignableFrom(value.GetType())) return GetHashCode(value as System.Collections.IDictionary);
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType())) return GetHashCode(value as System.Collections.IEnumerable);
                }
            }
            return 0;
        }
        public static int GetHashCode(System.Collections.IEnumerable value)
        {
            int count = 0;
            int hashCode = 0;
            foreach (object item in value)
            {
                int tmp = GetHashCode(item);
                if (tmp != 0) count++;
                hashCode = hashCode ^ tmp;
            }
            hashCode = hashCode ^ count;
            return hashCode;
        }
        public static int GetHashCode(System.Collections.IDictionary value)
        {
            int hashCode = value.Count;
            foreach (string key in value.Keys)
            {
                hashCode = hashCode ^ GetHashCode(key) ^ GetHashCode(value[key]);
            }
            return hashCode;
        }

        public static System.Collections.IList GetChildren(System.Collections.IDictionary value)
        {
            System.Collections.Generic.List<object> children = new System.Collections.Generic.List<object>();
            foreach (object key in value.Keys)
            {
                object item = value[key];
                if (!object.ReferenceEquals(null, item))
                {
                    if (!typeof(string).IsAssignableFrom(item.GetType()))
                    {
                        System.Collections.IEnumerable ienumerable = item as System.Collections.IEnumerable;
                        if (!object.ReferenceEquals(null, ienumerable)) { children.Add(item); }
                    }
                }
            }
            return children;
        }

        public void Save(string filename, Json.JsonFormat format = Json.JsonFormat.Indented)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
            if (!System.IO.Directory.Exists(fi.DirectoryName)) System.IO.Directory.CreateDirectory(fi.DirectoryName);
            using (System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Create))
            {
                GetWriter().Write(fs,this);
            }
        }


        public void Save(System.IO.Stream stream)
        {
            GetWriter().Write(stream, this);
            stream.Flush();
        }

        public virtual void Open(System.IO.Stream stream)
        {
            Clear();
            GetReader().Read(stream, this);
            Initialize();
        }
        public virtual void Open(string filename)
        {
            using (System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Open))
            {
                Open(fs);
            }
        }

        private Document document = null;
        [System.ComponentModel.Browsable(false)]
        public virtual Document Document
        {
            get { return document; }
            set { document = value; }
        }
        
        [System.ComponentModel.Browsable(false)]
        public object Parent
        {
            get 
            {
                if (!object.ReferenceEquals(null, Document))
                {
                    return Document.Traverser.GetParent(this);
                }
                return null;
            }
        }
        public T GetAncestor<T>()
        {
            if (!object.ReferenceEquals(null, Document))
            {
                return Document.Traverser.GetAncestor<T>(this);
            }
            return default(T);
        }

        public static System.Collections.IDictionary Convert(System.Collections.IDictionary source, System.Collections.IDictionary typeConversions)
        {
            System.Collections.IDictionary result = new Hash();
            if (source.Contains("Type"))
            {
                if (typeConversions.Contains(source["Type"].ToString()))
                {
                    System.Type type = typeConversions[source["Type"].ToString()] as System.Type;
                    if (!object.ReferenceEquals(null, type))
                    {
                        System.Type[] types = { typeof(System.Collections.IDictionary) };
                        System.Reflection.ConstructorInfo dictionaryConstructor
                            = type.GetConstructor(types);
                        if (!object.ReferenceEquals(null, dictionaryConstructor))
                        {
                            object[] args = { source };
                            return System.Activator.CreateInstance(type, args) as System.Collections.IDictionary;
                        }
                        result = System.Activator.CreateInstance(type) as System.Collections.IDictionary;
                    }
                }
            }

            foreach (object key in source.Keys)
            {
                object value = source[key];
                System.Collections.IDictionary dictionary = value as System.Collections.IDictionary;
                if (!object.ReferenceEquals(null, dictionary))
                {
                    result[key] = Convert(dictionary, typeConversions);
                }
                else
                {
                    System.Collections.IEnumerable enumerable = value as System.Collections.IEnumerable;
                    if (!object.ReferenceEquals(null, value) && value.GetType() != typeof(string)
                        && !object.ReferenceEquals(null, enumerable))
                    {
                        // TODO, convert array elements
                        result[key] = enumerable;
                    }
                    else
                    {
                        result[key] = value;
                    }
                }
            }

            return result;
        }

        [System.ComponentModel.Browsable(false)]
        public new System.Collections.Generic.Dictionary<string, object>.KeyCollection Keys => base.Keys;

        [System.ComponentModel.Browsable(false)]
        public new System.Collections.Generic.Dictionary<string, object>.ValueCollection Values => base.Values;

        [System.ComponentModel.Browsable(false)]
        public new System.Collections.Generic.IEqualityComparer<string> Comparer => base.Comparer;

        [System.ComponentModel.Browsable(false)]
        public new int Count => base.Count;

        public int GetCount(System.Type type)
        {
            int count = 0;
            foreach (string key in Keys)
            {
                if(!object.ReferenceEquals(null,this[key]))
                {
                    if (type.IsAssignableFrom(this[key].GetType())) ++count;
                }
            }
            return count;
        }

        public int GetCount<T>() => GetCount(typeof(T));
        public int GetDeepCount<T>() => Traverser.DeepCount<T>(this);
        public string[] CollectKeys<T>() => Traverser.CollectKeys<T>(this);

        public T[] Collect<T>() => Traverser.Collect<T>(this);

        public T[] DeepCollect<T>() => Traverser.DeepCollect<T>(this);

        public virtual void Update(bool deep = true) { if (deep) { Traverser.Update(this); } }

        public T Get<T>(string key)
        {
            if (ContainsKey(key)) return (T)this[key];
            return default(T);
        }

        public T Get<T>(int index)
        {
            int i = 0;
            foreach (string key in CollectKeys<T>())
            {
                if (i == index) return (T)this[key];
                ++i;
            }
            return default(T);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        
#if NET35
        public new object this[string key]
#else
        public new dynamic this[string key]
#endif
        {
            get
            {
                if (!ContainsKey(key)) return null;
                return base[key];
            }
            set
            {
                bool notify = false;
                object current_value = null;
                if(ContainsKey(key))
                {
                    current_value = base[key];
                }
                if(ReferenceEquals(null, value))
                {
                    if (!ReferenceEquals(null, current_value)) notify = true;
                }
                else
                {
                    if (ReferenceEquals(null, current_value)) notify = true;
                    else
                    {
                        if(value.GetType() == current_value.GetType())
                        {
                            if (!value.Equals(current_value)) notify = true;
                        }
                        else { notify = true; }
                    }
                }
                base[key] = value;
                if (notify) NotifyPropertyChanged(key);
            }
        }

#if NET35
        public object this[int index]
#else
        public dynamic this[int index]
#endif
        {
            get
            {
                int i = 0;
                foreach(string key in Keys)
                {
                    if (i == index) return this[key];
                    ++i;
                }
                return null;
            }
        }

        public T[] ToArray<T>()
        {
            System.Collections.Generic.List<T> items = new System.Collections.Generic.List<T>();
            foreach(string key in Keys)
            {
                if(!object.ReferenceEquals(null,this[key]))
                {
                    if (typeof(T).IsAssignableFrom(this[key].GetType())) items.Add((T)this[key]);
                }
            }
            return items.ToArray();
        }


        [System.ComponentModel.Browsable(false)]
        public virtual string Key => "";
    }
}
