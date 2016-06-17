using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Extension
    {
        #region IDictionary
        public static void Save(this IDictionary dictionary, Stream stream) => Extensions.IDictionaryExtension.Save(dictionary, stream);
        public static void Save(this IDictionary dictionary, string filename) => Extensions.IDictionaryExtension.Save(dictionary, filename);
        public static object Get(this IDictionary dictionary, string key) => Extensions.IDictionaryExtension.Get(dictionary, key);
        public static void Set(this IDictionary dictionary, string key, object value) => Extensions.IDictionaryExtension.Set(dictionary, key, value);
        public static string[] Find(this IDictionary dictionary, IFilter filter) => Extensions.IDictionaryExtension.Find(dictionary, filter);
        public static IDictionary Collect(this IDictionary dictionary, IFilter filter) => Extensions.IDictionaryExtension.Collect(dictionary, filter);
        #endregion

        #region TextReader
        public static void EatWhiteSpace(this TextReader reader) => Extensions.TextReaderExtension.EatWhiteSpace(reader);

        public static string Seek(this TextReader reader, char value) => Extensions.TextReaderExtension.Seek(reader, value);
        public static string Seek(this TextReader reader, char[] values) => Extensions.TextReaderExtension.Seek(reader, values);
        #endregion

        #region ImageSource
        public static ImageSource GetImageSource(this Image image) => Extensions.ImageExtension.GetImageSource(image);
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

        #region IChild
        public static T GetNearestAncestor<T>(this IChild child) => Extensions.IChildExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this IChild child) => Extensions.IChildExtension.GetFurthestAncestor<T>(child);
        public static string GetKey(this IChild child) => Extensions.IChildExtension.GetKey(child);
        public static string GetFullKey(this IChild child) => Extensions.IChildExtension.GetFullKey(child);
        #endregion

        #region IModel3D
        public static Matrix3D GetParentToWorld(this IModel3D model3D) => Extensions.IModel3DExtension.GetParentToWorld(model3D);
        public static Matrix3D GetLocalToWorld(this IModel3D model3D) => Extensions.IModel3DExtension.GetLocalToWorld(model3D);
        public static Point3D TransformLocalToWorld(this IModel3D model3D, Point3D local) => Extensions.IModel3DExtension.TransformLocalToWorld(model3D, local);
        public static Vector3D TransformLocalToWorld(this IModel3D model3D, Vector3D local) => Extensions.IModel3DExtension.TransformLocalToWorld(model3D, local);
        public static Point3D TransformLocalToParent(this IModel3D model3D, Point3D local) => Extensions.IModel3DExtension.TransformLocalToParent(model3D, local);
        public static Vector3D TransformLocalToParent(this IModel3D model3D, Vector3D local) => Extensions.IModel3DExtension.TransformLocalToParent(model3D, local);
        public static Vector3D[] TransformLocalToParent(this IModel3D model3D, Vector3D[] local) => Extensions.IModel3DExtension.TransformLocalToParent(model3D, local);
        public static Point3D TransformWorldToLocal(this IModel3D model3D, Point3D world) => Extensions.IModel3DExtension.TransformWorldToLocal(model3D, world);
        public static Vector3D TransformWorldToLocal(this IModel3D model3D, Vector3D world) => Extensions.IModel3DExtension.TransformWorldToLocal(model3D, world);
        public static Point3D GetWorldOrigin(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldOrigin(model3D);
        public static Point3D GetWorldRotations(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldRotations(model3D);
        public static Vector3D GetWorldXDirectionVector(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldXDirectionVector(model3D);
        public static Vector3D GetWorldYDirectionVector(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldYDirectionVector(model3D);
        public static Vector3D GetWorldZDirectionVector(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldZDirectionVector(model3D);
        public static double GetWorldOrientation(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldOrientation(model3D);
        public static double GetWorldTilt(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldTilt(model3D);
        public static double GetWorldSpin(this IModel3D model3D) => Extensions.IModel3DExtension.GetWorldSpin(model3D);
        #endregion

        #region IGetDataSet
        public static string[] GetStringArray(this IGetDataSet getDataSet, string sql) => Extensions.IGetDataSetExtension.GetStringArray(getDataSet, sql);
        #endregion
    }
}