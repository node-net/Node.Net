using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    [TestFixture]
    class IDocumentTest
    {
        [Test]
        public void IDocument_Usage()
        {
            var document = new Document();
            Assert.True(typeof(Node.Net.IElement).IsAssignableFrom(document.GetType()));
            //Assert.True(typeof(Node.Net.Readers.IElement).IsAssignableFrom(document.GetType()));

            document.Load("States.json");
            
            Assert.AreEqual("States.json", document.FileName);
            Assert.AreEqual(50, document.Count);
            Assert.AreEqual(12720, document.GetDeepCount());
            var json = document.JSON;
            Assert.True(json.Contains("Colorado"));
            Assert.True(json.Contains("Jefferson"));

            var colorado = document.Get<IElement>("Colorado");
            Assert.NotNull(colorado, nameof(colorado));
            Assert.AreSame(document, colorado.Parent, "colorado.Parent");
            Assert.AreEqual("Colorado", colorado.Name);
            Assert.AreSame(document, colorado.Parent);
            //Assert.AreSame(document, colorado.Document);

            var states = document.Find<IElement>("State");
            Assert.AreEqual(50, states.Count);
        }

    }
}
