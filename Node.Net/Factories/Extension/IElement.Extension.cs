﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public static class IElementExtension
    {
        public static Vector3D GetRotationsXYZ(this Node.Net.Factories.IElement source)
        {
            return new Vector3D(
                GetAngleDegrees(source, "RotationX,Spin,Roll"),
                GetAngleDegrees(source, "RotationY,Tilt,Pitch"),
                GetAngleDegrees(source, "RotationZ,Orientation,Yaw"));
        }
        public static Vector3D GetTranslation(this Node.Net.Factories.IElement source)
        {
            return new Vector3D(
                GetLengthMeters(source, "X"),
                GetLengthMeters(source, "Y"),
                GetLengthMeters(source, "Z"));
        }
        public static double GetLengthMeters(this Node.Net.Factories.IElement source, string name)
        {
            return Helpers.LengthHelper.GetLengthMeters(GetValueAsString(source, name));
        }
        public static double GetAngleDegrees(this Node.Net.Factories.IElement source, string name)
        {
            if (source == null) return 0.0;
            if (name.Contains(','))
            {
                var names = name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var n in names)
                {
                    if (source.Contains(n))
                    {
                        return Helpers.AngleHelper.GetAngleDegrees(GetValueAsString(source, n));
                    }
                }
                return 0.0;
            }
            else return Helpers.AngleHelper.GetAngleDegrees(GetValueAsString(source, name));
        }
        public static string GetValueAsString(this Node.Net.Factories.IElement source, string name)
        {
            if (source != null && source.Contains(name))
            {
                return source.Get(name).ToString();
            }

            if (name.Contains(","))
            {
                var keys = name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var key in keys)
                {
                    var value = GetValueAsString(source, key);
                    if (value.Length > 0) return value;
                }
            }
            return string.Empty;
        }
        public static Matrix3D GetLocalToParent(this Node.Net.Factories.IElement source)
        {
            var matrix3D = new Matrix3D();
            if (source != null)
            {
                var rotations = source.GetRotationsXYZ();// Extension.IDictionaryExtension.GetRotationsXYZ(dictionary);
                matrix3D = Helpers.Matrix3DHelper.RotateXYZ(new Matrix3D(), source.GetRotationsXYZ());// Extension.IDictionaryExtension.GetRotationsXYZ(dictionary));
                matrix3D.Translate(source.GetTranslation());// Extension.IDictionaryExtension.GetTranslation(dictionary));
            }
            return matrix3D;
        }

        public static Matrix3D GetLocalToWorld(this Node.Net.Factories.IElement source)
        {
            var localToWorld = source.GetLocalToParent();// GetLocalToParent(dictionary);
            if (source != null)
            {

                var parent = source.Parent;// Node.Net.Factories.Extension.ObjectExtension.GetParent(source);
                if (parent != null)
                {
                    localToWorld.Append(parent.GetLocalToWorld());// GetLocalToWorld(parent as IDictionary));
                }
            }

            return localToWorld;
        }
    }
}
