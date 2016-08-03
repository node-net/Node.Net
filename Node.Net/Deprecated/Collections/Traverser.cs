namespace Node.Net.Deprecated.Collections
{
    public class Traverser
    {
        private MetaDataManager metaData = null;
        public Traverser() { metaData = new MetaDataManager(); }

        public Traverser(object model) 
        { 
            metaData = new MetaDataManager();
            Traverse(model);
        }

        public IMetaDataManager MetaData
        {
            get { return metaData; }
        } 
        public object GetParent(object value)
        {
            if(metaData.ContainsKey(value) && metaData[value].Contains("Parent"))
            {
                return metaData[value]["Parent"];
            }
            return null;
        }

        public object GetRoot(object value)
        {
            var root = GetParent(value);
            while(!object.ReferenceEquals(null,GetParent(root)))
            {
                root = GetParent(root);
            }
            return root;
        }

        public T GetAncestor<T>(object value)
        {
            var ancestor = GetParent(value);
            while (!object.ReferenceEquals(null, ancestor))
            {
                if(typeof(T).IsAssignableFrom(ancestor.GetType())) return (T)ancestor;
                ancestor = GetParent(ancestor);
            }
            return default(T);
        }


        public static int Count<T>(object value)
        {
            var count = 0;
            var dictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                foreach (string key in dictionary.Keys)
                {
                    if (!object.ReferenceEquals(null, dictionary[key]))
                    {
                        if (typeof(T).IsAssignableFrom(dictionary[key].GetType())) ++count;
                    }
                }
            }
            return count;
        }
        public static string[] CollectKeys<T>(object value)
        {
            var keys = new System.Collections.Generic.List<string>();
            var dictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                foreach (string key in dictionary.Keys)
                {
                    if (!object.ReferenceEquals(null, dictionary[key]))
                    {
                        if (typeof(T).IsAssignableFrom(dictionary[key].GetType())) keys.Add(key);
                    }
                }
            }
            return keys.ToArray();
        }

        public static T[] Collect<T>(object value)
        {
            var items = new System.Collections.Generic.List<T>();
            var dictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                foreach (string key in dictionary.Keys)
                {
                    if (!object.ReferenceEquals(null, dictionary[key]))
                    {
                        if (typeof(T).IsAssignableFrom(dictionary[key].GetType())) items.Add((T)dictionary[key]);
                    }
                }
            }
            return items.ToArray();
        }

        public static int DeepCount<T>(object value)
        {
            var count = Count<T>(value);
            var dictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                foreach (string key in dictionary.Keys)
                {
                    if (!object.ReferenceEquals(null, dictionary[key]))
                    {
                        if (typeof(T).IsAssignableFrom(dictionary[key].GetType())) count += DeepCount<T>(dictionary[key]);
                    }
                }
            }
            return count;
        }

        public static T[] DeepCollect<T>(object value)
        {
            var items = new System.Collections.Generic.List<T>(Collect<T>(value));
            var dictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                foreach(var child in Collect<System.Collections.IDictionary>(dictionary))
                {
                    foreach (T item in DeepCollect<T>(child)) { items.Add(item); }
                }
            }
            return items.ToArray();
        }

        public static void Update(object value)
        {
            System.Type[] types = { typeof(bool) };
            var updateMethod = value.GetType().GetMethod(nameof(Update), types);
            if(!object.ReferenceEquals(null,updateMethod))
            {
                object[] parameters = { false };
                updateMethod.Invoke(value, parameters);
            }

            var dictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                foreach (System.Collections.IDictionary child in Collect<System.Collections.IDictionary>(dictionary))
                {
                    foreach (object item in Collect<object>(child)) { Update(item); }
                }
            }
        }
        private void Traverse(object value)
        {
            if(!object.ReferenceEquals(null,value))
            {
                if(value.GetType() != typeof(string))
                {
                    var ienumerable = Node.Net.Collections.KeyValuePair.GetValue(value) as System.Collections.IEnumerable;
                    if (!object.ReferenceEquals(null, ienumerable))
                    {
                        foreach (object item in ienumerable)
                        {
                            var itemValue = Node.Net.Collections.KeyValuePair.GetValue(item);
                            if (!metaData.ContainsKey(item)) metaData[itemValue] = new Node.Net.Deprecated.Collections.Hash();
                            metaData[itemValue]["Parent"] = Node.Net.Collections.KeyValuePair.GetValue(value);
                            Traverse(item);
                        }
                    }
                }
            }
        }

        public void SetDocument(object value,Document document)
        {
            if (!object.ReferenceEquals(null, value))
            {
                if (value.GetType() != typeof(string))
                {
                    var ienumerable = Node.Net.Collections.KeyValuePair.GetValue(value) as System.Collections.IEnumerable;
                    if (!object.ReferenceEquals(null, ienumerable))
                    {
                        foreach (object item in ienumerable)
                        {
                            var itemValue = Node.Net.Collections.KeyValuePair.GetValue(item);
                            var documentProperty = itemValue.GetType().GetProperty(nameof(Document));
                            if(!object.ReferenceEquals(null,documentProperty) && documentProperty.CanWrite)
                            {
                                documentProperty.SetValue(itemValue, document, null);
                            }
                            SetDocument(item,document);
                        }
                    }
                }
            }
        }
    }
}
