using System.Collections;

namespace Node.Net.Factories.Extension
{
    public static class ObjectExtension
    {
        public static object GetParent(object item)
        {
            if(MetaDataMap.GetMetaDataFunction != null)
            {
                if(MetaDataMap.GetMetaDataFunction(item).ContainsKey("Parent"))
                {
                    return MetaDataMap.GetMetaDataFunction(item)["Parent"];
                }
            }
            return null;
        }

        public static void SetParent(object item, object parent)
        {
            if(item != null)
            {
                if(MetaDataMap.GetMetaDataFunction != null)
                {
                    MetaDataMap.GetMetaDataFunction(item)["Parent"] = parent;
                }
            }
        }

        public static T GetNearestAncestor<T>(object child)
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


        public static void UpdateParentBindings(object item)
        {
            var dictionary = item as IDictionary;
            if(dictionary != null)
            {
                foreach(var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if(child != null && typeof(IDictionary).IsAssignableFrom(child.GetType()))
                    {
                        SetParent(child, item);
                        UpdateParentBindings(child);
                    }
                }
            }
            else
            {
                var ienumerable = item as IEnumerable;
                if (ienumerable != null)
                {
                    foreach (var child in ienumerable)
                    {
                        if(child != null && typeof(IDictionary).IsAssignableFrom(child.GetType()))
                        {
                            SetParent(child, item);
                            UpdateParentBindings(child);
                        }
                    }
                }
            }
        }
    }
}
