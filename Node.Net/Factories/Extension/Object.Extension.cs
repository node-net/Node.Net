using System.Collections;

namespace Node.Net.Factories.Extension
{
    static class ObjectExtension
    {
        private static Internal.ParentMap parentMap = new Internal.ParentMap();
        public static object GetParent(object item)
        {
            return parentMap.GetParent(item);
        }

        public static void SetParent(object item, object parent)
        {
            parentMap.SetParent(item, parent);
        }

        public static T GetNearestAncestor<T>(object child)
        {
            var parent = child.GetParent();
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
                        }
                    }
                }
            }
        }
    }
}
