using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public class Renderer : IRenderer, IVisual3DTransformer,IModel3DTransformer,IModel3DGroupTransformer,IResources
    {
        public Renderer()
        {
            Model3DKeys.Add(nameof(Model3D));
            Model3DKeys.Add("Type");
        }

        private IMetaDataManager _metaData;
        public IMetaDataManager MetaData
        {
            get
            {
                if(ReferenceEquals(null,_metaData))
                {
                    _metaData = Deprecated.Collections.MetaDataManager.Default;
                }
                return _metaData;
            }
            set
            {
                _metaData = value;
            }
        }

        private readonly Dictionary<string, IModel3DTransformer> _typeModel3DTransformers = new Dictionary<string, IModel3DTransformer>();
        public Dictionary<string, IModel3DTransformer> TypeModel3DTransformers { get { return _typeModel3DTransformers; } }

        private readonly Dictionary<string, IModel3DGroupTransformer> _typeModel3DGroupTransformers = new Dictionary<string, IModel3DGroupTransformer>();
        public Dictionary<string, IModel3DGroupTransformer> TypeModel3DGroupTransformers { get { return _typeModel3DGroupTransformers; } }

        public IResources Resources { get; set; }



        public virtual object GetResource(string name)
        {
            if (Resources != null) return Resources.GetResource(name);
            //if (Resources.Contains(name)) return Resources[name];
            return null;
        }

        private readonly List<string> model3DKeys = new List<string>();
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

        public virtual System.Windows.Media.Media3D.Model3DGroup GetModel3DGroup(object value)
        {
            return Transform.ObjectTransform.ToModel3DGroup(this, value);
        }

        public virtual GeometryModel3D GetGeometryModel3D(object value)
        {
            return Transform.ObjectTransform.ToGeometryModel3D(this, value);
        }

        public virtual System.Windows.Media.Media3D.Transform3D GetTransform3D(object value)
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
