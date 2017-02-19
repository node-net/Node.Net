using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls
{
    public static class ObjectExtension
    {
        public static object GetKey(object instance) => Internal.KeyValuePair.IsKeyValuePair(instance);
        public static object GetValue(object instance) => Internal.KeyValuePair.GetValue(instance);
        public static bool IsKeyValuePair(object instance) => Internal.KeyValuePair.IsKeyValuePair(instance);
    }
}
