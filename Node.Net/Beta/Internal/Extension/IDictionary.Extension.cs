using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal
{
    static class IDictionaryExtension
    {
        public static string GetJSON(this IDictionary dictionary) { return new JSONWriter().WriteToString(dictionary); }// return Writers.JSONWriter.Default.WriteToString(dictionary); }
        public static void Save(this IDictionary dictionary, Stream stream) { new JSONWriter().Write(stream, dictionary); }
        public static void SetParent(this IDictionary dictionary, object parent) { Internal.Collections.MetaData.Default.GetMetaData(dictionary)["Parent"] = parent; }
        public static object GetParent(this IDictionary dictionary) { return Internal.Collections.MetaData.Default.GetMetaData(dictionary, "Parent"); }
        public static void SetFileName(this IDictionary dictionary,string filename) { Internal.Collections.MetaData.Default.GetMetaData(dictionary)["FileName"] = filename; }
        public static string GetFileName(this IDictionary dictionary) { return Internal.Collections.MetaData.Default.GetMetaData<string>(dictionary, "FileName"); }
        public static void DeepUpdateParents(this IDictionary dictionary)
        {
            foreach (var value in dictionary.Values)
            {
                var child = value as IDictionary;
                if (child != null)
                {
                    child.SetParent(dictionary); DeepUpdateParents(child);
                }
            }
        }
        public static double GetLengthMeters(this IDictionary dictionary,string name)
        {
            var svalue = dictionary.Get<string>(name);
            return Units.Length.GetMeters(svalue);
        }
        public static int ComputeHashCode(this IDictionary dictionary)
        {
            var hashCode = dictionary.Count;
            foreach (var key in dictionary.Keys)
            {
                hashCode = hashCode ^ key.GetHashCode();
                var value = dictionary[key];
                if (value != null)
                {
                    if (value.GetType() == typeof(bool) ||
                       value.GetType() == typeof(double) ||
                       value.GetType() == typeof(float) ||
                       value.GetType() == typeof(int) ||
                       value.GetType() == typeof(long) ||
                       value.GetType() == typeof(string))
                    {
                        hashCode = hashCode ^ value.GetHashCode();
                    }
                    else
                    {
                        if (typeof(IDictionary).IsAssignableFrom(value.GetType())) hashCode = hashCode ^ (value as IDictionary).ComputeHashCode();
                        else
                        {
                            if (typeof(IDictionary).IsAssignableFrom(value.GetType())) hashCode = hashCode ^ (value as IEnumerable).ComputeHashCode();
                        }
                    }
                }
            }
            return hashCode;
        }
        public static Stream GetStream(this IDictionary dictionary)
        {
            var memory = new MemoryStream();
            dictionary.Save(memory);
            memory.Flush();
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }
        public static IList Collect(this IDictionary idictionary,string type)
        {
            var results = new List<object>();
            _Collect(idictionary, type, results);
            return results;
        }
        public static IList Collect(this IDictionary idictionary,Type type)
        {
            var results = new List<object>();
            _Collect(idictionary, type, results);
            return results;
        }
        public static IList Collect<T>(this IDictionary idictionary)
        {
            var results = new List<object>();
            _Collect<T>(idictionary, results);

            return results;
        }
        private static void _Collect<T>(this IDictionary idictionary, IList results)
        {
            foreach (var item in idictionary.Values)
            {
                if (item != null)
                {
                    if (typeof(T).IsAssignableFrom(item.GetType()))
                    {
                        if (!results.Contains(item)) results.Add(item);
                    }
                    var child_idictionary = item as IDictionary;
                    if (child_idictionary != null) _Collect<T>(child_idictionary, results);
                }
            }
        }
        private static void _Collect(this IDictionary idictionary, Type type,IList results)
        {
            foreach (var item in idictionary.Values)
            {
                if (item != null)
                {
                    if (type.IsAssignableFrom(item.GetType()))
                    {
                        if (!results.Contains(item)) results.Add(item);
                    }
                    var child_idictionary = item as IDictionary;
                    if (child_idictionary != null) _Collect(child_idictionary, type,results);
                }
            }
        }
        private static void _Collect(this IDictionary idictionary, string type, IList results)
        {
            foreach (var item in idictionary.Values)
            {
                if (item != null)
                {
                    if (item.GetType().Name == type)
                    {
                        if (!results.Contains(item)) results.Add(item);
                    }
                    var child_idictionary = item as IDictionary;
                    if (child_idictionary != null) _Collect(child_idictionary, type, results);
                }
            }
        }


        public static IDictionary Copy(this IDictionary dictionary, IDictionary source)
        {
            dictionary.Clear();
            foreach (var key in source)
            {
                var value = source[key];
                var child_dictionary = value as IDictionary;
                if (child_dictionary != null)
                {
                    dictionary[key] = new Dictionary<object, dynamic>().Copy(child_dictionary);
                }
                else
                {
                    dictionary[key] = value;
                }
            }
            return dictionary;
        }
        public static T Get<T>(this IDictionary dictionary, string name)
        {
            if (dictionary.Contains(name))
            {
                var value = dictionary[name];
                if (value != null)
                {
                    if (typeof(T).IsAssignableFrom(value.GetType())) return (T)value;
                }
            }
            if (name.Contains(","))
            {
                var names = name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var n in names)
                {
                    if (dictionary.Contains(n)) return dictionary.Get<T>(n);
                }
            }
            if (typeof(T) == typeof(string)) return (T)(object)string.Empty;
            return default(T);
        }
        public static string GetName(this IDictionary dictionary)
        {
            var parent = GetParent(dictionary) as IDictionary;
            if (parent != null)
            {
                foreach (string key in parent.Keys)
                {
                    var test_element = parent.Get<IDictionary>(key);
                    if (test_element != null)
                    {
                        if (object.ReferenceEquals(test_element, dictionary)) return key;
                    }
                }
            }
            return string.Empty;
        }
        public static string GetFullName(this IDictionary dictionary)
        {
            var key = GetName(dictionary);
            if (key != null)
            {
                var parent = GetParent(dictionary) as IDictionary;
                if (parent != null)
                {
                    var parent_full_key = GetFullName(parent);
                    if (parent_full_key.Length > 0)
                    {
                        return $"{parent_full_key}/{key.ToString()}";
                    }
                }
                return key.ToString();
            }
            return string.Empty;
        }
        public static string GetTypeName(this IDictionary source, string typeKey = "Type")
        {
            if (source.Contains(typeKey)) return source.Get<string>(typeKey);
            return string.Empty;
        }
        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            if (source == null) return null;
            if (types == null) return source;
            var copy = Activator.CreateInstance(source.GetType()) as IDictionary;
            if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");
            var typename = source.Get<string>(typeKey);
            if (typename.Length > 0 && types.ContainsKey(typename))
            {
                var targetType = types[typename];
                if (targetType == null) throw new Exception($"types['{typename}'] was null");
                if (source.GetType() != targetType)
                {
                    copy = Activator.CreateInstance(targetType) as IDictionary;
                    if (copy == null) throw new Exception($"failed to create instance of type {targetType.FullName}");
                }
            }
            foreach (string key in source.Keys)
            {
                var value = source[key];
                var childDictionary = value as IDictionary;
                if (childDictionary != null)
                {
                    copy[key] = ConvertTypes(childDictionary, types, typeKey);
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        //copy[key] = IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey);
                        copy[key] = childEnumerable.ConvertTypes(types, typeKey);
                    }
                    else
                    {
                        copy[key] = value;
                    }
                }
            }
            return copy;
        }

        public static Matrix3D GetLocalToParent(this IDictionary dictionary)
        {
            var matrix3D = new Matrix3D();
            if (dictionary != null)
            {
                matrix3D = Factory.Default.Create<Matrix3D>(dictionary);
            }
            return matrix3D;
        }
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary)
        {
            var localToWorld = GetLocalToParent(dictionary);
            if (dictionary != null)
            {

                var parent = dictionary.GetParent();
                if (parent != null)
                {
                    localToWorld.Append(GetLocalToWorld(parent as IDictionary));
                }
            }

            return localToWorld;
        }
        public static Point3D GetWorldOrigin(this IDictionary dictionary)
        {
            return GetLocalToWorld(dictionary).Transform(new Point3D(0, 0, 0));
        }

        public static T GetNearestAncestor<T>(this IDictionary child)
        {
            var parent = child.GetParent() as IDictionary;
            if (child != null && parent != null)
            {
                if (typeof(T).IsAssignableFrom(parent.GetType()))
                {
                    var ancestor = (T)parent;
                    if (ancestor != null) return ancestor;
                }
                return GetNearestAncestor<T>(parent);
            }
            return default(T);
        }
        public static T GetFurthestAncestor<T>(this IDictionary child)
        {
            if (child != null)
            {
                var ancestor = GetNearestAncestor<T>(child) as IDictionary;
                if (ancestor != null)
                {
                    var further_ancestor = GetFurthestAncestor<T>(ancestor);
                    if (further_ancestor != null) return further_ancestor;
                }
                if (ancestor == null)
                {
                    if (typeof(T).IsAssignableFrom(child.GetType()))
                    {
                        ancestor = (IDictionary)(T)(object)child;
                    }
                }
                return (T)ancestor;
            }
            return default(T);
        }
        public static object GetRootAncestor(this IDictionary child)
        {
            return GetFurthestAncestor<IDictionary>(child);

        }
    }
}
