using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            var depChildren = new List<DependencyObject>();
            var children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    depChildren.Add(child as DependencyObject);
                }
            }
            if (depChildren.Count == 0)
            {
                var contentControl = parent as ContentControl;
                if (contentControl != null)
                {
                    var depContent = contentControl.Content as DependencyObject;
                    if (depContent != null) depChildren.Add(depContent);
                }
            }

            foreach (var depChild in depChildren)
            {
                if (depChild is T)
                {
                    logicalChildren.Add(depChild as T);
                }
                CollectLogicalChildren(depChild, logicalChildren);
            }
        }
    }
}
