using System.Collections.Generic;
using System.Windows;

namespace Node.Net
{
    public static class DependencyObjectExtension
    {
        public static IList<T> Collect<T>(this DependencyObject parent) where T : DependencyObject
        {
            var results = new List<T>();
            CollectLogicalChildren<T>(parent, results);
            return results;
        }

        private static void CollectLogicalChildren<T>(DependencyObject parent, List<T> logicalChildren) where T : DependencyObject
        {
            var children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    var depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalChildren.Add(child as T);
                    }
                    CollectLogicalChildren(depChild, logicalChildren);
                }
            }
        }
    }
}
