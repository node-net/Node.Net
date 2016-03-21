using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D.Transform
{
    class ObjectTransform
    {
        public static Visual3D ToVisual3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = Json.KeyValuePair.GetValue(value) as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToVisual3D(renderer, dictionary);
            return null;
        }
        public static ModelVisual3D ToModelVisual3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = Json.KeyValuePair.GetValue(value) as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToModelVisual3D(renderer, dictionary);
            return null;
        }
        public static System.Windows.Media.Media3D.Model3D ToModel3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = Json.KeyValuePair.GetValue(value) as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToModel3D(renderer, dictionary);
            return null;
        }
        public static GeometryModel3D ToGeometryModel3D(IRenderer renderer, object value)
        {
            return IDictionaryTransform.ToGeometryModel3D(renderer, value as IDictionary);
        }
        public static System.Windows.Media.Media3D.Transform3D ToTransform3D(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToTransform3D(renderer, dictionary);
            return null;
        }
        public static Material ToBackMaterial(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToBackMaterial(renderer, dictionary);
            return null;
        }
        public static Material ToMaterial(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToMaterial(renderer, dictionary);
            return null;
        }
        public static Vector3D ToTranslation(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToTranslation(renderer, dictionary);
            return new Vector3D(0, 0, 0);
        }
        public static Vector3D ToScale(IRenderer renderer, object value)
        {
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary)) return IDictionaryTransform.ToScale(renderer, dictionary);
            return new Vector3D(0, 0, 0);
        }
    }
}
