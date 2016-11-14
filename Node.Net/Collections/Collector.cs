﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    public class Collector
    {
        #region State
        public Func<object, bool?> KeyFilter { get; set; }
        public Func<object, bool?> ValueFilter { get; set; }
        //public Func<object, bool?> Filter { get; set; }
        public Func<object, bool?> DeepFilter { get; set; }
        #endregion
        #region Construction
        //public Collector() { }
        /*
        public Collector(IFilter filter)
        {
            if(filter != null) Filter = filter.Include;
        }
        public Collector(IFilter filter,IFilter deepFilter)
        {
            if (filter != null) Filter = filter.Include;
            if (deepFilter != null) DeepFilter = deepFilter.Include;
        }*/
        #endregion

        

        #region CollectKeys
        public string[] CollectKeys(IDictionary dictionary,bool deep)
        {
            var keys = new List<string>();
            if (dictionary != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    if (IncludeKey(key) && !keys.Contains(key)) keys.Add(key);
                    if(deep)
                    {
                        var child = dictionary[key] as IDictionary;
                        if(child != null)
                        {
                            var child_keys = CollectKeys(child, deep);
                            foreach(var child_key in child_keys)
                            {
                                if (IncludeKey(child_key) && !keys.Contains(child_key)) keys.Add(child_key);
                            }
                        }
                    }
                }
            }
            return keys.ToArray();
        }
        
        #endregion

        #region Collect
        
        public Dictionary<string, T> Collect<T>(IDictionary dictionary,bool deep)
        {
            var children = new Dictionary<string, T>();
            if (dictionary != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    if (IncludeKey(key))
                    {
                        var child = dictionary[key];
                        if (IncludeValue<T>(child))// && Include<T>(child))
                        {
                            children.Add(key.ToString(), (T)child);
                        }
                    }
                    if(deep)
                    {
                        var child_dictonary = dictionary[key] as IDictionary;
                        if(child_dictonary != null)
                        {
                            if (DeepInclude(child_dictonary))
                            {
                                var deep_children = Collect<T>(child_dictonary,deep);
                                foreach (var child_key in deep_children.Keys)
                                {
                                    children.Add($"{key}/{child_key}", deep_children[child_key]);
                                }
                            }
                        }
                    }
                }
            }
            return children;
        }
        #endregion

        #region Internal
        private bool IncludeKey(string key)
        {
            if (KeyFilter == null || KeyFilter(key) == true)
            {
                return true;
            }
            return false;
        }
        private bool IncludeValue<T>(object value)
        {
            if (value == null) return false;
            if (!typeof(T).IsAssignableFrom(value.GetType())) return false;
            if (ValueFilter == null || ValueFilter(value) == true) return true;
            return false;
        }

        private bool DeepInclude(object value)
        {
            var dictionary = value as IDictionary;
            if (dictionary == null) return false;
            if (DeepFilter != null)
            {
                var include = DeepFilter(value);
                if (include.HasValue && !include.Value) return false;
            }
            return true;
        }
        #endregion




        #region Legacy
        /*
        public bool Include(object value)
        {
            if (Filter != null)
            {
                var include = Filter(value);
                if (include.HasValue && !include.Value) return false;
            }
            return true;
        }*/
        /*
public bool Include<T>(object value)
{
    if (value == null) return false;
    if (!typeof(T).IsAssignableFrom(value.GetType())) return false;
    return Include((T)value);
}*/
        /*
        public Dictionary<string, dynamic> Collect(IDictionary dictionary)
        {
            var children = new Dictionary<string, object>();
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if (Include(child)) children.Add(key.ToString(), child);
                }
            }
            return children;
        }*/
        /*
        public string[] DeepCollectKeys(IDictionary dictionary)
        {
            var keys = new List<string>();
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    if (key.GetType() == typeof(string))
                    {
                        if (!keys.Contains(key.ToString()))
                        {
                            keys.Add(key.ToString());
                        }
                    }
                    var child = dictionary[key] as IDictionary;
                    if (child != null)
                    {
                        var childKeys = DeepCollectKeys(child);
                        foreach (var child_key in childKeys)
                        {
                            if (!keys.Contains(child_key))
                            {
                                keys.Add(child_key);
                            }
                        }
                    }
                }
            }
            return keys.ToArray();
        }*/
        /*
        public Dictionary<string, dynamic> DeepCollect(IDictionary dictionary)
        {
            var results = Collect(dictionary);
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if (DeepInclude(child))
                    {
                        var children = DeepCollect(child as IDictionary);
                        foreach (var childKey in children.Keys)
                        {
                            results.Add($"{key}/{childKey}", children[childKey]);
                        }
                    }
                }
            }
            return results;
        }*/
        /*
        public Dictionary<string, T> DeepCollect<T>(IDictionary dictionary)
        {
            var results = Collect<T>(dictionary,false);
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    if (DeepInclude(dictionary[key]))
                    {
                        var children = DeepCollect<T>(dictionary[key] as IDictionary);
                        foreach (var childKey in children.Keys)
                        {
                            results.Add($"{key}/{childKey}", children[childKey]);
                        }
                    }
                }
            }
            return results;
        }*/
        #endregion
    }
}
