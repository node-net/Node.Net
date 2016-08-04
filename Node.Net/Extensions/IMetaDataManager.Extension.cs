using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public class IMetaDataManagerExtension
    {
        public static void SetTransformMetaData(IMetaDataManager metaData, IDictionary dictionary)
        {
            if (ReferenceEquals(null, dictionary)) return;
            var parent = metaData.GetMetaData(dictionary, "Parent");

            var transform3D = new Node.Net.Model3D.Transform3D();
            var translation = Node.Net.Model3D.RenderHelper.GetTranslationMeters(dictionary);
            transform3D.Translation = new Point3D(translation.X, translation.Y, translation.Z);
            transform3D.RotationOTS = Node.Net.Model3D.RenderHelper.GetRotationOTSDegrees(dictionary as IDictionary);

            metaData.SetMetaData(dictionary, "Transform3D", transform3D);


            if (!ReferenceEquals(null, parent))
            {
                var parentTransform = metaData.GetMetaData(parent, "Transform3D") as Node.Net.Model3D.Transform3D;
                transform3D.Parent = parentTransform;
            }
        }
    }
}
