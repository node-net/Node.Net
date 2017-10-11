using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net
{
	public static class IDictionaryExtension
	{
		public static IList Collect(this IDictionary dictionary, string type) => Beta.Internal.IDictionaryExtension.Collect(dictionary, type);
		public static IList Collect(this IDictionary dictionary, Type type) => Beta.Internal.IDictionaryExtension.Collect(dictionary, type);
		public static IList Collect(this IDictionary dictionary, Type type, string search) => Beta.Internal.IDictionaryExtension.Collect(dictionary, type, search);
		public static IList<T> Collect<T>(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.Collect<T>(dictionary);
		public static IList<T> Collect<T>(this IDictionary dictionary, string search) => Beta.Internal.IDictionaryExtension.Collect<T>(dictionary, search);
		public static IList<T> Collect<T>(this IDictionary dictionary, Func<IDictionary, string, bool> matchFunction, string search = null) => Beta.Internal.IDictionaryExtension.Collect<T>(dictionary, matchFunction, search);
		public static IList<T> Collect<T>(this IDictionary dictionary, KeyValuePair<string, string> kvp) where T : IDictionary => Beta.Internal.IDictionaryExtension.Collect<T>(dictionary, kvp);
		public static IList<T> CollectValues<T>(this IDictionary dictionary, string key) => Beta.Internal.IDictionaryExtension.CollectValues<T>(dictionary, key);
		public static object GetParent(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetParent(dictionary);
		public static void SetParent(this IDictionary dictionary, object parent) => Beta.Internal.IDictionaryExtension.SetParent(dictionary, parent);
		public static object GetRootAncestor(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetRootAncestor(child);
		public static T GetFurthestAncestor<T>(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetFurthestAncestor<T>(child);
		public static T GetNearestAncestor<T>(this IDictionary child) => Beta.Internal.IDictionaryExtension.GetNearestAncestor<T>(child);
		public static IDictionary GetAncestor(this IDictionary child, string key, string value) => Beta.Internal.IDictionaryExtension.GetAncestor(child, key, value);
		public static T Get<T>(this IDictionary dictionary, string name, T defaultValue = default(T), bool search = false) => Beta.Internal.IDictionaryExtension.Get<T>(dictionary, name, defaultValue, search);
		//public static T Find<T>(this IDictionary dictionary, string name, bool exact = false) where T : IDictionary => Beta.Internal.IDictionaryExtension.Find<T>(dictionary, name, exact);
		public static T Find<T>(this IDictionary dictionary, string name, bool exact = false) => Beta.Internal.IDictionaryExtension.Find<T>(dictionary, name, exact);
		public static string GetName(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetName(dictionary);
		public static string GetFullName(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetFullName(dictionary);
		public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.ConvertTypes(source, types, typeKey);
		public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, Type defaultType,string typeKey = "Type") => Beta.Internal.IDictionaryExtension.ConvertTypes(source, types,defaultType, typeKey);
		public static string GetTypeName(this IDictionary source, string typeKey = "Type") => Beta.Internal.IDictionaryExtension.GetTypeName(source, typeKey);
		public static string GetJSON(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetJSON(dictionary);
		public static void DeepUpdateParents(this IDictionary source) => Beta.Internal.IDictionaryExtension.DeepUpdateParents(source);
		public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToParent(dictionary);
		public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetLocalToWorld(dictionary);
		public static Matrix3D GetWorldToLocal(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetWorldToLocal(dictionary);
		public static Point3D GetWorldOrigin(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetWorldOrigin(dictionary);
		public static void SetWorldOrigin(this IDictionary dictionary, Point3D worldOrigin) => Beta.Internal.IDictionaryExtension.SetWorldOrigin(dictionary, worldOrigin);
		public static Vector3D GetWorldRotations(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetWorldRotations(dictionary);
		public static Point3D GetOrigin(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetOrigin(dictionary);
		public static void SetOrigin(this IDictionary dictionary, Point3D origin) => Beta.Internal.IDictionaryExtension.SetOrigin(dictionary, origin);
		public static Vector3D GetRotations(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.GetRotations(dictionary);
		public static double GetLengthMeters(this IDictionary dictionary, string name) => Beta.Internal.IDictionaryExtension.GetLengthMeters(dictionary, name);
		public static double GetAngleDegrees(this IDictionary dictionary, string name) => Beta.Internal.IDictionaryExtension.GetAngleDegrees(dictionary, name);
		public static IDictionary Copy(this IDictionary dictionary, IDictionary source) => Beta.Internal.IDictionaryExtension.Copy(dictionary, source);
		public static IDictionary Copy(this IDictionary dictionary, IDictionary source, Func<object, bool> valueFilterFunction, Func<object, bool> keyFilterFunction = null) => Beta.Internal.IDictionaryExtension.Copy(dictionary, source, valueFilterFunction,keyFilterFunction);
		public static IDictionary Clone(this IDictionary source) => Beta.Internal.IDictionaryExtension.Clone(source);
		public static int ComputeHashCode(this IDictionary dictionary) => Beta.Internal.IDictionaryExtension.ComputeHashCode(dictionary);
		public static int CompareTo(this IDictionary a, IDictionary b) => Beta.Internal.IDictionaryExtension.CompareTo(a, b);
		public static void Set(this IDictionary dictionary, string key, object value) => Beta.Internal.IDictionaryExtension.Set(dictionary, key, value);
		public static void Save(this IDictionary dictionary, string filename)
		{
			Node.Net.JSONWriter.Default.Write(filename, dictionary);
		}
		public static void Save(this IDictionary dictionary,Stream stream)
		{
			Node.Net.JSONWriter.Default.Write(stream, dictionary);
		}
		public static T GetCurrent<T>(this IDictionary dictionary) where T : IDictionary => Beta.Internal.IDictionaryExtension.GetCurrent<T>(dictionary);
		public static void SetCurrent<T>(this IDictionary dictionary, string name) where T : IDictionary => Beta.Internal.IDictionaryExtension.SetCurrent<T>(dictionary, name);
	}
}
