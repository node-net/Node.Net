using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factory.Internal
{
    static class IChildExtension
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
                return GetNearestAncestor<T>(child.Parent as IChildFactory);
            }
            return default(T);
        }

        public static T GetFurthestAncestor<T>(IChild child)
        {
            if (child != null)
            {
                var ancestor = GetNearestAncestor<T>(child);
                if (ancestor != null)
                {
                    var further_ancestor = GetFurthestAncestor<T>(ancestor as IChildFactory);
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

        public static IFactory GetRootAncestor(IChild child)
        {
            if (child != null)
            {
                return child.GetFurthestAncestor<IFactory>();
            }
            return null;
        }
    }
}
