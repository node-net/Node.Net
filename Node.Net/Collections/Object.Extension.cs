using System.Collections;

namespace Node.Net.Collections
{
    public static class ObjectExtension
    {
        public static object GetKey(object instance)
        {
            if(KeyValuePair.IsKeyValuePair(instance)) return KeyValuePair.GetKey(instance);
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

        public static string GetFullKey(object instance)
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
        public static object GetValue(object instance) => KeyValuePair.GetValue(instance);
        public static object GetParent(object item)
        {
            if (MetaDataMap.GetMetaDataFunction != null)
            {
                if (MetaDataMap.GetMetaDataFunction(item).ContainsKey("Parent"))
                {
                    return MetaDataMap.GetMetaDataFunction(item)["Parent"];
                }
            }
            return null;
        }
        public static void SetParent(object item, object parent)
        {
            if (item != null)
            {
                if (MetaDataMap.GetMetaDataFunction != null)
                {
                    MetaDataMap.GetMetaDataFunction(item)["Parent"] = parent;
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
    }
}
