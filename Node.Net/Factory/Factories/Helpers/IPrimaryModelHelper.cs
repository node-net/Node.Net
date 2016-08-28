using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.Helpers
{
    public static class IPrimaryModelHelper
    {
        class PrimaryModel : IPrimaryModel { public Model3D Model3D { get; set; } = null; }
        public static IPrimaryModel FromIDictionary(IDictionary source,IFactory factory)
        {
            if (source == null) return null;

            var itypeName = factory.Create<ITypeName>(source);
            if (itypeName != null)
            {
                var typeName = itypeName.TypeName;// factory.Create<ITypeName>(source).TypeName;
                var model3D = factory.Create<Model3D>(typeName);
                if (model3D != null)
                {
                    var material = GetMaterial(source, factory);
                    if (material != null)
                    {
                        var geoModel = model3D as GeometryModel3D;
                        if (geoModel != null)
                        {
                            geoModel.Material = material;
                        }
                    }
                    var model3DGroup = new Model3DGroup { Transform = GetTransform(source, factory) };
                    model3DGroup.Children.Add(model3D);
                    return new PrimaryModel { Model3D = model3DGroup };
                }
            }
            return null;
        }
        private static string GetTypeName(IDictionary source,IFactory factory)
        {
       
            var typeName = string.Empty;
            if (factory != null)
            {
                var iTypeName = factory.Create<ITypeName>(source);
                if (iTypeName != null) typeName = iTypeName.TypeName;
            }
            return typeName;
        }

        private static Transform3D GetTransform(IDictionary source,IFactory factory)
        {
            IScale iscale = null;

            if (factory != null)
            {
                iscale = factory.Create<IScale>(source);
            }

            if (iscale != null)
            {
                return new ScaleTransform3D { ScaleX = iscale.Scale.X, ScaleY = iscale.Scale.Y, ScaleZ = iscale.Scale.Z };
            }
            return null;
        }

        private static Model3D GetModel3D(string typeName,IFactory factory)
        {

            if (factory != null)
            {
                return factory.Create<Model3D>(typeName);
            }

            return null;
        }

        private static Material GetMaterial(IDictionary source,IFactory factory)
        {

            if (factory != null)
            {
                return factory.Create<Material>(source);
            }
            return null;
        }
    }
}
