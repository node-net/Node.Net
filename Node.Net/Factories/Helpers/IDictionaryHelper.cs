//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Helpers
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

        public static double GetLengthMeters(IDictionary source, string name)
        {
            return LengthHelper.GetLengthMeters(GetDictionaryValueAsString(source, name));
        }

        public static Vector3D GetTranslation(IDictionary source)
        {
            return new Vector3D(
                GetLengthMeters(source, "X"),
                GetLengthMeters(source, "Y"),
                GetLengthMeters(source,"Z"));
        }

        public static Vector3D GetRotationsXYZ(IDictionary source)
        {
            return new Vector3D(
                GetAngleDegrees(source, "RotationX,Spin,Roll"),
                GetAngleDegrees(source, "RotationY,Tilt,Pitch"),
                GetAngleDegrees(source, "RotationZ,Orientation,Yaw"));
        }

        public static double GetAngleDegrees(IDictionary source, string name)
        {
            if (source == null) return 0.0;
            if (name.Contains(','))
            {
                var names = name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach(var n in names)
                {
                    if(source.Contains(n))
                    {
                        return Helpers.AngleHelper.GetAngleDegrees(GetDictionaryValueAsString(source, n));
                    }
                }
                return 0.0;
            }
            else return Helpers.AngleHelper.GetAngleDegrees(GetDictionaryValueAsString(source, name));
        }
        
        public static Matrix3D GetLocalToParent(IDictionary dictionary)
        {
            var matrix3D = new Matrix3D();
            if (dictionary != null)
            {
                var rotations = Helpers.IDictionaryHelper.GetRotationsXYZ(dictionary);
                matrix3D = Helpers.Matrix3DHelper.RotateXYZ(new Matrix3D(), Helpers.IDictionaryHelper.GetRotationsXYZ(dictionary));
                matrix3D.Translate(Helpers.IDictionaryHelper.GetTranslation(dictionary));
            }
            return matrix3D;
        }

        public static Matrix3D GetLocalToWorld(IDictionary dictionary)
        {
            Matrix3D localToWorld = GetLocalToParent(dictionary);
            if (dictionary != null)
            {

                var parent = global::Node.Net.Factories.ObjectExtension.GetParent(dictionary);
                if (parent != null)
                {
                    localToWorld.Append(GetLocalToWorld(parent as IDictionary));
                }
            }

            return localToWorld;
        }
    }
}
