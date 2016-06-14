using System.Collections.Generic;

namespace Node.Net.Extensions
{
    public static class IParentExtension
    {
        public static T[] Collect<T>(IParent parent)
        {
            var children = new List<T>();
            if (parent != null)
            {
                foreach (var child in parent.GetChildren())
                {
                    var instance = (T)child;
                    if (instance != null)
                    {
                        children.Add(instance);
                    }
                }
            }
            return children.ToArray();
        }

        public static T[] DeepCollect<T>(IParent parent)
        {
            var children = new List<T>();
            if (parent != null)
            {
                foreach (var child in parent.GetChildren())
                {
                    var instance = (T)child;
                    if (instance != null)
                    {
                        children.Add(instance);
                    }

                    var deep_children = DeepCollect<T>(child as IParent);
                    foreach (var deep_child in deep_children)
                    {
                        children.Add(deep_child);
                    }
                }
            }
            return children.ToArray();
        }
        /*
        public static void Update(IParent parent)
        {
            if (parent == null) return;
            foreach(var child in parent.GetChildren())
            {
                child.Parent = parent;
            }
        }
        public static void DeepUpdate(IParent parent)
        {
            if (parent == null) return;
            foreach (var child in parent.GetChildren())
            {
                child.Parent = parent;
                DeepUpdate(child as IParent);
            }
        }*/
    }
}
