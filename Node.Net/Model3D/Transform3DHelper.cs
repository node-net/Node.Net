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
            return MetaData.GetMetaData(value, nameof(Transform3D)) as Transform3D;
        }
        private void Traverse(IDictionary dictionary)
        {
            if (!ReferenceEquals(null, dictionary))
            {
                Extensions.IMetaDataManagerExtension.SetTransformMetaData(MetaData, dictionary);
                foreach(string key in dictionary.Keys)
                {
                    Extensions.IMetaDataManagerExtension.SetTransformMetaData(MetaData, dictionary[key] as IDictionary);
                }
            }
        }

        private readonly Deprecated.Collections.MetaDataManager _metaData = new Deprecated.Collections.MetaDataManager();
        private Deprecated.Collections.MetaDataManager MetaData
        {
            get { return _metaData; }
        }
    }
}
