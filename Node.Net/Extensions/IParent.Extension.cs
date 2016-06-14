using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Extensions
{
    public static class IParentExtension
    {
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
        }
    }
}
