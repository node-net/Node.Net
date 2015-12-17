using System;
using System.Collections;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D.Rendering
{
    public class Renderer : Node.Net.Model3D.Rendering.IRenderer
    {
        private ResourceDictionary resources = new ResourceDictionary();
        public ResourceDictionary Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        public virtual Visual3D GetVisual3D(object value)
        {
            return ToVisual3D(this, value);
        }

        public virtual ModelVisual3D GetModelVisual3D(object value)
        {
            return ToModelVisual3D(this, value);
        }

        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(object value)
        {
            return ToModel3D(this, value);
        }

        public virtual GeometryModel3D GetGeometryModel3D(object value)
        {
            return ToGeometryModel3D(this, value);
        }

        public virtual System.Windows.Media.Media3D.Transform3D GetTransform3D(object value)
        {
            return ToTransform3D(this, value);
        }
        public virtual Vector3D GetTranslation(object value)
        {
            return ToTranslation(this, value);
        }
        public virtual Vector3D GetScale(object value)
        {
            return ToScale(this, value);
        }
        public virtual System.Windows.Media.Media3D.Material GetMaterial(object value)
        {
            return ToMaterial(this, value);
        }
        public virtual System.Windows.Media.Media3D.Material GetBackMaterial(object value)
        {
            return ToBackMaterial(this, value);
        }
        public static Visual3D ToVisual3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToVisual3D(renderer, dictionary);
            return null;
        }
        public static ModelVisual3D ToModelVisual3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToModelVisual3D(renderer, dictionary);
            return null;
        }
        public static System.Windows.Media.Media3D.Model3D ToModel3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToGeometryModel3D(renderer, dictionary);
            return null;
        }
        public static GeometryModel3D ToGeometryModel3D(IRenderer renderer, object value)
        {
            return ToGeometryModel3D(renderer, value as IDictionary);
        }
        public static System.Windows.Media.Media3D.Transform3D ToTransform3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToTransform3D(renderer, dictionary);
            return null;
        }
        public static System.Windows.Media.Media3D.Material ToBackMaterial(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToBackMaterial(renderer, dictionary);
            return null;
        }
        public static System.Windows.Media.Media3D.Material ToMaterial(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToMaterial(renderer, dictionary);
            return null;
        }
        public static Vector3D ToTranslation(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToTranslation(renderer, dictionary);
            return new Vector3D(0, 0, 0);
        }
        public static Vector3D ToScale(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return ToScale(renderer, dictionary);
            return new Vector3D(0, 0, 0);
        }
        public static Visual3D ToVisual3D(IRenderer renderer, IDictionary value)
        {
            if (value.Contains("Visual3D"))
            {
                return renderer.Resources[value["Visual3D"].ToString()] as Visual3D;
            }
            else
            {
                return renderer.GetModelVisual3D(value);
            }
        }
        public static ModelVisual3D ToModelVisual3D(IRenderer renderer, System.Collections.IDictionary dictionary)
        {
            ModelVisual3D modelVisual3D = new ModelVisual3D();
            System.Windows.Media.Media3D.Model3D model3D = renderer.GetModel3D(dictionary);
            if (!ReferenceEquals(null, model3D))
            {
                modelVisual3D.Content = model3D;
            }
            else
            {
                foreach (string key in dictionary.Keys)
                {
                    IDictionary childDictionary = dictionary[key] as IDictionary;
                    if (!ReferenceEquals(null, childDictionary))
                    {
                        Visual3D v3d = renderer.GetVisual3D(childDictionary);
                        if (!ReferenceEquals(null, v3d)) modelVisual3D.Children.Add(v3d);
                    }
                }
            }
            return modelVisual3D;
        }
        public static System.Windows.Media.Media3D.Model3D ToModel3D(IRenderer renderer, IDictionary value)
        {
            GeometryModel3D geometryModel3D = renderer.GetGeometryModel3D(value);
            return geometryModel3D;
        }
        public static System.Windows.Media.Media3D.Model3DGroup ToModel3DGroup(IDictionary value)
        {
            return null;
        }
        public static GeometryModel3D ToGeometryModel3D(IRenderer renderer, System.Collections.IDictionary value)
        {
            Geometry3D geometry = null;
            if (value.Contains("Geometry"))
            {
                geometry = renderer.Resources[value["Geometry"].ToString()] as Geometry3D;
            }
            if (value.Contains("GeometryModel3D"))
            {
                geometry = renderer.Resources[value["GeometryModel3D"].ToString()] as Geometry3D;
            }
            if (!ReferenceEquals(null, geometry))
            {
                return new GeometryModel3D()
                {
                    Geometry = geometry,
                    Material = renderer.GetMaterial(value),
                    BackMaterial = renderer.GetBackMaterial(value),
                    Transform = renderer.GetTransform3D(value)
                };
            }

            return null;
        }
        public static System.Windows.Media.Media3D.Transform3D ToTransform3D(IRenderer renderer, System.Collections.IDictionary value)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(new ScaleTransform3D(renderer.GetScale(value)));
            transformGroup.Children.Add(GetRotateTransform3D(value));
            TranslateTransform3D translateTransform = new TranslateTransform3D(renderer.GetTranslation(value));
            Vector3D translation = renderer.GetTranslation(value);
            Point3D center = new Point3D(translation.X, translation.Y, translation.Z);
            transformGroup.Children.Add(new TranslateTransform3D(renderer.GetTranslation(value)));
            return transformGroup;
        }
        public static RotateTransform3D GetRotateTransform3D(System.Collections.IDictionary value)
        {
            if (value.Contains("RotationZ"))
            {
                double rotationZ = System.Convert.ToDouble(value["RotationZ"].ToString());
                Quaternion quaternion = new Quaternion(new Vector3D(0, 0, 1), rotationZ);
                QuaternionRotation3D rotation = new QuaternionRotation3D() { Quaternion = quaternion };
                return new RotateTransform3D(rotation);

            }

            return new RotateTransform3D();
        }
        public static Rotation3D GetRotationTrans(IDictionary value)
        {
            //Rotation3D rotationZ = new Rotation3D();//AxisAngleRotation,QuaternionRotatiom
            //return rotationZ();
            return null;
        }
        public static Vector3D ToTranslation(IRenderer renderer, IDictionary value)
        {
            Vector3D result = new Vector3D();
            if (value.Contains("X"))
            {
                result.X = System.Convert.ToDouble(value["X"].ToString());
            }
            if (value.Contains("Y"))
            {
                result.Y = System.Convert.ToDouble(value["Y"].ToString());
            }
            if (value.Contains("Z"))
            {
                result.Z = System.Convert.ToDouble(value["Z"].ToString());
            }
            return result;
        }
        public static Vector3D ToScale(IRenderer renderer, IDictionary value)
        {
            Vector3D result = new Vector3D(1, 1, 1);
            if (value.Contains("ScaleX"))
            {
                result.X = System.Convert.ToDouble(value["ScaleX"].ToString());
            }
            if (value.Contains("ScaleY"))
            {
                result.Y = System.Convert.ToDouble(value["ScaleY"].ToString());
            }
            if (value.Contains("ScaleZ"))
            {
                result.Z = System.Convert.ToDouble(value["ScaleZ"].ToString());
            }
            return result;
        }
        public static System.Windows.Media.Media3D.Material ToMaterial(IRenderer renderer, IDictionary value)
        {
            System.Windows.Media.Media3D.Material material = null;
            if (value.Contains("Material"))
            {
                material = renderer.Resources[value["Material"].ToString()] as System.Windows.Media.Media3D.Material;
            }
            return material;
        }
        public static System.Windows.Media.Media3D.Material ToBackMaterial(IRenderer renderer, IDictionary value)
        {
            System.Windows.Media.Media3D.Material material = null;
            if (value.Contains("BackMaterial"))
            {
                material = renderer.Resources[value["BackMaterial"].ToString()] as System.Windows.Media.Media3D.Material;
            }
            return material;
        }
    }
}
