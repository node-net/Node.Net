﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Collections
{
    public static class IDictionaryExtension
    {
        public static Func<IDictionary, Matrix3D> GetLocalToParentFunction;
        public static Func<IDictionary, Matrix3D> GetLocalToWorldFunction;
        public static Matrix3D GetLocalToParent(IDictionary dictionary)
        {
            if (GetLocalToParentFunction != null) return GetLocalToParentFunction(dictionary);
            return new Matrix3D();
        }
        public static Matrix3D GetLocalToWorld(IDictionary dictionary)
        {
            if (GetLocalToWorldFunction != null) return GetLocalToWorldFunction(dictionary);
            return new Matrix3D();
        }
        public static T Get<T>(IDictionary dictionary, string name)
        {
            if (!dictionary.Contains(name)) return default(T);
            var value = dictionary[name];
            if (value == null) return default(T);

            if (typeof(T) == typeof(DateTime) && value.GetType() == typeof(string))
            {
                return (T)((object)DateTime.Parse(value.ToString()));
            }
            return (T)dictionary[name];

        }

        public static void Set(IDictionary dictionary, string key, object value)
        {
            if (value != null && value.GetType() == typeof(DateTime))
            {
                dictionary[key] = ((DateTime)value).ToString("o");
            }
            else { dictionary[key] = value; }
        }

        public static void RemoveKey(IDictionary dictionary,string key)
        {
            if(key.Contains("/"))
            {

            }
            else
            {
                dictionary.Remove(key);
            }
        }
        public static void RemoveKeys(IDictionary dictionary,string[] keys)
        {
            foreach (var key in keys) { RemoveKey(dictionary, key); }
        }

        public static Dictionary<string, T> Collect<T>(IDictionary dictionary, IFilter filter = null)
        {
            var children = new Dictionary<string, T>();
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if (child != null)
                    {
                        SetParent(child as IDictionary, dictionary);
                        if (typeof(T).IsAssignableFrom(child.GetType()))
                        {
                            var instance = (T)child;
                            if (instance != null)
                            {
                                if (filter == null || filter.Include(instance))
                                {
                                    children.Add(key.ToString(), instance);
                                }
                            }
                        }
                    }
                }
            }
            return children;
        }

        public static void Remove<T>(IDictionary dictionary)
        {
            if (dictionary != null)
            {
                var remove_keys = new List<string>();
                foreach (var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if (child != null)
                    {
                        //SetParent(child as IDictionary, dictionary);
                        if (typeof(T).IsAssignableFrom(child.GetType()))
                        {

                            var instance = (T)child;
                            if (instance != null)
                            {
                                remove_keys.Add(key.ToString());
                            }
                        }
                    }
                }
                foreach(var remove_key in remove_keys)
                {
                    dictionary.Remove(remove_key);
                }
            }
        }

        public static void DeepRemove<T>(IDictionary dictionary)
        {
            Remove<T>(dictionary);
            //dictionary.Remove<T>();
            foreach(var key in dictionary.Keys)
            {
                var child_dictionary = dictionary[key] as IDictionary;
                if(child_dictionary != null)
                {
                    DeepRemove<T>(child_dictionary);
                    //child_dictionary.DeepRemove<T>();
                }
            }
        }

        public static Dictionary<string, T> DeepCollect<T>(IDictionary dictionary, IFilter filter = null)
        {
            var children = new Dictionary<string, T>();
            if (dictionary != null)
            {
                foreach (var child_key in dictionary.Keys)
                {
                    var child = dictionary[child_key];
                    if (child != null)
                    {
                        SetParent(child as IDictionary, dictionary);

                        if (typeof(T).IsAssignableFrom(child.GetType()))
                        {
                            var instance = (T)child;
                            if (instance != null)
                            {
                                if (filter == null || filter.Include(instance))
                                {
                                    children.Add(child_key.ToString(), instance);
                                }
                            }
                        }

                        var deep_children = DeepCollect<T>(child as IDictionary, filter);
                        foreach (var deep_child_key in deep_children.Keys)
                        {
                            var deep_child = deep_children[deep_child_key];
                            children.Add($"{child_key}/{deep_child_key}", deep_child);
                        }
                    }
                }
            }
            return children;
        }

        public static string[] CollectUniqueStrings(IDictionary dictionary, string key)
        {
            var results = new List<string>();
            if (dictionary.Contains(key))
            {
                var value = dictionary[key];
                if (value != null)
                {
                    if (!results.Contains(value.ToString()))
                    {
                        results.Add(value.ToString());
                    }
                }
            }
            foreach (var child_key in dictionary.Keys)
            {
                var child_dictionary = dictionary[child_key] as IDictionary;
                if (child_dictionary != null)
                {
                    foreach (var value in CollectUniqueStrings(child_dictionary, key))
                    {
                        if (!results.Contains(value)) results.Add(value);
                    }
                }
            }
            return results.ToArray();
        }


        public static object GetParent(IDictionary dictionary)
        {
            if(MetaDataMap.GetMetaDataFunction != null)
            {
                if(MetaDataMap.GetMetaDataFunction(dictionary).ContainsKey("Parent"))
                {
                    return MetaDataMap.GetMetaDataFunction(dictionary)["Parent"];
                }
            }
            return null;
            /*
            var parentProperty = dictionary.GetType().GetProperty("Parent");
            if (parentProperty != null)
            {
                return parentProperty.GetValue(dictionary);
            }
            else
            {

                return parentMap.GetParent(dictionary);
            }
            return null;
            */
        }

        public static void SetParent(IDictionary dictionary, object parent)
        {
            if (dictionary != null)
            {
                if(MetaDataMap.GetMetaDataFunction != null)
                {
                    MetaDataMap.GetMetaDataFunction(dictionary)["Parent"] = parent;
                }
                /*
                var parentProperty = dictionary.GetType().GetProperty("Parent");
                if (parentProperty != null)
                {
                    parentProperty.SetValue(dictionary, parent);
                }
                else
                {
                    parentMap.SetParent(dictionary, parent);
                }*/
            }
        }

        public static void Copy(IDictionary destination, IDictionary source)
        {
            destination.Clear();
            foreach (string key in source.Keys)
            {
                destination[key] = CopyChild(source[key]);
            }
        }

        public static object CopyChild(object instance)
        {
            var copy = instance;
            var child_dictionary = instance as IDictionary;
            var child_enumerable = instance as IEnumerable;
            if (child_dictionary != null)
            {
                var new_child_dictionary = Activator.CreateInstance(child_dictionary.GetType()) as IDictionary;
                IDictionaryExtension.Copy(new_child_dictionary, child_dictionary);
                //new_child_dictionary.Copy(child_dictionary);
                copy = new_child_dictionary;
            }
            else if (child_enumerable != null && child_enumerable.GetType() != typeof(string))
            {
                var new_child_list = new List<dynamic>();
                Copy(new_child_list, child_enumerable);
                copy = new_child_list;
            }
            return copy;
        }

        public static void Copy(IList destination, IEnumerable source)
        {
            foreach (var child in source)
            {
                destination.Add(CopyChild(child));
            }
        }

        public static T GetNearestAncestor<T>(IDictionary child)
        {
            var parent = GetParent(child);
            if (child != null && parent != null)
            {
                if (typeof(T).IsAssignableFrom(parent.GetType()))
                {
                    var ancestor = (T)parent;
                    if (ancestor != null) return ancestor;
                }
                return GetNearestAncestor<T>(parent as IDictionary);
            }
            return default(T);
        }
        public static T GetFurthestAncestor<T>(IDictionary child)
        {
            if (child != null)
            {
                var ancestor = GetNearestAncestor<T>(child);
                if (ancestor != null)
                {
                    var further_ancestor = GetFurthestAncestor<T>(ancestor as IDictionary);
                    if (further_ancestor != null) return further_ancestor;
                }
                if (ancestor == null)
                {
                    if (typeof(T).IsAssignableFrom(child.GetType()))
                    {
                        ancestor = (T)child;
                    }
                }
                return ancestor;
            }
            return default(T);
        }
        public static IDictionary GetRootAncestor(IDictionary child)
        {
            return GetFurthestAncestor<IDictionary>(child);
            //return child.GetFurthestAncestor<IDictionary>();
        }

        public static T Find<T>(IDictionary dictionary,string key)
        {
            var items = DeepCollect<T>(dictionary);
            foreach(var child_key in items.Keys)
            {
                if (child_key == key) return items[child_key];
            }

            foreach (var child_key in items.Keys)
            {
                if (child_key.Contains(key)) return items[child_key];
            }

            return default(T);
        }
    }
}
