using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D.Transform
{
    class IDictionaryTransform
    {
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
            return ToModel3DGroup(renderer, value);
            /*
            if(value.Contains("Model3D"))
            {
                string smodel3d = value["Model3D"].ToString();
                if(renderer.Resources.Contains(smodel3d))
                {
                    System.Windows.Media.Media3D.Model3D model = renderer.Resources[smodel3d] as System.Windows.Media.Media3D.Model3D;
                    if (!object.ReferenceEquals(null, model)) return model;
                }
            }
            GeometryModel3D geometryModel3D = renderer.GetGeometryModel3D(value);
            return geometryModel3D;*/
        }
        public static System.Windows.Media.Media3D.Model3DGroup ToModel3DGroup(IRenderer renderer,IDictionary value)
        {
            Model3DGroup model3DGroup = new Model3DGroup();

            // Primary Model
            if(value.Contains("Model3D"))
            {
                System.Windows.Media.Media3D.Model3D model = renderer.GetResource(value["Model3D"].ToString()) as System.Windows.Media.Media3D.Model3D;
                if(!object.ReferenceEquals(null, model))
                {
                    model.Transform = ToTransform3D(renderer, value);
                    model3DGroup.Children.Add(model);
                }
            }
            else
            {
                GeometryModel3D geometryModel3D = ToGeometryModel3D(renderer, value);
                if(!object.ReferenceEquals(null, geometryModel3D))
                {
                    model3DGroup.Children.Add(geometryModel3D);
                }
            }

            // Children
            foreach (string key in value.Keys)
            {
                IDictionary childDictionary = value[key] as IDictionary;
                if (!ReferenceEquals(null, childDictionary))
                {
                    System.Windows.Media.Media3D.Model3D m3d = renderer.GetModel3D(childDictionary);
                    if (!ReferenceEquals(null, m3d)) model3DGroup.Children.Add(m3d);
                }
            }
            return model3DGroup;
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
        public static Transform3D ToTransform3D(IRenderer renderer, System.Collections.IDictionary value)
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
        public static Vector3D ToScale(IRenderer renderer,IDictionary value)
        {
            Vector3D result = new Vector3D(1, 1, 1);
            if (value.Contains("ScaleX"))
            {
                result.X = System.Convert.ToDouble(value["ScaleX"].ToString());
            }
            if(value.Contains("Length"))
            {
                result.X = System.Convert.ToDouble(value["Length"].ToString());
            }
            if (value.Contains("ScaleY"))
            {
                result.Y = System.Convert.ToDouble(value["ScaleY"].ToString());
            }
            if(value.Contains("Width"))
            {
                result.Y = System.Convert.ToDouble(value["Width"].ToString());
            }
            if (value.Contains("ScaleZ"))
            {
                result.Z = System.Convert.ToDouble(value["ScaleZ"].ToString());
            }
            if(value.Contains("Height"))
            {
                result.Z = System.Convert.ToDouble(value["Height"].ToString());
            }
            return result;
        }
        public static Material ToMaterial(IRenderer renderer, IDictionary value)
        {
            Material material = null;
            if (value.Contains("Material"))
            {
                material = renderer.Resources[value["Material"].ToString()] as Material;
            }
            return material;
        }
        public static Material ToBackMaterial(IRenderer renderer, IDictionary value)
        {
            Material material = null;
            if (value.Contains("BackMaterial"))
            {
                material = renderer.Resources[value["BackMaterial"].ToString()] as Material;
            }
            return material;
        }
    }
}
