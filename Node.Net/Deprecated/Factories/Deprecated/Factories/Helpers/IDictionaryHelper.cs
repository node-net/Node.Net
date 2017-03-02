﻿using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    public static class IDictionaryHelper
    {
        public static string GetDictionaryValueAsString(IDictionary source, string name)
        {
            if (source != null && source.Contains(name))
            {
                return source[name].ToString();
            }

            if (name.Contains(","))
            {
                var keys = name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var key in keys)
                {
                    var value = GetDictionaryValueAsString(source, key);
                    if (value.Length > 0) return value;
                }
            }
            return string.Empty;
        }

        public static double GetLengthMeters(IDictionary source, string name, IFactory factory)
        {
            return factory.Create<ILength>(GetDictionaryValueAsString(source, name),null).Length;
        }

        public static double GetAngleDegrees(IDictionary source, string name, IFactory factory)
        {
            return factory.Create<IAngle>(GetDictionaryValueAsString(source, name),null).Angle;
        }

        public static Matrix3D GetLocalToParent(IDictionary dictionary)
        {
            var localToParent = Factory.Default.Create<ILocalToWorld>(dictionary,null);
            if (localToParent != null) return localToParent.LocalToWorld;
            return new Matrix3D();
        }
        public static Matrix3D GetLocalToWorld(IDictionary dictionary)
        {
            var localToWorld = Factory.Default.Create<ILocalToWorld>(dictionary,null);
            if (localToWorld != null) return localToWorld.LocalToWorld;
            return new Matrix3D();
        }
        public static Rect3D GetBounds(IDictionary dictionary)
        {
            var localToParent = GetLocalToParent(dictionary);
            var bounds = new Rect3D(localToParent.Transform(new Point3D(0, 0, 0)), new Size3D(0, 0, 0));
            foreach(var key in dictionary.Keys)
            {
                var childDictionary = dictionary[key] as IDictionary;
                if(childDictionary != null)
                {
                    var childBounds = GetBounds(childDictionary);
                    bounds.Union(localToParent.Transform(childBounds.Location));
                }
            }
            return bounds;
        }
    }
}