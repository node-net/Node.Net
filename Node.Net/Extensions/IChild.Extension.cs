using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Node.Net.Extensions
{
    public static class IChildExtension
    {
        public static T GetNearestAncestor<T>(IChild child)
        {
            if (child != null && child.Parent != null)
            {
                if (typeof(T).IsAssignableFrom(child.Parent.GetType()))
                {
                    var ancestor = (T)child.Parent;
                    if (ancestor != null) return ancestor;
                }
                return GetNearestAncestor<T>(child.Parent as IChild);
            }
            return default(T);
        }

        public static T GetFurthestAncestor<T>(IChild child)
        {
            if (child != null)
            {
                var ancestor = GetNearestAncestor<T>(child);
                if(ancestor != null)
                {
                    var further_ancestor = GetFurthestAncestor<T>(ancestor as IChild);
                    if (further_ancestor != null) return further_ancestor;
                }
                if(ancestor == null)
                {
                    if(typeof(T).IsAssignableFrom(child.GetType()))
                    {
                        ancestor = (T)child;
                    }
                }
                return ancestor;
            }
            return default(T);
        }

        public static IParent GetRootAncestor(IChild child)
        {
            if (child != null)
            {
                return child.GetFurthestAncestor<IParent>();
            }
            return null;
        }

        public static Dictionary<string, T> LocateAll<T>(IChild child)
        {
            var root = GetRootAncestor(child);
            if(root != null)
            {
                return root.DeepCollect<T>();
            }
            var parent = child as IParent;
            if(parent != null)
            {
                return parent.DeepCollect<T>();
            }
            return new Dictionary<string, T>();
        }


        public static T LocateFirst<T>(IChild child)
        {
            var dictionary = LocateAll<T>(child);
            if (dictionary.Count > 0)
            {
                foreach(var key in dictionary.Keys)
                {
                    return dictionary[key];
                }
            }
            return default(T);
        }

        public static string GetKey(IChild child)
        {
            var parentDictionary = child.Parent as IDictionary;
            if(parentDictionary != null)
            {
                foreach(var key in parentDictionary.Keys)
                {
                    if(object.ReferenceEquals(child,parentDictionary[key]))
                    {
                        return key.ToString();
                    }
                }
            }
            return "";
        }

        public static string GetFullKey(IChild child)
        {
            var parent = child.Parent;
            if (parent != null)
            {
                var parent_as_child = parent as IChild;
                if(parent_as_child != null)
                {
                    var parent_full_key = GetFullKey(parent_as_child);
                    if(parent_full_key.Length > 0)
                    {
                        return $"{parent_full_key}/{GetKey(child)}";
                    }
                }
            }
            return GetKey(child);
        }
    }
}
