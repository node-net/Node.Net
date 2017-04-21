using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Node.Net.Beta.Internal.Factories
{
    class ChildrenFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (!typeof(IChildren).IsAssignableFrom(target_type)) return null;
            if (source != null)
            {
                var children = new List<object>();
                if(source is Panel)
                {
                    foreach(var item in (source as Panel).Children)
                    {
                        children.Add(item);
                    }
                }
                return children;
            }
    
            return null;
        }
    }
}
