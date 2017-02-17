using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Node.Net.Deprecated.Model
{
    [TestFixture,Category(nameof(Model))]
    class ElementTest
    {
        [Test]
        public void Element_DeepCollect_From_Root()
        {
            var root = new Element();
            var a = new Element();
            var b = new Element();
            var c = new SpatialElement();
            root.Add("a", a);
            a.Add("b", b);
            b.Add("c", c);
            root.DeepCollect<IChild>();
            c.GetFurthestAncestor<Node.Net.IParent>().DeepCollect<SpatialElement>();
            c.GetRoot().DeepCollect<SpatialElement>();
        }
    }
}
