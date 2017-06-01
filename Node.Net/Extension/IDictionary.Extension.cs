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
        public static IList Collect(this IDictionary dictionary, string type) => Beta.Internal.IDictionaryExtension.Collect(dictionary, type);
        public static IList Collect(this IDictionary dictionary, Type type) => Beta.Internal.IDictionaryExtension.Collect(dictionary, type);
        public static IList Collect(this IDictionary dictionary, Type type,string search) => Beta.Internal.IDictionaryExtension.Collect(dictionary, type,search);
        public static IList<T> Collect<T>(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.Collect<T>(dictionary);
        public static IList<T> Collect<T>(this IDictionary dictionary,string search) => Beta.Internal.IDictionaryExtension.Collect<T>(dictionary,search);
        public static IList<T> CollectValues<T>(this IDictionary dictionary, string key) => Beta.Internal.IDictionaryExtension.CollectValues<T>(dictionary,key);
        public static object GetParent(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetParent(dictionary);
        public static void SetParent(this IDictionary dictionary, object parent) => Beta.Internal.IDictionaryExtension.SetParent(dictionary, parent);
        public static object GetRootAncestor(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetRootAncestor(child);
        public static T GetFurthestAncestor<T>(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetFurthestAncestor<T>(child);
        public static T GetNearestAncestor<T>(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetNearestAncestor<T>(child);
        public static T Get<T>(this IDictionary dictionary, string name, T defaultValue = default(T), bool search = false) => Beta.Internal.IDictionaryExtension.Get<T>(dictionary, name, defaultValue,search);
        public static string GetName(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetName(dictionary);
        public static string GetFullName(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetFullName(dictionary);
        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.ConvertTypes(source, types, typeKey);
        public static string GetTypeName(this IDictionary source, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.GetTypeName(source, typeKey);
        public static string GetJSON(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetJSON(dictionary);
        public static void DeepUpdateParents(this IDictionary source) => Beta.Internal.IDictionaryExtension.DeepUpdateParents(source);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToWorld(dictionary);
        public static Matrix3D GetWorldToLocal(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetWorldToLocal(dictionary);
        public static Point3D GetWorldOrigin(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetWorldOrigin(dictionary);
        public static Vector3D GetWorldRotations(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetWorldRotations(dictionary);
        public static Point3D GetOrigin(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetOrigin(dictionary);
        public static Vector3D GetRotations(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetRotations(dictionary);
        public static double GetLengthMeters(this IDictionary dictionary, string name) => Beta.Internal.IDictionaryExtension.GetLengthMeters(dictionary, name);
        public static double GetAngleDegrees(this IDictionary dictionary, string name) => Beta.Internal.IDictionaryExtension.GetAngleDegrees(dictionary, name);
        public static IDictionary Copy(this IDictionary dictionary, IDictionary source) => Beta.Internal.IDictionaryExtension.Copy(dictionary, source);
        public static IDictionary Clone(this IDictionary source) => Beta.Internal.IDictionaryExtension.Clone(source);
        public static int ComputeHashCode(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.ComputeHashCode(dictionary);
        public static void Set(this IDictionary dictionary, string key, object value) => Beta.Internal.IDictionaryExtension.Set(dictionary, key, value);
        public static void Save(this IDictionary dictionary, string filename)
        {
            Node.Net.JSONWriter.Default.Write(filename, dictionary);
        }
    }
}
