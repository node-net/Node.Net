using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Extension
    {
        /*
        static Extension()
        {
            Node.Net.Factories.MetaDataMap.GetMetaDataFunction = Node.Net.Collections.MetaDataMap.GetMetaDataFunction;
            Node.Net.Collections.IDictionaryExtension.GetLocalToParentFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToParent;
            Node.Net.Collections.IDictionaryExtension.GetLocalToWorldFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToWorld;
        }*/
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
        public static Dictionary<string, T> Collect<T>(this IParent parent) => Extensions.IParentExtension.Collect<T>(parent);
        public static Dictionary<string, T> DeepCollect<T>(this IParent parent) => Extensions.IParentExtension.DeepCollect<T>(parent);
        #endregion





        #region IGetDataSet
        public static string[] GetStringArray(this IGetDataSet getDataSet, string sql) => Extensions.IGetDataSetExtension.GetStringArray(getDataSet, sql);
        #endregion

        #region Grid
        public static void AddRow(this Grid grid, string[] content, System.Windows.Media.Brush backgroundBrush = null, System.Windows.Media.Brush foregroundBrush = null) => Extensions.GridExtension.AddRow(grid, content, backgroundBrush, foregroundBrush);

        #endregion

        #region Object
        public static object GetKey(this object item) => Collections.ObjectExtension.GetKey(item);
        public static object GetValue(this object item) => Collections.ObjectExtension.GetValue(item);
        public static object GetParent(this object item) => Collections.ObjectExtension.GetParent(item);
        public static void SetParent(this object item, object parent) => Collections.ObjectExtension.SetParent(item, parent);
        public static T GetNearestAncestor<T>(this object child) => Collections.ObjectExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this object child) => Collections.ObjectExtension.GetFurthestAncestor<T>(child);
        public static object GetRootAncestor(this object child) => Collections.ObjectExtension.GetRootAncestor(child);
        #endregion

        #region IDictionary
        
        #endregion

    }
}