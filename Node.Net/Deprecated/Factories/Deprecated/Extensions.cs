﻿using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated
{
    public static class Extensions
    {
        public static T Create<T>(this IFactory factory, object source,IFactory helper) => Internal.IFactoryExtension.Create<T>(factory, source,helper);
        public static object Create(this IFactory factory, object source,IFactory helper) => Internal.IFactoryExtension.Create(factory, source,helper);
        public static Point3D? HitTest(this Visual3D reference, Point3D point, Vector3D direction) => Extension.Visual3DExtension.HitTest(reference, point, direction);
        public static void SetName(this Stream stream, string name) => Extension.StreamExtension.SetName(stream, name);
        public static string GetName(this Stream stream) => Deprecated.Extension.StreamExtension.GetName(stream);
        public static T GetNearestAncestor<T>(this IChild child) => Internal.IChildExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this IChild child) => Internal.IChildExtension.GetFurthestAncestor<T>(child);
        public static IFactory GetRootAncestor(this IChild child) => Internal.IChildExtension.GetRootAncestor(child);
        public static object GetParent(this object item) => Node.Net.Factories.Extension.ObjectExtension.GetParent(item);
        public static void SetParent(this object item, object parent) => Node.Net.Factories.Extension.ObjectExtension.SetParent(item, parent);
        public static void UpdateParentBindings(this object item) => Node.Net.Factories.Extension.ObjectExtension.UpdateParentBindings(item);
        public static T GetNearestAncestor<T>(this object child) => Node.Net.Factories.Extension.ObjectExtension.GetNearestAncestor<T>(child);
    }
}