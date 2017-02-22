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
        public static void SetParent(this IDictionary dictionary, object parent) => Beta.Internal.IDictionaryExtension.SetParent(dictionary, parent);
        public static object GetRootAncestor(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetRootAncestor(child);
        public static T GetFurthestAncestor<T>(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetFurthestAncestor<T>(child);
        public static T GetNearestAncestor<T>(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetNearestAncestor<T>(child);
        public static string GetName(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetName(dictionary);
        public static string GetFullName(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetFullName(dictionary);
        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.ConvertTypes(source, types, typeKey);
        public static string GetTypeName(this IDictionary source, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.GetTypeName(source, typeKey);
        public static string GetJSON(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetJSON(dictionary);
        public static void DeepUpdateParents(this IDictionary source) => Beta.Internal.IDictionaryExtension.DeepUpdateParents(source);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToWorld(dictionary);
        public static Point3D GetWorldOrigin(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetWorldOrigin(dictionary);
    }
}
