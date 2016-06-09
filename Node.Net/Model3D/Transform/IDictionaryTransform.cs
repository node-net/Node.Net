using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D.Transform
{
    class IDictionaryTransform
    {
        public static Visual3D ToVisual3D(IRenderer renderer, IDictionary value)
        {
            if (value.Contains("Visual3D"))
            {
                //return renderer.Resources[value["Visual3D"].ToString()] as Visual3D;
                return renderer.Resources.GetResource(value["Visual3D"].ToString()) as Visual3D;
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
            string stype = "";
            if (value.Contains("Type")) stype = value["Type"].ToString();
            if (stype.Length > 0 && renderer.TypeModel3DTransformers.ContainsKey(stype))
            {
                return renderer.TypeModel3DTransformers[stype].GetModel3D(value);
            }
            
            return ToModel3DGroup(renderer, value);
        }
        public static System.Windows.Media.Media3D.Model3DGroup ToModel3DGroup(IRenderer renderer, IDictionary value)
        {
            string stype = "";
            if (value.Contains("Type")) stype = value["Type"].ToString();
            if (stype.Length > 0 && renderer.TypeModel3DGroupTransformers.ContainsKey(stype))
            {
                return renderer.TypeModel3DGroupTransformers[stype].GetModel3DGroup(value);
            }

            Model3DGroup model3DGroup = new Model3DGroup();
            model3DGroup.Transform = ToTransform3D_NoScale(renderer, value);
            // NoScale, but has Translation AND Rotations

            // Primary Model
            System.Windows.Media.Media3D.Model3D primaryModel = null;
            foreach (string modelKey in renderer.Model3DKeys)
            {
                if (object.ReferenceEquals(null, primaryModel))
                {
                    if (value.Contains(modelKey))
                    {
                        string modelKeyValue = value[modelKey].ToString();
                        System.Windows.Media.Media3D.Model3D modelResource = renderer.GetResource(modelKeyValue) as System.Windows.Media.Media3D.Model3D;
                        if (!object.ReferenceEquals(null, modelResource))
                        {
                            Model3DGroup modelGroup = new Model3DGroup();
                            modelGroup.Transform = new ScaleTransform3D(renderer.GetScale(value));
                            modelGroup.Children.Add(modelResource);
                            primaryModel = modelGroup;
                        }
                        
                    }
                }
            }
            if (object.ReferenceEquals(null, primaryModel))
            {
                primaryModel = ToGeometryModel3D(renderer, value);
            }
            if (!object.ReferenceEquals(null, primaryModel))
            {
                model3DGroup.Children.Add(primaryModel);
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
                geometry = renderer.GetResource(value["Geometry"].ToString()) as Geometry3D;
                //geometry = renderer.Resources[value["Geometry"].ToString()] as Geometry3D;
            }
            if (value.Contains("GeometryModel3D"))
            {
                geometry = renderer.GetResource(value["GeometryModel3D"].ToString()) as Geometry3D;
                //geometry = renderer.Resources[value["GeometryModel3D"].ToString()] as Geometry3D;
            }
            if (!ReferenceEquals(null, geometry))
            {
                return new GeometryModel3D()
                {
                    Geometry = geometry,
                    Material = renderer.GetMaterial(value),
                    BackMaterial = renderer.GetBackMaterial(value),
                    Transform = new ScaleTransform3D(ToScale(renderer, value))// ToTransform3D(renderer, value)// renderer.GetTransform3D(value)
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
        public static System.Windows.Media.Media3D.Transform3D ToTransform3D_NoScale(IRenderer renderer, System.Collections.IDictionary value)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(GetRotateTransform3D(value));
            TranslateTransform3D translateTransform = new TranslateTransform3D(renderer.GetTranslation(value));
            Vector3D translation = renderer.GetTranslation(value);
            Point3D center = new Point3D(translation.X, translation.Y, translation.Z);
            transformGroup.Children.Add(new TranslateTransform3D(renderer.GetTranslation(value)));
            return transformGroup;
        }
        public static RotateTransform3D GetRotateTransform3D(System.Collections.IDictionary value)
        {
            Quaternion rotationZ = new Quaternion();
            Quaternion rotationY = new Quaternion();
            Quaternion rotationX = new Quaternion();
            QuaternionRotation3D rotation = new QuaternionRotation3D();
            if (value.Contains("RotationZ"))
            {
                double rotationZ_degrees = GetRotationDegrees(value, "RotationZ");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("Orientation"))
            {
                double rotationZ_degrees = GetRotationDegrees(value, "Orientation");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("RotationY"))
            {
                double rotationY_degrees = GetRotationDegrees(value, "RotationY");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("Tilt"))
            {
                double rotationY_degrees = GetRotationDegrees(value, "Tilt");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("RotationX"))
            {
                double rotationX_degrees = GetRotationDegrees(value, "RotationX");
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }
            if (value.Contains("Spin"))
            {
                double rotationX_degrees = GetRotationDegrees(value, "Spin");
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }

            Quaternion total_rotation = Quaternion.Multiply(rotationX, Quaternion.Multiply(rotationY, rotationZ));
            return new RotateTransform3D(new QuaternionRotation3D(total_rotation));
        }

        public static Vector3D ToTranslation(IRenderer renderer, IDictionary value)
        {
            Vector3D result = new Vector3D();
            if (value.Contains("X"))
            {
                result.X = GetLengthMeters(value, "X");
            }
            if (value.Contains("Y"))
            {
                result.Y = GetLengthMeters(value, "Y"); ;
            }
            if (value.Contains("Z"))
            {
                result.Z = GetLengthMeters(value, "Z");
            }
            return result;
        }

        private static double GetLengthMeters(IDictionary dictionary, string key)
        {
            return Measurement.Length.Parse(dictionary[key].ToString())[Measurement.LengthUnit.Meters];
        }
        private static double GetRotationDegrees(IDictionary dictionary, string key)
        {
            if (object.ReferenceEquals(null, dictionary)) return 0;
            if (!dictionary.Contains(key)) return 0;
            if (object.ReferenceEquals(null, dictionary[key])) return 0;
            return Measurement.Angle.Parse(dictionary[key].ToString())[Measurement.AngularUnit.Degrees];
        }
        public static Vector3D ToScale(IRenderer renderer, IDictionary value)
        {
            Vector3D result = new Vector3D(1, 1, 1);
            if (value.Contains("ScaleX"))
            {
                result.X = GetLengthMeters(value, "ScaleX");
            }
            if (value.Contains("Length"))
            {
                result.X = GetLengthMeters(value, "Length");
            }
            if (value.Contains("ScaleY"))
            {
                result.Y = GetLengthMeters(value, "ScaleY");
            }
            if (value.Contains("Width"))
            {
                result.Y = GetLengthMeters(value, "Width");
            }
            if (value.Contains("ScaleZ"))
            {
                result.Z = GetLengthMeters(value, "ScaleZ");
            }
            if (value.Contains("Height"))
            {
                result.Z = GetLengthMeters(value, "Height");
            }
            return result;
        }
        public static Material ToMaterial(IRenderer renderer, IDictionary value)
        {
            Material material = null;
            if (value.Contains("Material"))
            {
                //material = renderer.Resources[value["Material"].ToString()] as Material;
                material = renderer.GetResource(value["Material"].ToString()) as Material;
            }
            return material;
        }
        public static Material ToBackMaterial(IRenderer renderer, IDictionary value)
        {
            Material material = null;
            if (value.Contains("BackMaterial"))
            {
                material = renderer.GetResource(value["BackMaterial"].ToString()) as Material;
                //material = renderer.Resources[value["BackMaterial"].ToString()] as Material;
            }
            return material;
        }
    }
}
