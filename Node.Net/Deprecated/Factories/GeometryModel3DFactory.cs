using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Factories
{
    public sealed class GeometryModel3DFactory : Generic.TargetTypeFactory<GeometryModel3D>
    {
        public override GeometryModel3D Create(object source)
        {
            if (source != null)
            {
                /*
                if(typeof(Node.Net.Factories.IElement).IsAssignableFrom(source.GetType()))
                {
                    var instance = CreateFromIElement(source as Node.Net.Factories.IElement);
                    if (instance != null) return instance;
                }*/
                if (typeof(IDictionary).IsAssignableFrom(source.GetType()))
                {
                    var instance = CreateFromDictionary(source as IDictionary);
                    if (instance != null) return instance;
                }
            }


            return null;
        }

        private bool _helperCreating = false;
        private object HelperCreate(Type targetType, object source)
        {
            if (_helperCreating) return null;
            try
            {
                _helperCreating = true;
                if (Helper != null)
                {
                    return Helper.Create(targetType, source);
                }
            }
            finally
            {
                _helperCreating = false;
            }
            return null;
        }
        private GeometryModel3D CreateFromDictionary(IDictionary source)
        {
            if (source.Contains("Type"))
            {
                var mesh = HelperCreate(typeof(MeshGeometry3D), source["Type"].ToString()) as MeshGeometry3D;
                if (mesh != null)
                {
                    var materialName = "Blue";
                    var backMaterialName = "Yellow";
                    var dictionary = source as IDictionary;
                    if (dictionary != null)
                    {
                        if (dictionary.Contains("Material")) materialName = dictionary["Material"].ToString();
                        if (dictionary.Contains("BackMaterial")) backMaterialName = dictionary["BackMaterial"].ToString();
                    }
                    return new GeometryModel3D
                    {
                        Geometry = mesh,
                        Material = Helper.Create(typeof(Material), materialName) as Material,
                        BackMaterial = Helper.Create(typeof(Material), backMaterialName) as Material,
                        Transform = GetTransform(source)
                    };
                }
            }
            return null;
        }

        private static Transform3D GetTransform(IDictionary source)
        {
            var scaleX = 1.0;
            var scaleY = 1.0;
            var scaleZ = 1.0;

            var tmp = source.GetLengthMeters("Height");// Extension.IDictionaryExtension.GetLengthMeters(source, "Height");
            if (tmp != 0.0) scaleZ = tmp;
            tmp = source.GetLengthMeters("Width");// Extension.IDictionaryExtension.GetLengthMeters(source, "Width");
            if (tmp != 0.0) scaleY = tmp;
            tmp = source.GetLengthMeters("Length");// Extension.IDictionaryExtension.GetLengthMeters(source, "Length");
            if (tmp != 0.0) scaleX = tmp;
            if (scaleX != 1.0 || scaleY != 1.0 || scaleZ != 1.0)
            {
                var matrix3D = new Matrix3D();
                matrix3D.Scale(new Vector3D(scaleX, scaleY, scaleZ));
                return new MatrixTransform3D { Matrix = matrix3D };
            }
            return null;
        }
        /*
        private GeometryModel3D CreateFromIElement(Node.Net.Factories.IElement source)
        {
            if (source.Contains("Type"))
            {
                var mesh = HelperCreate(typeof(MeshGeometry3D), source.Get("Type").ToString()) as MeshGeometry3D;
                if (mesh != null)
                {
                    var materialName = "Blue";
                    var backMaterialName = "Yellow";
                    //var dictionary = source as IDictionary;
                    //if (dictionary != null)
                    //{
                        if (source.Contains("Material")) materialName = source.Get("Material").ToString();
                        if (source.Contains("BackMaterial")) backMaterialName = source.Get("BackMaterial").ToString();
                   // }
                    return new GeometryModel3D
                    {
                        Geometry = mesh,
                        Material = Helper.Create(typeof(Material), materialName) as Material,
                        BackMaterial = Helper.Create(typeof(Material), backMaterialName) as Material,
                        Transform = GetTransform(source)
                    };
                }
            }
            return null;
        }*/
        /*
        private Transform3D GetTransform(Node.Net.Factories.IElement source)
        {
            var scaleX = 1.0;
            var scaleY = 1.0;
            var scaleZ = 1.0;

            var tmp = source.GetLengthMeters("Height");
            if (tmp != 0.0) scaleZ = tmp;
            tmp = source.GetLengthMeters("Width");
            if (tmp != 0.0) scaleY = tmp;
            tmp = source.GetLengthMeters( "Length");
            if (tmp != 0.0) scaleX = tmp;
            if (scaleX != 1.0 || scaleY != 1.0 || scaleZ != 1.0)
            {
                var matrix3D = new Matrix3D();
                matrix3D.Scale(new Vector3D(scaleX, scaleY, scaleZ));
                return new MatrixTransform3D { Matrix = matrix3D };
            }
            return null;
        }*/
    }
}
