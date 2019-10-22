using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Node.Net.Collections
{
    // Filtering dictionary values using LINQ: http://stackoverflow.com/questions/2131648/filtering-out-values-from-a-c-sharp-generic-dictionary
    [TestFixture]
    public class IDictionaryExtensionTest : Fixture
    {
        #region CollectKeys
        [Test]
        [TestCase("Translations.json", "Type" ,true)]
        [TestCase("States.json", "Colorado" ,true)]
        [TestCase("States.json", "Jefferson",true)]
        [TestCase("States.json", "Softball", false)]
        public void IDictionaryExtension_CollectKeys_Value(string name, string key_value, bool contains_value)
        {
            var keys = new List<string>(IDictionaryExtension.CollectKeys(Read(name) as IDictionary));
            Assert.AreEqual(contains_value, keys.Contains(key_value));
        }
        #endregion

        #region Collect
        [Test]
        [TestCase("Translations.json", "Type", 3)]
        [TestCase("info.json", "path", 2)]
        [TestCase("States.json", "Name", 3155)]
        public void IDictionaryExtension_Collect_StringValues(string name, string key_value, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var valueInclude = Filters.GetStringFilter(key_value);
            //var filter = new Filters.StringFilter { Value = key_value };
           // var collector = new Collector { KeyFilter = filter.Include };
            var keys = IDictionaryExtension.Collect<string>(dictionary, null, valueInclude);
            Assert.AreEqual(count, keys.Count);
        }
        [Test]
        [TestCase("States.json", "State", false, 50)]
        [TestCase("States.json", "County", false, 0)]
        [TestCase("States.json", "County", true, 3105)]
        public void IDictionaryExtension_Collect_IDictionary(string name, string type_name, bool deep, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var valueInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", type_name);
            /*
            var filter = new Filters.IDictionaryFilter<IDictionary>
            {
                Key = "Type",
                KeyStringValue = type_name
            };*/
            //var collector = new Collector { ValueFilter = filter.Include };
            var keys = IDictionaryExtension.Collect<IDictionary>(dictionary, valueInclude, null, null, deep);
            Assert.AreEqual(count, keys.Count);
        }

       
        [Test]
        public void IDictionaryExtension_Collect_DeepFilter()
        {
            var dictionary = Read("Translations.json") as IDictionary;
            var valueInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", "Bar");
            /*
            var valueFilter = new Filters.IDictionaryFilter<IDictionary>
            {
                Key = "Type",
                KeyStringValue = "Bar"
            };
            */
            var deepInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", "Foo");
            /*
            var deepFilter = new Filters.IDictionaryFilter<IDictionary>
            {
                Key = "Type",
                KeyStringValue = "Foo"
            };*/
            var instances = IDictionaryExtension.Collect<IDictionary>(dictionary,valueInclude, null, deepInclude);
            Assert.AreEqual(1, instances.Count);
            deepInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", "X");
            /*
            deepFilter = new Filters.IDictionaryFilter<IDictionary>
            {
                Key = "Type",
                KeyStringValue = "X"
            };*/
            instances = IDictionaryExtension.Collect<IDictionary>(dictionary, valueInclude, null, deepInclude);
            Assert.AreEqual(0, instances.Count);
        }
        #endregion

        #region CollectValues
        [Test]
        [TestCase("Translations.json", "Type", 3)]
        [TestCase("Translations.json", "Height", 1)]
        [TestCase("Translations.json", "Z", 3)]
        public void IDictionaryExtension_CollectValues(string name, string key, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var values = IDictionaryExtension.CollectValues<string>(dictionary, key);
            Assert.AreEqual(count, values.Length);
        }
        #endregion

        #region CollectTypes
        [Test]
        [TestCase("Translations.json", 3)]
        [TestCase("info.json", 3)]
        [TestCase("States.json", 2)]
        public void IDictionaryExtension_CollectTypes(string name, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var types = IDictionaryExtension.CollectTypes(dictionary);
            Assert.AreEqual(count, types.Length);
        }
        #endregion

        [Test]
        [TestCase("ExitCode",null)]
        [TestCase("ExitCode", 1)]
        [TestCase("Type", "Widget")]
        [TestCase("foo0/Type", "Foo")]
        [TestCase("foo0/bar0/Type", "Bar")]
        public void IDictionaryExtension_SetGet(string key,object value)
        {
            var d = new Dictionary<string, dynamic>();
            Assert.IsNull(IDictionaryExtension.Get<object>(d,key));

            IDictionaryExtension.Set(d, key,value);
            Assert.AreEqual(value, IDictionaryExtension.Get<object>(d, key));

            
        }

        [Test]
        public void IDictionaryExtension_Get_NonExistant_Value()
        {
            var d = new Dictionary<string, dynamic>();
            Assert.AreEqual("", IDictionaryExtension.Get<string>(d, "NonExistant"));
        }

        [Test]
        [TestCase("Translations.json")]
        public void IDictionaryExtension_DeepCollect(string name)
        {
            var dictionary = Read(name) as IDictionary;
            Assert.NotNull(dictionary);
        }

        [Test]
        public void IDictionaryExtension_DeepUpdateParents_Recursive_Tree()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Type"] = "Root";
            var foo = new Dictionary<string, dynamic>();
            foo["Type"] = "Foo";
            var bar = new Dictionary<string, dynamic>();
            bar["Type"] = "Bar";
            dictionary["foo"] = foo;
            foo["bar"] = bar;
            IDictionaryExtension.DeepUpdateParents(dictionary);// .DeepCount<IDictionary>(dictionary);
            //Assert.AreEqual(2, count);
            bar["root"] = dictionary;
            IDictionaryExtension.DeepUpdateParents(dictionary);
            //count = IDictionaryExtension.DeepCount<IDictionary>(dictionary);
            //Assert.AreEqual(2, count);
        }
        [Test, Explicit]
        public void IDictionaryExtension_DeepCollect_Stress()
        {
            var dictionary = IDictionaryTest.GetLargeDictionary(4);
            var children = IDictionaryExtension.Collect<IDictionary>(dictionary);
            Assert.AreEqual(30940, children.Count);
        }
        [Test, Explicit]
        public void IDictionaryExtension_DeepCollect_Stress2()
        {
            var dictionary = IDictionaryTest.GetLargeDictionary(3);
            var children = IDictionaryExtension.Collect<IDictionary>(dictionary);
            Assert.AreEqual(2379, children.Count);

            for (int i = 0; i < 100; i++)
            {
                var c2 = IDictionaryExtension.Collect<IDictionary>(dictionary);
                Assert.AreEqual(2379, c2.Count);
            }
        }
        [Test, Explicit]
        public void IDictionaryExtension_DeepCollect_Stress3()
        {
            var dictionary = IDictionaryTest.GetLargeDictionary(3);
            var children = IDictionaryExtension.Collect<IDictionary>(dictionary);
            Assert.AreEqual(2379, children.Count);

            for (int i = 0; i < 100; i++)
            {
                var c2 = IDictionaryExtension.Collect<IDictionary>(children);
                Assert.AreEqual(2379, c2.Count);
            }
        }




        [Test]
        public void IDictionaryExtension_Copy()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["double"] = 1.23;
            var child = new Dictionary<string, dynamic>();
            child["name"] = nameof(child);
            dictionary[nameof(child)] = child;
            var copy = new Dictionary<string, dynamic>();
            IDictionaryExtension.Copy(copy, dictionary);
            Assert.True(copy.ContainsKey("double"));
            Assert.True(copy.ContainsKey(nameof(child)));
        }



        [Test]
        public void IDictionaryExtension_RemoveKeys()
        {
            var a = new Dictionary<string, dynamic>();
            a["name"] = nameof(a);
            var b = new Dictionary<string, dynamic>();
            b["name"] = nameof(b);
            var c = new Dictionary<string, dynamic>();
            c["name"] = nameof(c);
            a[nameof(b)] = b;
            b[nameof(c)] = c;
            var results = IDictionaryExtension.Collect<IDictionary>(a);

            IDictionaryExtension.RemoveKeys(a, results.Keys.ToArray<string>());
        }

        class Child2 : Dictionary<string, dynamic> { public object Parent { get; set; } }
        [Test]
        public void IDictionaryExtension_Remove()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["child"] = new Child2();
            dictionary["d"] = new Dictionary<string, dynamic>();
            IDictionaryExtension.DeepUpdateParents(dictionary);
            //var children = IDictionaryExtension.Collect<Child2>(dictionary);
            //Assert.AreEqual(1, children.Count);
            Assert.AreSame(dictionary, ObjectExtension.GetParent(dictionary["child"] as IDictionary));

            Assert.True(dictionary.ContainsKey("child"));
            Assert.True(dictionary.ContainsKey("d"));

            IDictionaryExtension.Remove<Child2>(dictionary);
            Assert.False(dictionary.ContainsKey("child"));
            Assert.True(dictionary.ContainsKey("d"));
        }

        [Test]
        public void IDictionaryExtension_DeepRemove()
        {
            var dictionary = new Dictionary<string, dynamic>();
            var subDictionary = new Dictionary<string, dynamic>();
            var child = new Child2();
            dictionary[nameof(subDictionary)] = subDictionary;
            subDictionary[nameof(child)] = child;

            Assert.True(subDictionary.ContainsKey(nameof(child)));
            IDictionaryExtension.DeepRemove<Child2>(dictionary);
            Assert.False(subDictionary.ContainsKey(nameof(child)));

        }

        [Test]
        public void IDictionaryExtension_Find()
        {
            var dictionary = new Dictionary<string, dynamic>();
            var subDictionary = new Dictionary<string, dynamic>();
            var childA = new Child2();
            var childB = new Child2();
            subDictionary[nameof(childA)] = childA;
            subDictionary[nameof(childB)] = childB;
            dictionary[nameof(subDictionary)] = subDictionary;


            var result = IDictionaryExtension.Find<Child2>(subDictionary, nameof(childB));
            Assert.AreSame(childB, result);

            result = IDictionaryExtension.Find<Child2>(dictionary, nameof(childB));
            Assert.AreSame(childB, result);
        }

        [Test]
        public void IDictionaryExtension_DeepCollectKeys()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["A"] = "a";

            var b = new Dictionary<string, dynamic>();
            b["C"] = "b";
            dictionary["B"] = b;

            var keys = IDictionaryExtension.CollectKeys(dictionary);
            Assert.True(keys.Contains("A"), "key A not found");
            Assert.True(keys.Contains("B"), "key B not found");
            Assert.True(keys.Contains("C"), "key C not found");

        }

        [Test]
        public void IDictionaryExtension_CollectAncestorUniqueValues()
        {
            var data = new Dictionary<string, dynamic>();
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo0/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo1/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo2/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Bars/bar1/Type", "Bar");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Widgets/widget0/Type", "Widget");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Widgets/widget1/Type", "Widget");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo0/widget0/Type", "Widget");
            var fw0 = Node.Net.Collections.IDictionaryExtension.Get<IDictionary>(data, "Foos/foo0/widget0");// as IDictionary;
            Assert.NotNull(fw0);
            //var 
        }

        [Test]
        public void IDictionaryExtension_GetItemsSource()
        {
            var data = new Dictionary<string, dynamic>();
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo0/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo1/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo2/Type", "Foo");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Bars/bar1/Type", "Bar");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Widgets/widget0/Type", "Widget");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Widgets/widget1/Type", "Widget");
            Node.Net.Collections.IDictionaryExtension.Set(data, "Foos/foo0/widget0/Type", "Widget");

            var itemsSource = new List<object>(data.GetItemsSource());
            Assert.AreEqual(3, itemsSource.Count);
        }
    }
}
