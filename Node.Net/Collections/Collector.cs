﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    public class Collector
    {
        public Collector() { }
        public Collector(IFilter filter)
        {
            if(filter != null) Filter = filter.Include;
        }
        public Collector(IFilter filter,IFilter deepFilter)
        {
            if (filter != null) Filter = filter.Include;
            if (deepFilter != null) DeepFilter = deepFilter.Include;
        }
    
        public Func<object, bool?> Filter { get; set; }
        public Func<object,bool?> DeepFilter { get; set; }
        public bool Include(object value)
        {
            if (Filter != null)
            {
                var include = Filter(value);
                if (include.HasValue && !include.Value) return false;
            }
            return true;
        }
        public bool Include<T>(object value)
        {
            if (value == null) return false;
            if (!typeof(T).IsAssignableFrom(value.GetType())) return false;
            return Include((T)value);
        }
        public bool DeepInclude(object value)
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
        }
        public Dictionary<string, T> Collect<T>(IDictionary dictionary)
        {
            var children = new Dictionary<string, T>();
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if(Include<T>(child))
                    {
                        children.Add(key.ToString(), (T)child);
                    }
                }
            }
            return children;
        }
        public Dictionary<string, dynamic> DeepCollect(IDictionary dictionary)
        {
            var results = Collect(dictionary);
            if(dictionary != null)
            {
                foreach(var key in dictionary.Keys)
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
        }
        public Dictionary<string, T> DeepCollect<T>(IDictionary dictionary)
        {
            var results = Collect<T>(dictionary);
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
        }

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
        }
    }
}