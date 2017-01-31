using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public static class ObjectExtension
    {
        public static object GetParent(this object item)
        {
            var element = item as IElement;
            if (element != null) return element.Parent;
            if (MetaDataMap.GetMetaDataFunction != null)
            {
                if(MetaDataMap.GetMetaDataFunction(item).ContainsKey("Parent"))
                {
                    return MetaDataMap.GetMetaDataFunction(item)["Parent"];
                }
            }
            return null;
        }

        public static void SetParent(this object item, object parent)
        {
            var element = item as IElement;
            if (element != null) element.Parent = parent;
            else
            {
                if (item != null)
                {
                    if (MetaDataMap.GetMetaDataFunction != null)
                    {
                        MetaDataMap.GetMetaDataFunction(item)["Parent"] = parent;
                    }
                }
            }
        }

        public static T GetNearestAncestor<T>(this object child)
        {
            var parent = GetParent(child);
            if (child != null && parent != null)
            {
                if (typeof(T).IsAssignableFrom(parent.GetType()))
                {
                    var ancestor = (T)parent;
                    if (ancestor != null) return ancestor;
                }
                return GetNearestAncestor<T>(parent as IDictionary);
            }
            return default(T);
        }


        public static void UpdateParentBindings(this object item)
        {
            var dictionary = item as IDictionary;
            if(dictionary != null)
            {
                foreach(var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if(child != null && typeof(IDictionary).IsAssignableFrom(child.GetType()))
                    {
                        SetParent(child, item);
                        UpdateParentBindings(child);
                    }
                }
            }
            else
            {
                var ienumerable = item as IEnumerable;
                if (ienumerable != null)
                {
                    foreach (var child in ienumerable)
                    {
                        if(child != null && typeof(IDictionary).IsAssignableFrom(child.GetType()))
                        {
                            SetParent(child, item);
                            UpdateParentBindings(child);
                        }
                    }
                }
            }
        }

        public static Matrix3D GetLocalToParent(this object source)
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

        public static Matrix3D GetLocalToWorld(this object source)
        {
            var localToWorld = source.GetLocalToParent();// GetLocalToParent(dictionary);
            if (source != null)
            {

                var parent = source.GetParent();// Node.Net.Factories.Extension.ObjectExtension.GetParent(source);
                if (parent != null)
                {
                    localToWorld.Append(parent.GetLocalToWorld());// GetLocalToWorld(parent as IDictionary));
                }
            }

            return localToWorld;
        }
        public static Vector3D GetRotationsXYZ(this object source)
        {
            return new Vector3D(
                GetAngleDegrees(source, "RotationX,Spin,Roll"),
                GetAngleDegrees(source, "RotationY,Tilt,Pitch"),
                GetAngleDegrees(source, "RotationZ,Orientation,Yaw"));
        }
        public static Vector3D GetTranslation(this object source)
        {
            return new Vector3D(
                GetLengthMeters(source, "X"),
                GetLengthMeters(source, "Y"),
                GetLengthMeters(source, "Z"));
        }
        public static double GetLengthMeters(this object source, string name)
        {
            var element = source as IElement;
            if (element != null) return element.GetLengthMeters(name);
            var dictionary = source as IDictionary;
            if (dictionary != null) return dictionary.GetLengthMeters(name);
            return 0;
        }
        public static double GetAngleDegrees(this object source, string name)
        {
            var element = source as IElement;
            if (element != null) return element.GetAngleDegrees(name);
            var dictionary = source as IDictionary;
            if (dictionary != null) return dictionary.GetAngleDegrees(name);
            return 0;
        }
        /*
        public static Matrix3D GetLocalToParent(this object source)
        {
            var element = source as IElement;
            if (element != null) return element.GetLocalToParent();
            var dictionary = source as IDictionary;
            if (dictionary != null) return dictionary.GetLocalToParent();
            return new Matrix3D();
        }

        public static Matrix3D GetLocalToWorld(this object source)
        {
            var element = source as IElement;
            if (element != null) return element.GetLocalToWorld();
            var dictionary = source as IDictionary;
            if (dictionary != null) return dictionary.GetLocalToWorld();
            return new Matrix3D();
        }*/
    }
}
