using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net
{
    [TestFixture]
    class DocumentTest
    {
        [Test]
        public void Document_Usage()
        {
            var doc = Document.Open("States.json");
            Assert.NotNull(doc, nameof(doc));

            foreach(var key in doc.Keys)
            {
                var child = doc[key] as IDictionary;
                if(child != null)
                {
                    var parent = child.GetParent();
                    Assert.NotNull(parent, nameof(parent));
                    var name = child.GetName();
                    Assert.AreEqual(name, key);
                }
            }
        }
    }
}
