using System;
using System.Collections;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D.Render
{
    public class Renderer : IRenderer
    {
        private ResourceDictionary resources = new ResourceDictionary();
        public ResourceDictionary Resources
        {
            get { return resources; }
            set { resources = value; }
        }

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
