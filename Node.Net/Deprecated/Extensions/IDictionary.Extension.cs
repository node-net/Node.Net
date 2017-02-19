using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated
{
    public static class IDictionaryExtension
    {
        public static string GetLengthAsString(this IDictionary dictionary, string name) => Deprecated.Measurement.IDictionaryExtension.GetLengthAsString(dictionary, name);
        public static double GetLengthMeters(this IDictionary dictionary, string name) => Deprecated.Measurement.IDictionaryExtension.GetLengthMeters(dictionary, name);
        public static void SetLength(this IDictionary dictionary, string name, string value) => Deprecated.Measurement.IDictionaryExtension.SetLength(dictionary, name, value);
        public static void SetLength(this IDictionary dictionary, string name, double length_meters) => Deprecated.Measurement.IDictionaryExtension.SetLength(dictionary, name, length_meters);
        public static string GetAngleAsString(this IDictionary dictionary, string name) => Deprecated.Measurement.IDictionaryExtension.GetAngleAsString(dictionary, name);
        public static double GetAngleDegrees(this IDictionary dictionary, string name) => Deprecated.Measurement.IDictionaryExtension.GetAngleDegrees(dictionary, name);
        public static void SetAngle(this IDictionary dictionary, string name, string value) => Deprecated.Measurement.IDictionaryExtension.SetAngle(dictionary, name, value);
        public static void SetAngle(this IDictionary dictionary, string name, double angle_degrees) => Deprecated.Measurement.IDictionaryExtension.SetAngle(dictionary, name, angle_degrees);
        //////////////////////////////////////////////////////////////////
        /// Collections
        public static IList Collect(this IDictionary dictionary, string typeName) => Collections.IDictionaryExtension.Collect(dictionary, typeName);
        public static IList Collect(this IDictionary dictionary, Type type) => Collections.IDictionaryExtension.Collect(dictionary, type);
        public static string[] CollectKeys(this IDictionary dictionary) => Collections.IDictionaryExtension.CollectKeys(dictionary);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary,
                                                      Func<object, bool?> valueFilter = null,
                                                      Func<object, bool?> keyFilter = null,
                                                      Func<object, bool?> deepFilter = null,
                                                      bool deep = true) => Collections.IDictionaryExtension.Collect<T>(dictionary, valueFilter, keyFilter, deepFilter, deep);
        public static Dictionary<string, dynamic> Collect(this IDictionary dictionary,
                                                       Func<object, bool?> valueFilter = null,
                                                       Func<object, bool?> keyFilter = null,
                                                       Func<object, bool?> deepFilter = null,
                                                       bool deep = true) => Collections.IDictionaryExtension.Collect(dictionary, valueFilter, keyFilter, deepFilter, deep);
        public static T[] CollectValues<T>(this IDictionary dictionary, string key) => Collections.IDictionaryExtension.CollectValues<T>(dictionary, key);
        public static Type[] CollectTypes(this IDictionary dictionary) => Collections.IDictionaryExtension.CollectTypes(dictionary);
        public static T Get<T>(this IDictionary dictionary, string name) => Collections.IDictionaryExtension.Get<T>(dictionary, name);
        public static void Set(this IDictionary dictionary, string name, object value) => Collections.IDictionaryExtension.Set(dictionary, name, value);
        public static void RemoveKeys(this IDictionary dictionary, string[] keys) => Collections.IDictionaryExtension.RemoveKeys(dictionary, keys);
        public static void DeepUpdateParents(this IDictionary dictionary) => Collections.IDictionaryExtension.DeepUpdateParents(dictionary);
        public static void Remove<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.Remove<T>(dictionary);
        public static void DeepRemove<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.DeepRemove<T>(dictionary);
        public static void Copy(this IDictionary destination, IDictionary source) => Collections.IDictionaryExtension.Copy(destination, source);
        //public static T Find<T>(this IDictionary dictionary, string key) => Collections.IDictionaryExtension.Find<T>(dictionary, key);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Deprecated.Factories.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Deprecated.Factories.IDictionaryExtension.GetLocalToWorld(dictionary);
        public static Point3D GetWorldOrigin(this IDictionary dictionary) => Deprecated.Factories.IDictionaryExtension.GetWorldOrigin(dictionary);

        //////////////////////////////////////////////////////////////////
        /// Readers
        //public static object ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type") => Readers.IDictionaryExtension.ConvertTypes(source, types, typeKey);
        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            if (source == null) return null;

            var temp = new Dictionary<string, dynamic>();
            foreach (var key in source.Keys)
            {
                var value = source[key];
                var childDictionary = value as IDictionary;
                if (childDictionary != null)
                {
                    temp.Add(key.ToString(), ConvertTypes(childDictionary, types, typeKey));
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        temp.Add(key.ToString(), childEnumerable.ConvertTypes(types, typeKey));
                    }
                    else
                    {
                        temp.Add(key.ToString(), value);
                    }
                }
            }

            IDictionary copy = new Dictionary<string, dynamic>();//new Internal.Element(temp);
            if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");
            var typename = source.Get<string>(typeKey);// source.GetTypeName(typeKey);
            if (typename.Length > 0 && types.ContainsKey(typename))
            {
                var targetType = types[typename];
                if (targetType == null) throw new Exception($"types['{typename}'] was null");
                if (source.GetType() != targetType)
                {
                    object[] args = { temp };
                    copy = Activator.CreateInstance(targetType, args) as IDictionary;
                    if (copy == null) throw new Exception($"failed to create instance of type {targetType.FullName}");
                }
            }

            return copy;
        }
        public static string GetJSON(this IDictionary source)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                var writer = new Node.Net.Deprecated.Writers.JsonWriter { Format = Deprecated.Writers.JsonFormat.Indented };
                writer.Write(memory, source);
                memory.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(memory))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        
    }
}
