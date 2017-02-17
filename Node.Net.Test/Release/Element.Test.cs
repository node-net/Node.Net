using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    [TestFixture]
    class ElementTest
    {
        [Test]
        public void Element_Parent()
        {
            var elementA = new Element();
            var elementB = new Element();
            elementB.Parent = elementA;
            Assert.AreSame(elementA, elementB.Parent);
        }
    }
}
