using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class ObjectExtension
    {
        public static object GetValue(this object instance) => Beta.Internal.ObjectExtension.GetValue(instance);
        public static bool IsKeyValuePair(this object instance) => Beta.Internal.ObjectExtension.IsKeyValuePair(instance);
    }
}
