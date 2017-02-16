using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    public static class ObjectExtension
    {
        public static object GetKey(this object instance)
        {
            if(Internal.KeyValuePair.IsKeyValuePair(instance)) return Internal.KeyValuePair.GetKey(instance);
            var parent = GetParent(instance) as IDictionary;
            if(parent != null)
            {
                foreach(var key in parent.Keys)
                {
                    if (object.ReferenceEquals(parent[key], instance)) return key;
                }
            }
            return null;
        }
        public static string GetFullKey(this object instance)
        {
            var key = GetKey(instance);
            if (key != null)
            {
                var parent = GetParent(instance);
                if (parent != null)
                {
                    var parent_full_key = GetFullKey(parent);
                    if (parent_full_key.Length > 0)
                    {
                        return $"{parent_full_key}/{key.ToString()}";
                    }
                }
                return key.ToString();
            }
            return string.Empty;
        }
        public static string GetName(this object instance)
        {
            var key = GetKey(instance);
            if (key != null) return key.ToString();
            return string.Empty;
        }
        public static string GetFullName(this object instance) => GetFullKey(instance);
        public static object GetValue(object instance) => Internal.KeyValuePair.GetValue(instance);
        public static bool IsKeyValuePair(object instance) => Internal.KeyValuePair.IsKeyValuePair(instance);
        //public static Func<object, Dictionary<string, dynamic>> MetaDataFunctionGetMetaDataFunction = Default.GetMetaData;
        public static object GetParent(this object item)
        {
            var element = item as IElement;
            if (element != null) return element.Parent;
            if (GlobalFunctions.GetMetaDataFunction != null)
            {
                if (GlobalFunctions.GetMetaDataFunction(item).ContainsKey("Parent"))
                {
                    return GlobalFunctions.GetMetaDataFunction(item)["Parent"];
                }
            }
            return null;
        }
        public static void SetParent(this object item, object parent)
        {
            var element = item as IElement;
            if (element != null) element.Parent = parent;
            else
            {
                if (item != null)
                {
                    if (GlobalFunctions.GetMetaDataFunction != null)
                    {
                        GlobalFunctions.GetMetaDataFunction(item)["Parent"] = parent;
                    }
                }
            }
        }

        public static T GetNearestAncestor<T>(object child)
        {
            var parent = ObjectExtension.GetParent(child);
            if (child != null && parent != null)
            {
                if (typeof(T).IsAssignableFrom(parent.GetType()))
                {
                    var ancestor = (T)parent;
                    if (ancestor != null) return ancestor;
                }
                return GetNearestAncestor<T>(parent);
            }
            return default(T);
        }
        public static T GetFurthestAncestor<T>(object child)
        {
            if (child != null)
            {
                var ancestor = GetNearestAncestor<T>(child);
                if (ancestor != null)
                {
                    var further_ancestor = GetFurthestAncestor<T>(ancestor);
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
        public static object GetRootAncestor(object child)
        {
            return GetFurthestAncestor<object>(child);

        }
        public static Dictionary<string, dynamic> GetAncestors(object child)
        {
            var ancestors = new Dictionary<string, dynamic>();
            var parent = ObjectExtension.GetParent(child);
            if (parent != null)
            {
                ancestors.Add(ObjectExtension.GetFullKey(parent), parent);
                var parent_ancestors = GetAncestors(parent);
                foreach(var key in parent_ancestors.Keys)
                {
                    if (!ancestors.ContainsKey(key))
                    {
                        ancestors.Add(key, parent_ancestors[key]);
                    }
                }
            }
            //public static object GetParent(object item)
            //public static T GetNearestAncestor<T>(object child)

            return ancestors;
        }
    }
}
