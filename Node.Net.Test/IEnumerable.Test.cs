using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    internal class IEnumerableTest
    {
        [Test]
        public void GetAt()
        {
            var items = new List<int> { 0, 1, 2, 3 };
            Assert.IsNull(items.GetAt(99));
            Assert.AreEqual(3, items.GetAt(3));
        }
    }
}
