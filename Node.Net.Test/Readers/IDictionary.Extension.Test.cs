using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers.Test
{
    [TestFixture]
    class IDictionaryExtensionTest
    {
        class Widget : Dictionary<string, dynamic> { }
        class Foo : Dictionary<string, dynamic> { }
        [Test]
        public void IDictionary_ConvertTypes()
        {
            var idictionary = IReadExtension.Read(
                Reader.Default,
                typeof(IDictionaryExtensionTest).Assembly,
                "simple.json") as IDictionary;
            Assert.NotNull(idictionary, nameof(idictionary));
            var widget = idictionary["widget"] as IDictionary;
            Assert.True(widget.Contains("string_array"));
            Assert.AreSame(typeof(string[]), widget["string_array"].GetType(), "widget/string_array from json read");

            var types = new Dictionary<string, Type>();
            types.Add("Widget", typeof(Widget));
            types.Add("Foo", typeof(Foo));


            //idictionary["string_array"] = new List<string> { "A", "B", "C" }.ToArray();

            var converted = IDictionaryExtension.ConvertTypes(
                idictionary, types) as IDictionary;
            widget = converted["widget"] as IDictionary;
            Assert.AreSame(typeof(Widget), widget.GetType());
            var foo = widget["foo"];
            Assert.AreSame(typeof(Foo), foo.GetType());
            Assert.True(widget.Contains("string_array"));
            Assert.AreSame(typeof(string[]), widget["string_array"].GetType(),"widget/string_array converted");
        }
    }
}
