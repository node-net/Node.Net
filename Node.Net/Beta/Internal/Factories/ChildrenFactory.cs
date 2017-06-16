using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Node.Net.Beta.Internal.Factories
{
    class Children : List<object>, IChildren { }
    class ChildrenFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (!typeof(IChildren).IsAssignableFrom(target_type)) return null;
            if (source != null)
            {
                var children = new Children();
                if (source is Panel)
                {
                    foreach (var item in (source as Panel).Children)
                    {
                        children.Add(item);
                    }
                }
                if (source is IDictionary)
                {
                    foreach (var key in (source as IDictionary).Keys)
                    {
                        var cdictionary = (source as IDictionary)[key] as IDictionary;
                        if (cdictionary != null) children.Add(cdictionary);
                    }
                }
                return children;
            }

            return null;
        }
    }
}
