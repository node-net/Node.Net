using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class Model3DFactory : IFactory
    {
        public IFactory Factory
        {
            get { return Node.Net.Factory.Factory.Default; }
        }
        public object Create(Type type, object value)
        {
            if (value == null) return null;
            /*
            var model3DGroup = new Model3DGroup();
            var typeName = Factory.Create<ITypeName>(value).TypeName;
            if (typeName.Length > 0)
            {
                var geometry = Factory.Create<Geometry3D>(typeName);
                if (geometry != null)
                {
                    var v3d = Create(model3D);
                    if (v3d != null)
                    {
                        v3d.Transform = Factory.Create<Transform3D>(value);
                        modelVisual3D.Children.Add(v3d);
                    }
                }
                //return modelVisual3D;
            }
            // Children
            var dictionary = value as IDictionary;
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    var v3dc = Factory.Create<Visual3D>(dictionary[key]);
                    if (v3dc != null) modelVisual3D.Children.Add(v3dc);
                }
            }
            if (modelVisual3D.Children.Count > 0) return modelVisual3D;
            if (typeof(Model3D).IsAssignableFrom(value.GetType())) return Create(value as Model3D);
            if (value.GetType() == typeof(IDictionary)) return Create(value as IDictionary);
            var geometryModel3D = Factory.Create<GeometryModel3D>(value);
            if (geometryModel3D != null) return Create(geometryModel3D);
            */
            return null;
        }


    }
}
