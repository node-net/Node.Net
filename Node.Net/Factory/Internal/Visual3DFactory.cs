using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class Visual3DFactory : IFactory
    {
        public IFactory Factory
        {
            get { return Node.Net.Factory.Factory.Default; }
        }
        public object Create(Type type, object value)
        {
            var typeName = Factory.Create<ITypeName>(value).TypeName;
            if (typeName.Length > 0)
            {
                var model3D = Factory.Create<Model3D>(typeName);

                if (model3D != null) return Create(model3D.Clone());
            }
            if (typeof(Model3D).IsAssignableFrom(value.GetType())) return Create(value as Model3D);
            //if (value.GetType() == typeof(GeometryModel3D)) return Create(value as GeometryModel3D);
            if (value.GetType() == typeof(IDictionary)) return Create(value as IDictionary);
            var geometryModel3D = Factory.Create<GeometryModel3D>(value);
            if (geometryModel3D != null) return Create(geometryModel3D);
            return null;
        }

        private Visual3D Create(Model3D model)
        {
            return new ModelVisual3D { Content = model };
        }
        private Visual3D Create(IDictionary dictionary)
        {
            //var typeName = Factory.Default.Create<ITypeName>(dic)
            return null;
        }
    }
}
