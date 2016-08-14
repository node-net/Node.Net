using System;
using System.Collections;


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
            if (value == null) return null;
            var modelVisual3D = new System.Windows.Media.Media3D.ModelVisual3D();
            var typeName = Factory.Create<ITypeName>(value).TypeName;
            if (typeName.Length > 0)
            {
                var model3D = Factory.Create<System.Windows.Media.Media3D.Model3D>(typeName);

                if (model3D != null)
                {
                    var v3d =  Create(model3D);
                    if (v3d != null)
                    {
                        v3d.Transform = Factory.Create<System.Windows.Media.Media3D.Transform3D>(value);
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
                    var v3dc = Factory.Create<System.Windows.Media.Media3D.Visual3D>(dictionary[key]);
                    if (v3dc != null) modelVisual3D.Children.Add(v3dc);
                }
            }
            if (modelVisual3D.Children.Count > 0) return modelVisual3D;
            if (typeof(System.Windows.Media.Media3D.Model3D).IsAssignableFrom(value.GetType())) return Create(value as System.Windows.Media.Media3D.Model3D);
            if (value.GetType() == typeof(IDictionary)) return Create(value as IDictionary);
            var geometryModel3D = Factory.Create<System.Windows.Media.Media3D.GeometryModel3D>(value);
            if (geometryModel3D != null) return Create(geometryModel3D);
            return null;
        }


        private static System.Windows.Media.Media3D.Visual3D Create(System.Windows.Media.Media3D.Model3D model)
        {
            return new System.Windows.Media.Media3D.ModelVisual3D { Content = model };
        }
        private static System.Windows.Media.Media3D.Visual3D Create(IDictionary dictionary)
        {
            //var typeName = Factory.Default.Create<ITypeName>(dic)
            return null;
        }
    }
}
