using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Extension
    {
        static Extension()
        {
            Node.Net.Factories.MetaDataMap.GetMetaDataFunction = Node.Net.Collections.MetaDataMap.GetMetaDataFunction;
            Node.Net.Collections.IDictionaryExtension.GetLocalToParentFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToParent;
            Node.Net.Collections.IDictionaryExtension.GetLocalToWorldFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToWorld;
        }
        public static T Create<T>(this IFactory factory, object source) => Extensions.IFactoryExtension.Create<T>(factory, source);
        

        #region TextReader
        public static void EatWhiteSpace(this TextReader reader) => Extensions.TextReaderExtension.EatWhiteSpace(reader);

        public static string Seek(this TextReader reader, char value) => Extensions.TextReaderExtension.Seek(reader, value);
        public static string Seek(this TextReader reader, char[] values) => Extensions.TextReaderExtension.Seek(reader, values);
        #endregion

        #region ImageSource
        public static ImageSource GetImageSource(this System.Drawing.Image image) => Extensions.ImageExtension.GetImageSource(image);
        public static void Save(this ImageSource imageSource, string filename) => Extensions.ImageSourceExtension.Save(imageSource, filename);
        public static ImageSource Crop(this ImageSource imageSource, int width, int height) => Extensions.ImageSourceExtension.Crop(imageSource, width, height);
        #endregion

        #region Type
        public static Stream GetStream(this Type type, string name) => Extensions.TypeExtension.GetStream(type, name);
        public static string[] GetManifestResourceNames(this Type type, string name) => Extensions.TypeExtension.GetManifestResourceNames(type, name);
        public static Dictionary<string, T> CollectManifestResources<T>(this Type type, string pattern) => Extensions.TypeExtension.CollectManifestResources<T>(type, pattern);
        #endregion

        #region IMetaData
        public static void SetTransformMetaData(this IMetaDataManager metaData, IDictionary dictionary) => Extensions.IMetaDataManagerExtension.SetTransformMetaData(metaData, dictionary);
        #endregion

        #region IParent
        public static Dictionary<string,T> Collect<T>(this IParent parent) => Extensions.IParentExtension.Collect<T>(parent);
        public static Dictionary<string,T> DeepCollect<T>(this IParent parent) => Extensions.IParentExtension.DeepCollect<T>(parent);
        #endregion

        

        

        #region IGetDataSet
        public static string[] GetStringArray(this IGetDataSet getDataSet, string sql) => Extensions.IGetDataSetExtension.GetStringArray(getDataSet, sql);
        #endregion

        #region Grid
        public static void AddRow(this Grid grid, string[] content, System.Windows.Media.Brush backgroundBrush = null, System.Windows.Media.Brush foregroundBrush = null) => Extensions.GridExtension.AddRow(grid, content, backgroundBrush, foregroundBrush);

        #endregion

        /*
        #region IWriter
        public static void Save(this IWriter writer, string filename, object value) => Extensions.IWriterExtension.Save(writer, filename, value);
        #endregion
        */
        #region IDictionary
        public static T Get<T>(this IDictionary dictionary, string name) => Collections.IDictionaryExtension.Get<T>(dictionary, name);
        public static void Set(this IDictionary dictionary, string name, object value) => Collections.IDictionaryExtension.Set(dictionary, name, value);
        public static void RemoveKeys(this IDictionary dictionary, string[] keys) => Collections.IDictionaryExtension.RemoveKeys(dictionary, keys);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.Collect<T>(dictionary);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary, Collections.IFilter filter) => Collections.IDictionaryExtension.Collect<T>(dictionary, filter);
        public static Dictionary<string, T> DeepCollect<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.DeepCollect<T>(dictionary);
        public static Dictionary<string, T> DeepCollect<T>(this IDictionary dictionary, Collections.IFilter filter) => Collections.IDictionaryExtension.DeepCollect<T>(dictionary, filter);
        public static void Remove<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.Remove<T>(dictionary);
        public static void DeepRemove<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.DeepRemove<T>(dictionary);
        public static string[] CollectUniqueStrings(this IDictionary dictionary, string key) => Collections.IDictionaryExtension.CollectUniqueStrings(dictionary, key);
        public static object GetParent(this IDictionary dictionary) => Collections.IDictionaryExtension.GetParent(dictionary);
        public static void SetParent(this IDictionary dictionary, object parent) => Collections.IDictionaryExtension.SetParent(dictionary, parent);
        public static void Copy(this IDictionary destination, IDictionary source) => Collections.IDictionaryExtension.Copy(destination, source);
        public static T GetNearestAncestor<T>(this IDictionary child) => Collections.IDictionaryExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this IDictionary child) => Collections.IDictionaryExtension.GetFurthestAncestor<T>(child);
        public static IDictionary GetRootAncestor(this IDictionary child) => Collections.IDictionaryExtension.GetRootAncestor(child);
        public static T Find<T>(this IDictionary dictionary, string key) => Collections.IDictionaryExtension.Find<T>(dictionary, key);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Factories.Extension.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Factories.Extension.IDictionaryExtension.GetLocalToWorld(dictionary);
        #endregion

    }
}