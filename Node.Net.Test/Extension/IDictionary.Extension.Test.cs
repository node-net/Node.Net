using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    [TestFixture]
    class IDictionaryExtensionTest
    {
        [Test]
        public void IDictionary_Extension_GetLocalToParent()
        {
            var dictionary = new Dictionary<string, dynamic>
            {
                {"X","10 m" }
            };
            var matrix = dictionary.GetLocalToParent();
            Assert.NotNull(matrix, nameof(matrix));
            Assert.False(matrix.IsIdentity);
            var parentOrigin = matrix.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(10, parentOrigin.X);
        }

        [Test]
        public void IDictionary_Extension_GetLocalToWorld()
        {
            var foo = new Dictionary<string, dynamic>
            {
                {"X","1 m" }
            };
            var dictionary = new Dictionary<string,dynamic>
            {
                {"X","10 m" },
                {"foo",foo}
            };
            dictionary.DeepUpdateParents();
            var matrix = foo.GetLocalToWorld();
            Assert.NotNull(matrix, nameof(matrix));
            Assert.False(matrix.IsIdentity);
            var worldOrigin = matrix.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(11, worldOrigin.X);
        }
    }
}
