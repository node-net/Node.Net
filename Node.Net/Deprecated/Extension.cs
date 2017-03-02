//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated
{
    public static class Extension
    {
        public static T Create<T>(this IFactory factory) => Create<T>(factory, null);
        public static T Create<T>(this IFactory factory,object source)
        {
            return (T)(object)factory.Create(typeof(T), source);
        }

        #region TextReader
        public static void EatWhiteSpace(this TextReader reader) => Extensions.TextReaderExtension.EatWhiteSpace(reader);

        public static string Seek(this TextReader reader, char value) => Extensions.TextReaderExtension.Seek(reader, value);
        public static string Seek(this TextReader reader, char[] values) => Extensions.TextReaderExtension.Seek(reader, values);
        #endregion

        #region ImageSource
        //public static ImageSource GetImageSource(this System.Drawing.Image image) => Extensions.ImageExtension.GetImageSource(image);
        //public static void Save(this ImageSource imageSource, string filename) => Extensions.ImageSourceExtension.Save(imageSource, filename);
        //public static ImageSource Crop(this ImageSource imageSource, int width, int height) => Extensions.ImageSourceExtension.Crop(imageSource, width, height);
        #endregion

        #region Type
        public static Stream GetStream(this Type type, string name) => AssemblyExtension.GetStream(type.Assembly, name);
        public static string[] GetManifestResourceNames(this Type type, string name) => AssemblyExtension.GetManifestResourceNames(type.Assembly, name);
        public static Dictionary<string, T> CollectManifestResources<T>(this Type type, string pattern) => global::Node.Net.Deprecated.Resources.Resources.CollectManifestResources<T>(type, pattern);
        #endregion

        #region IParent
        //public static Dictionary<string, T> Collect<T>(this IParent parent) => Extensions.IParentExtension.Collect<T>(parent);
        //public static Dictionary<string, T> DeepCollect<T>(this IParent parent) => Extensions.IParentExtension.DeepCollect<T>(parent);
        #endregion



        #region IWrite
        public static string WriteToString(this Deprecated.Writers.IWrite write, object value) => Deprecated.Writers.IWriteExtension.WriteToString(write, value);
        #endregion

        #region IGetDataSet
        //public static string[] GetStringArray(this IGetDataSet getDataSet, string sql) => Extensions.IGetDataSetExtension.GetStringArray(getDataSet, sql);
        #endregion

        #region Grid
        //public static void AddRow(this Grid grid, string[] content, System.Windows.Media.Brush backgroundBrush = null, System.Windows.Media.Brush foregroundBrush = null) => Extensions.GridExtension.AddRow(grid, content, backgroundBrush, foregroundBrush);

        #endregion

        #region Object
        public static object GetKey(this object item) => Collections.ObjectExtension.GetKey(item);
        public static object GetValue(this object item) => Collections.ObjectExtension.GetValue(item);
        public static string GetFullKey(this object item) => Collections.ObjectExtension.GetFullKey(item);
        public static string GetName(this object item) => Collections.ObjectExtension.GetName(item);
        public static string GetFullName(this object item) => Collections.ObjectExtension.GetFullName(item);
        public static T GetValue<T>(this object item)
        {
            var instance = GetValue(item);
            if (instance != null && typeof(T).IsAssignableFrom(instance.GetType())) return (T)instance;
            return default(T);
        }
        public static bool IsKeyValuePair(this object item) => Collections.ObjectExtension.IsKeyValuePair(item);
        public static object GetParent(this object item) => Collections.ObjectExtension.GetParent(item);
        public static void SetParent(this object item, object parent) => Collections.ObjectExtension.SetParent(item, parent);
        public static T GetNearestAncestor<T>(this object child) => Collections.ObjectExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this object child) => Collections.ObjectExtension.GetFurthestAncestor<T>(child);
        public static object GetRootAncestor(this object child) => Collections.ObjectExtension.GetRootAncestor(child);

        public static object GetPropertyValue(this object item, string propertyName) => ObjectExtension.GetPropertyValue(item, propertyName);
        public static T GetPropertyValue<T>(this object item, string propertyName) => ObjectExtension.GetPropertyValue<T>(item, propertyName);
        public static void SetPropertyValue(this object item, string propertyName, object propertyValue) => ObjectExtension.SetPropertyValue(item, propertyName, propertyValue);
        #endregion

        #region IDictionary

        #endregion

        #region PerspectiveCamera
        public static PerspectiveCamera GetTransformedPerspectiveCamera(this PerspectiveCamera camera, Transform3D transform)
            => Extensions.PerspectiveCameraExtension.GetTransformedPerspectiveCamera(camera, transform);
        public static bool IsVisible(this PerspectiveCamera camera, Point3D worldPoint) => Extensions.PerspectiveCameraExtension.IsVisible(camera, worldPoint);
        #endregion

        #region UIElement
        public static void Refresh(UIElement element) => Extensions.UIElementExtension.Refresh(element);
        #endregion

        public static Matrix3D RotateXYZ(this Matrix3D matrix, Vector3D rotationsXYZ) => Matrix3DExtension.RotateXYZ(matrix, rotationsXYZ);
        public static Point3D[] Transform(this Matrix3D matrix, Point3D[] points) => Matrix3DExtension.Transform(matrix, points);
    }
}