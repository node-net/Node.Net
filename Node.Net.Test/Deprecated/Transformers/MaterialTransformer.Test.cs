using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Transformers
{
    [TestFixture,Category("Transformers.MaterialTransformer")]
    class MaterialTransformerTest
    {
        [Test]
        public void MaterialTransformer_Usage()
        {
            var t = new MaterialTransformer();
            var m = MaterialTransformer.Transform("Blue") as Material;
            Assert.NotNull(m);
        }
    }
}
