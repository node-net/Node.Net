using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public class Renderer : IRenderer
    {
        public Renderer()
        {
            Model3DKeys.Add("Model3D");
            Model3DKeys.Add("Type");
        }

        private ResourceDictionary resources = new ResourceDictionary();
        public ResourceDictionary Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        public virtual object GetResource(string name)
        {
            if (resources.Contains(name)) return resources[name];
            return null;
        }

        private List<string> model3DKeys = new List<string>();
        public List<string> Model3DKeys { get { return model3DKeys; } }

        public virtual Visual3D GetVisual3D(object value)
        {
            return Transform.ObjectTransform.ToVisual3D(this, value);
        }

        public virtual ModelVisual3D GetModelVisual3D(object value)
        {
            return Transform.ObjectTransform.ToModelVisual3D(this, value);
        }

        
        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(object value)
        {
            return Transform.ObjectTransform.ToModel3D(this, value);
        }

        public virtual GeometryModel3D GetGeometryModel3D(object value)
        {
            return Transform.ObjectTransform.ToGeometryModel3D(this, value);
        }
        
        public virtual Transform3D GetTransform3D(object value)
        {
            return Transform.ObjectTransform.ToTransform3D(this, value);
        }
        public virtual Vector3D GetTranslation(object value)
        {
            return Transform.ObjectTransform.ToTranslation(this, value);
        }
        public virtual Vector3D GetScale(object value)
        {
            return Transform.ObjectTransform.ToScale(this,value);
        }
        public virtual Material GetMaterial(object value)
        {
            return Transform.ObjectTransform.ToMaterial(this, value);
        }
        public virtual Material GetBackMaterial(object value)
        {
            return Transform.ObjectTransform.ToBackMaterial(this, value);
        }
    }
}
