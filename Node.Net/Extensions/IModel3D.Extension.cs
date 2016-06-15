﻿using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public static class IModel3DExtension
    {
        public static void Update(IModel3D model3D)
        {
            UpdateLocalToParent(model3D);

            var parent = model3D as IParent;
            if (parent != null)
            {
                var deepModels = parent.DeepCollect<IModel3D>();
                foreach (var deepModel in deepModels)
                {
                    UpdateLocalToParent(deepModel);
                }
            }
        }

        public static Matrix3D GetParentToWorld(IModel3D model3D)
        {
            if (model3D != null)
            {
                var child = model3D as IChild;
                if (child != null)
                {
                    var parentToWorld = new Matrix3D();
                    var current_parent = child.GetFirstAncestor<IModel3D>();
                    while(current_parent != null)
                    {
                        parentToWorld.Append(current_parent.LocalToParent);
                        var parent_as_child = current_parent as IChild;
                        if(parent_as_child == null)
                        {
                            current_parent = null;
                        }
                        else
                        {
                            current_parent = parent_as_child.GetFirstAncestor<IModel3D>();
                        }
                    }
                    
                }
                return model3D.LocalToParent;
            }
            return new Matrix3D();
        }

        public static Matrix3D GetLocalToWorld(IModel3D model3D)
        {
            if (model3D != null)
            {
                var localToWorld = Matrix3D.Multiply(model3D.LocalToParent, new Matrix3D());
                localToWorld.Append(GetParentToWorld(model3D));
                return localToWorld;
            }
            return new Matrix3D();
        }
        public static void UpdateLocalToParent(IModel3D model3D)
        {
            if (model3D != null)
            {
                var matrix = new Matrix3D();
                matrix.Rotate(GetQuaternionRotation(model3D as IDictionary));
                var translation = model3D as ITranslation3D;
                if(translation != null)
                {
                    matrix.Translate(translation.Translation3D);
                }
                //matrix.Translate(GetTranslationVector3D(model3D as ITranslation3D));
                model3D.LocalToParent = matrix;
            }
        }

        public static Point3D ApplyTransform(Point3D source, Matrix3D trans)
        {
            var point4D =
                new Point4D(source.X, source.Y, source.Z, 1.0);
            point4D = trans.Transform(point4D);
            return new Point3D(point4D.X, point4D.Y, point4D.Z);
        }
        public static Vector3D ApplyTransform(Vector3D source, Matrix3D trans)
        {
            var mv = new Vector3D(source.X, source.Y, source.Z);
            mv = trans.Transform(mv);
            var result = new Vector3D(mv.X, mv.Y, mv.Z);
            return result;
        }

        public static Point3D TransformLocalToWorld(IModel3D model3D,Point3D local)
        {
            return ApplyTransform(local, GetLocalToWorld(model3D));
        }
        public static Point3D TransformLocalToParent(IModel3D model3D,Point3D local)
        {
            return ApplyTransform(local, model3D.LocalToParent);
        }

        private static double GetRotationDegrees(IDictionary dictionary, string key)
        {
            if (object.ReferenceEquals(null, dictionary)) return 0;
            if (!dictionary.Contains(key)) return 0;
            if (object.ReferenceEquals(null, dictionary[key])) return 0;
            return Measurement.Angle.Parse(dictionary[key].ToString())[Measurement.AngularUnit.Degrees];
        }

        public static Quaternion GetQuaternionRotation(IDictionary value)
        {
            if (value == null) return new Quaternion();
            var rotationZ = new Quaternion();
            var rotationY = new Quaternion();
            var rotationX = new Quaternion();
            if (value.Contains("RotationZ"))
            {
                var rotationZ_degrees = GetRotationDegrees(value, "RotationZ");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("Orientation"))
            {
                var rotationZ_degrees = GetRotationDegrees(value, "Orientation");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("RotationY"))
            {
                var rotationY_degrees = GetRotationDegrees(value, "RotationY");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("Tilt"))
            {
                var rotationY_degrees = GetRotationDegrees(value, "Tilt");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("RotationX"))
            {
                var rotationX_degrees = GetRotationDegrees(value, "RotationX");
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }
            if (value.Contains("Spin"))
            {
                var rotationX_degrees = GetRotationDegrees(value, "Spin");
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }

            var total_rotation = Quaternion.Multiply(rotationX, Quaternion.Multiply(rotationY, rotationZ));
            return total_rotation;
        }
    }
}
