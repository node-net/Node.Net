using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections.Test
{
    [TestFixture]
    class IDictionaryExtensionTestExplicit
    {
        [Test,Explicit]
        public void IDictionaryExtensionExplicit_Get()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["ExitCode"] = 1;
            Assert.AreEqual(1, IDictionaryExtension.Get<int>(dictionary, "ExitCode"));
            var dateTime = DateTime.Now;
            dictionary["StartTime"] = dateTime.ToString("o");
            Assert.AreEqual(dateTime, IDictionaryExtension.Get<DateTime>(dictionary, "StartTime"));

            Assert.AreEqual(default(int), IDictionaryExtension.Get<int>(dictionary, "Bogus"));

            dictionary["NullInt"] = null;
            Assert.AreEqual(default(int), IDictionaryExtension.Get<int>(dictionary, "NullInt"));

            dictionary = new Dictionary<string, dynamic>();
            var foo = new Dictionary<string, dynamic>();
            foo["Type"] = "Foo";
            dictionary["Type"] = "Bar";
            dictionary["foo"] = foo;
            Assert.IsNull(IDictionaryExtension.Get<object>(dictionary, "?"));
            Assert.AreEqual("Bar", IDictionaryExtension.Get<string>(dictionary, "Type"));
            Assert.AreEqual("Foo", IDictionaryExtension.Get<string>(dictionary, "foo/Type"));
        }

        [Test,Explicit]
        public void IDictionaryExtensionExplicit_Set()
        {
            var dictionary = new Dictionary<string, dynamic>();

            IDictionaryExtension.Set(dictionary, "ExitCode", null);
            Assert.IsNull(dictionary["ExitCode"]);

            IDictionaryExtension.Set(dictionary, "ExitCode", 1);
            Assert.AreEqual(1, dictionary["ExitCode"]);

            var dateTime = DateTime.Now;
            IDictionaryExtension.Set(dictionary, "StartTime", dateTime);
            Assert.AreEqual(dateTime.ToString("o"), dictionary["StartTime"].ToString());

            var commands = new Dictionary<string, dynamic>();
            IDictionaryExtension.Set(commands, "Type", "Commands");
            Assert.AreEqual("Commands", commands["Type"]);
            var start = DateTime.Now;
            IDictionaryExtension.Set(commands, "command0/StartTime", start);
            var command0 = commands["command0"] as IDictionary;
            Assert.NotNull(command0);
            Assert.True(command0.Contains("StartTime"));

            dictionary = new Dictionary<string, dynamic>();
            IDictionaryExtension.Set(dictionary, "Type", "Bar");
            Assert.AreEqual("Bar", dictionary["Type"]);
            IDictionaryExtension.Set(dictionary, "foo/Type", "Foo");
            Assert.True(dictionary.ContainsKey("foo"));
            var foo = dictionary["foo"] as IDictionary;
            Assert.NotNull(foo);
        }

        class Child : Dictionary<string, dynamic> { public object Parent { get; set; } }
        [Test,Explicit]
        public void IDictionaryExtension_Collect()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["child"] = new Dictionary<string, dynamic>();// new Child();
            var children = IDictionaryExtension.Collect<IDictionary>(dictionary);
            Assert.AreEqual(1, children.Count);
            Assert.AreSame(dictionary, ObjectExtension.GetParent(dictionary["child"] as IDictionary));

            // Use LINQ
            //var result = dict1.Where(kvp => kvp.Value.subs.Any(ss => ss.Id == 3));
            var result = dictionary.Where(kvp => kvp.Value.GetType() == typeof(Child)).ToDictionary(x => x.Key, x => x.Value);
            Assert.AreEqual(1, children.Count);
            Assert.AreSame(dictionary, ObjectExtension.GetParent(dictionary["child"] as IDictionary));
        }
    }
}
