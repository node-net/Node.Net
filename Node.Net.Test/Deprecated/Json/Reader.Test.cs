using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Node.Net.Data;
namespace Node.Net.Deprecated.Json
{
    [TestFixture,Category("Node.Net.Deprecated.Json.Reader")]
    class Reader_Test
    {
        [TestCase]
        public void Reader_Usage()
        {
            var reader = new Data.Readers.Reader();
            /*
            var reader = new Reader
            {
                DefaultDictionaryType = typeof(Dictionary<string, object>)
            };// typeof(Dictionary<string, object>));
            */
            var dictionary
                = (Dictionary<string, object>)reader.Read("{'string':'ABC','false':false,'true':true,'null':null,'double':1.23}");
            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual("ABC", dictionary["string"] as string);

            //reader = new Reader();
            var hash = reader.Read("{'child':{'chash':{}}}") as IDictionary;
            Assert.NotNull(hash);
            Assert.AreEqual(1, hash.Count);
            var child = hash["child"] as IDictionary;
            Assert.NotNull(child);
            var chash = child["chash"] as IDictionary;
            Assert.NotNull(chash);
        }

        [TestCase]
        public void Reader_Types()
        {
            /*
            var reader = new Reader();
            Assert.AreEqual(0, reader.Types.Count);

            reader = new Reader(System.Reflection.Assembly.GetAssembly(typeof(Reader)));
            Assert.AreSame(typeof(Reader),reader.Types[nameof(Reader)]);

            System.Reflection.Assembly[] assemblies
                = { System.Reflection.Assembly.GetAssembly(typeof(Collections.Hash))};
            reader = new Reader(assemblies);
            Assert.AreSame(typeof(Collections.Hash), reader.Types["Hash"]);
            */
        }

        [TestCase]
        public void Reader_OneLineHashRead()
        {
            var reader = new Reader();
            var hash = (IDictionary)reader.Read("{'string':'ABC'}");
        }

        [TestCase]
        public void Reader_CurlyBraceInQuotes_Hash()
        {
            var reader = new Reader();
            var hash = (IDictionary)reader.Read("{'{a}':'A'}");
            Assert.AreEqual(hash["{a}"], "A");
        }
        [TestCase]
        public void Reader_CurlyBraceInQuotes_Array()
        {
            var reader = new Reader();
            var arr = new Deprecated.Collections.Array();
            reader.Read("[\"{a}\"]",arr);
            Assert.AreEqual("{a}",arr[0].ToString());
        }

        [TestCase]
        public void Reader_Roundtrip_Strings_With_Quotes()
        {
            /*
            string testString = "";
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Reader_Test));
            for(int i = 0; i < 2;++i)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(assembly.GetManifestResourceStream("Node.Net.Resources.History.txt")))
                {
                    testString = sr.ReadToEnd();
                }

                Hash hash = new Hash();
                hash["TestString"] = testString;
                string json = Writer.ToString(hash);

                Hash hash2 = new Hash();
                Reader reader = new Reader();
                reader.Read(json, hash2);
                NUnit.Framework.Assert.AreEqual(testString, hash2["TestString"]);
            }*/
        }
        [TestCase]
        public void Reader_History()
        {
            /*
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Reader_Test));

            string output = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(assembly.GetManifestResourceStream("Node.Net.Resources.History.txt")))
            {
                output = sr.ReadToEnd();
            }
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                              + @"\History.Test.json";
            Array arr = new Array();
            Hash hash = new Hash();
            hash["Output"] = output;
            arr.Add(hash);
            Writer writer = new Writer();
            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Create))
            {
                writer.Write(fs, arr);
            }


            Reader reader = new Reader();
            arr = new Array();
            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Open))
            {
                reader.Read(fs, arr);
            }
            //reader.Read(assembly.GetManifestResourceStream("Node.Net.Resources.History.json"), arr);
             */
        }

        class Foo : Deprecated.Collections.Hash
        {
            public Foo() { this["Type"] = nameof(Foo); }
        }
        class Bar : Deprecated.Collections.Hash
        {
            public Bar() { this["Type"] = nameof(Bar); }
        }
        [TestCase]
        public void Reader_CustomTypes()
        {
            var reader = new Reader();
            reader.Types[nameof(Foo)] = typeof(Foo);
            reader.Types[nameof(Bar)] = typeof(Bar);
            var hash = (IDictionary)reader.Read("{'foo':{'Type':'Foo','bar':{'Type':'Bar'}}}");
            Assert.NotNull(hash);
            Assert.True(hash.Contains("foo"),"hash does not contain key 'Foo'");
            var foo = hash["foo"] as Foo;
            Assert.NotNull(foo,"hash['foo'] did not cast to type Foo");
            Assert.True(foo.ContainsKey("bar"),"foo did not contain key 'bar'");
            var bar = foo["bar"] as Bar;
            Assert.NotNull(bar, "foo['bar'] did not cast to type Bar");

            hash = (IDictionary)reader.Read("{'foo':{'Type':'Foo','history':[{'Type':'Bar'}]}}");
            Assert.NotNull(hash);
            Assert.True(hash.Contains(nameof(foo)), "hash does not contain key 'Foo'");
            foo = hash[nameof(foo)] as Foo;
            Assert.NotNull(foo, "hash['foo'] did not cast to type Foo");
            Assert.True(foo.ContainsKey("history"), "foo does not contain key 'history'");
            //Array array = hash["array"] as Array;
            //Assert.NotNull(array, "hash['array'] did not cast to Array");
            NUnit.Framework.Assert.AreEqual(1, foo["history"].Count, "foo['history'].Count was not 1");
            NUnit.Framework.Assert.NotNull(foo["history"][0], "foo['history'][0] was null");
            bar = foo["history"][0] as Bar;
            Assert.NotNull(bar, "foo['history'][0] did not cast to Bar");
        }
    }
}
