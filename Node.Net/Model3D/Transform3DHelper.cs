using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public class Transform3DHelper
    {
        public void Clear() { _metaData.Clear(); }
        public void Traverse(object value)
        {
            Traverse(value as IDictionary);
        }

        public Transform3D GetTransform3D(object value)
        {
            return MetaData.GetMetaData(value, "Transform3D") as Transform3D;
        }
        private void Traverse(IDictionary dictionary)
        {
            if (!object.ReferenceEquals(null, dictionary))
            {
                SetTransformMetaData(dictionary);
            }
        }

        private readonly  Collections.MetaData _metaData = new Collections.MetaData();
        private Collections.MetaData MetaData
        {
            get { return _metaData; }
        }
        private void SetTransformMetaData(IDictionary dictionary)
        {
            if (object.ReferenceEquals(null, dictionary)) return;

            var transform3D = new Node.Net.Model3D.Transform3D();
            var translation = Node.Net.Model3D.RenderHelper.GetTranslationMeters(dictionary);
            transform3D.Translation = new Point3D(translation.X, translation.Y, translation.Z);
            transform3D.RotationOTS = Node.Net.Model3D.RenderHelper.GetRotationOTSDegrees(dictionary as IDictionary);

            MetaData.SetMetaData(dictionary, "Transform3D", transform3D);

            var parent = MetaData.GetMetaData(dictionary, "Parent");
            if (!object.ReferenceEquals(null, parent))
            {
                var parentTransform = MetaData.GetMetaData(parent, "Transform3D") as Node.Net.Model3D.Transform3D;
                transform3D.Parent = parentTransform;
            }

            foreach (string key in dictionary.Keys)
            {
                var childDictionary = dictionary[key] as IDictionary;
                if (!object.ReferenceEquals(null, childDictionary))
                {
                    SetTransformMetaData(childDictionary);
                }
            }
        }
    }
}
