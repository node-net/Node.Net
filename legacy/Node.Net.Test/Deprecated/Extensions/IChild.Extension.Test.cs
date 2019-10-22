using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Deprecated.Extensions
{
    [TestFixture,Category("Extensions")]
    class IChildExtensionTest
    {
        [Test]
        public void IChildExtension_GetNearestAncestor()
        {
            var a = new Model.Element();
            var b = new Model.Element();
            var c = new Model.Element();
            a["b"] = b;
            b["c"] = c;
            a.DeepCollect<Model.Element>();

            Assert.AreSame(a, b.GetNearestAncestor<Model.Element>());
        }
        [Test]
        public void IChildExtension_GetFurthestAncestor()
        {
            var a = new Model.Element();
            var b = new Model.Element();
            var c = new Model.Element();
            a["b"] = b;
            b["c"] = c;
            a.DeepCollect<Model.Element>();

            Assert.NotNull(c.Parent,"c.Parent is null");
            Assert.AreSame(a, c.GetFurthestAncestor<Model.Element>());
        }
    }
}
