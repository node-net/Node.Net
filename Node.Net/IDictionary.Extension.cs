﻿//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class IDictionaryExtension
    {
        public static string GetLengthAsString(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetLengthAsString(dictionary, name);
        public static double GetLengthMeters(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetLengthMeters(dictionary, name);
        public static void SetLength(this IDictionary dictionary, string name,string value) => Measurement.IDictionaryExtension.SetLength(dictionary, name,value);
        public static void SetLength(this IDictionary dictionary, string name, double length_meters) => Measurement.IDictionaryExtension.SetLength(dictionary, name, length_meters);
        public static string GetAngleAsString(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetAngleAsString(dictionary, name);
        public static double GetAngleDegrees(this IDictionary dictionary, string name) => Measurement.IDictionaryExtension.GetAngleDegrees(dictionary, name);
        public static void SetAngle(this IDictionary dictionary, string name, string value) => Measurement.IDictionaryExtension.SetAngle(dictionary, name, value);
        public static void SetAngle(this IDictionary dictionary, string name, double angle_degrees) => Measurement.IDictionaryExtension.SetAngle(dictionary, name, angle_degrees);
        //////////////////////////////////////////////////////////////////
        /// Collections
        public static string[] CollectKeys(this IDictionary dictionary) => Collections.IDictionaryExtension.CollectKeys(dictionary);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary,
                                                      Func<object, bool?> valueFilter = null,
                                                      Func<object, bool?> keyFilter = null,
                                                      Func<object, bool?> deepFilter = null,
                                                      bool deep = true) => Collections.IDictionaryExtension.Collect<T>(dictionary, valueFilter, keyFilter, deepFilter, deep);
        public static Dictionary<string, IDictionary> Collect(this IDictionary dictionary,
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
        public static T Find<T>(this IDictionary dictionary, string key) => Collections.IDictionaryExtension.Find<T>(dictionary, key);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Factories.Extension.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Factories.Extension.IDictionaryExtension.GetLocalToWorld(dictionary);

        //////////////////////////////////////////////////////////////////
        /// Readers
        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type") => Readers.IDictionaryExtension.ConvertTypes(source, types, typeKey);

    }
}
