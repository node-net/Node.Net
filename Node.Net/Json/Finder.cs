namespace Node.Net.Json
{
    public class Finder
    {
        public static object[] Find(System.Collections.IDictionary dictionary,string search)
        {
            System.Collections.Generic.List<object> results = new System.Collections.Generic.List<object>();
            foreach(object key in dictionary.Keys)
            {
                if (key.ToString().IndexOf(search) > -1) results.Add(key);
                else
                {
                    if (dictionary[key].ToString().IndexOf(search) > -1) results.Add(key);
                }
            }
            return results.ToArray();
        }

        public static object[] Find(System.Collections.IDictionary dictionary,IFilter filter)
        {
            System.Collections.Generic.List<object> results = new System.Collections.Generic.List<object>();
            foreach (object key in dictionary.Keys)
            {
                if (filter.Include(key)) results.Add(key);
                else
                {
                    if (filter.Include(dictionary[key])) results.Add(key);
                }
                //if (key.ToString().IndexOf(search) > -1) results.Add(key);
                //else
                //{
                //    if (dictionary[key].ToString().IndexOf(search) > -1) results.Add(key);
                //}
            }
            return results.ToArray();
        }

        public static object[] Collect(System.Collections.IDictionary dictionary,string search)
        {
            System.Collections.Generic.List<object> results = new System.Collections.Generic.List<object>();
            foreach (object key in dictionary.Keys)
            {
                if (key.ToString().IndexOf(search) > -1) results.Add(dictionary[key]);
                else
                {
                    if (dictionary[key].ToString().IndexOf(search) > -1) results.Add(dictionary[key]);
                }
            }
            return results.ToArray();
        }

        public static int[] Find(System.Collections.IEnumerable enumerable,string search)
        {
            System.Collections.Generic.List<int> results = new System.Collections.Generic.List<int>();
            int index = 0;
            foreach(object item in enumerable)
            {
                if (item.ToString().IndexOf(search) > -1) results.Add(index);
                else
                {
                    System.Collections.IDictionary idictionary = item as System.Collections.IDictionary;
                    if(!object.ReferenceEquals(null,idictionary) && Find(idictionary,search).Length > 0)
                    {
                        results.Add(index);
                    }
                }
                index++;
            }
            return results.ToArray();
        }

        public static object[] Collect(System.Collections.IEnumerable enumerable, string search)
        {
            System.Collections.Generic.List<object> results = new System.Collections.Generic.List<object>();
            int index = 0;
            foreach (object item in enumerable)
            {
                if (item.ToString().IndexOf(search) > -1) results.Add(item);
                else
                {
                    System.Collections.IDictionary idictionary = item as System.Collections.IDictionary;
                    if (!object.ReferenceEquals(null, idictionary) && Find(idictionary, search).Length > 0)
                    {
                        results.Add(item);
                    }
                }
                index++;
            }
            return results.ToArray();
        }

        public static object GetValue(System.Collections.IDictionary dictionary,object key)
        {
            if (object.ReferenceEquals(null, dictionary)) return null;
            if (dictionary.Contains(key)) return dictionary[key];
            if(!object.ReferenceEquals(null,key))
            {
                if(key.GetType()==typeof(string))
                {
                    System.Collections.Generic.List<string> words
                        = new System.Collections.Generic.List<string>(key.ToString().Split('/'));
                    if(words.Count > 1)
                    {
                        object value = GetValue(dictionary,words[0]);
                        System.Collections.IDictionary idict = KeyValuePair.GetValue(value) as System.Collections.IDictionary;
                        if(!object.ReferenceEquals(null,idict))
                        {
                            words.RemoveAt(0);
                            return GetValue(idict,System.String.Join("/",words.ToArray()));
                        }
                    }
                }
            }
            return null;
        }
    }
}
