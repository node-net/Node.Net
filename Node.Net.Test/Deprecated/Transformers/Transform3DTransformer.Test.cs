using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Transformers
{
    [TestFixture, Category("Tranformers.Transform3DTransformer")]
    class Transform3DTransformerTest
    {
        [Test]
        public void Transform3D_Usage()
        {
            var t = new Transform3DTransformer();
            var data = new Dictionary<string, dynamic>();
            data["X"] = "1 m";
            var transform = Transform3DTransformer.Transform(data) as Transform3D;
            Assert.NotNull(transform);

        }
    }
}
