using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class IDictionaryExtension
    {
        public static IList Collect(this IDictionary dictionary, Type type) => Beta.Internal.IDictionaryExtension.Collect(dictionary, type);
        public static object GetParent(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetParent(dictionary);
        public static string GetName(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetName(dictionary);
        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.ConvertTypes(source, types, typeKey);
        public static string GetTypeName(this IDictionary source, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.GetTypeName(source, typeKey);
        public static void DeepUpdateParents(this IDictionary source) => Beta.Internal.IDictionaryExtension.DeepUpdateParents(source);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToWorld(dictionary);
    }
}
