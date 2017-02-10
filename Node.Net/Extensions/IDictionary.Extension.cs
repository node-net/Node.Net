//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class IDictionaryExtension
    {
        public static string GetLengthAsString(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetLengthAsString(dictionary, name);
        public static double GetLengthMeters(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetLengthMeters(dictionary, name);
        public static void SetLength(this IDictionary dictionary, string name, string value) => Measurement.IDictionaryExtension.SetLength(dictionary, name, value);
        public static void SetLength(this IDictionary dictionary, string name, double length_meters) => Measurement.IDictionaryExtension.SetLength(dictionary, name, length_meters);
        public static string GetAngleAsString(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetAngleAsString(dictionary, name);
        public static double GetAngleDegrees(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetAngleDegrees(dictionary, name);
        public static void SetAngle(this IDictionary dictionary, string name, string value) => Measurement.IDictionaryExtension.SetAngle(dictionary, name, value);
        public static void SetAngle(this IDictionary dictionary, string name, double angle_degrees) => Measurement.IDictionaryExtension.SetAngle(dictionary, name, angle_degrees);
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
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Factories.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Factories.IDictionaryExtension.GetLocalToWorld(dictionary);
        public static Point3D GetWorldOrigin(this IDictionary dictionary) => Factories.IDictionaryExtension.GetWorldOrigin(dictionary);

        //////////////////////////////////////////////////////////////////
        /// Readers
        public static object ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type") => Readers.IDictionaryExtension.ConvertTypes(source, types, typeKey);

        public static string GetJSON(this IDictionary source)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                var writer = new Node.Net.Writers.JsonWriter { Format = Writers.JsonFormat.Indented };
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
