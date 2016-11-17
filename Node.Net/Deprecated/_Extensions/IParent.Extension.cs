using System.Collections.Generic;

namespace Node.Net.Extensions
{
    public static class IParentExtension
    {
        public static Dictionary<string,T> Collect<T>(IParent parent)
        {
            var children = new Dictionary<string,T>();
            if (parent != null)
            {
                var all = parent.GetChildren();
                foreach (var child_key in all.Keys)
                {
                    var child = all[child_key];
                    if (child != null)
                    {
                        if (typeof(T).IsAssignableFrom(child.GetType()))
                        {
                            var instance = (T)all[child_key];
                            if (instance != null)
                            {
                                children.Add(child_key, instance);
                            }
                        }
                    }
                }
            }
            return children;
        }

        public static Dictionary<string,T> DeepCollect<T>(IParent parent)
        {
            var children = new Dictionary<string,T>();
            if (parent != null)
            {
                var all = parent.GetChildren();
                foreach (var child_key in all.Keys)
                {
                    var child = all[child_key];
                    if (child != null)
                    {
                        if (typeof(T).IsAssignableFrom(child.GetType()))
                        {
                            var instance = (T)all[child_key];
                            if (instance != null)
                            {
                                children.Add(child_key, instance);
                            }
                        }

                        var deep_children = DeepCollect<T>(child as IParent);
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
    }
}
