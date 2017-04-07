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
        public static string GetFullName(this object instance) => Beta.Internal.ObjectExtension.GetFullName(instance);
        public static void SetFullName(this object instance, string fullname) => Beta.Internal.ObjectExtension.SetFullName(instance, fullname);
        public static string GetName(this object instance) => Beta.Internal.ObjectExtension.GetName(instance);
    }
}
