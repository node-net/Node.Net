using NUnit.Framework;
using System;
using System.Collections.Generic;
namespace Node.Net.Json
{
    [TestFixture,Category("Json"),Category("Reader")]
    class Reader_Test
    {
        [TestCase]
        public void Reader_Usage()
        {
            Reader reader = new Reader(typeof(Dictionary<string, object>));
            Dictionary<string, object> dictionary
                = (Dictionary<string, object>)reader.Read("{'string':'ABC','false':false,'true':true,'null':null,'double':1.23}");
            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual("ABC", dictionary["string"] as string);

            reader = new Reader();
            Hash hash = reader.Read("{'child':{'chash':{}}}") as Hash;
            Assert.NotNull(hash);
            Assert.AreEqual(1, hash.Count);
            Hash child = hash["child"] as Hash;
            Assert.NotNull(child);
            Hash chash = child["chash"] as Hash;
            Assert.NotNull(chash);
        }

        [TestCase]
        public void Reader_Types()
        {
            Reader reader = new Reader();
            Assert.AreEqual(0, reader.Types.Count);

            reader = new Reader(System.Reflection.Assembly.GetAssembly(typeof(Reader)));
            Assert.AreSame(typeof(Reader),reader.Types["Reader"]);

            System.Reflection.Assembly[] assemblies
                = { System.Reflection.Assembly.GetAssembly(typeof(Hash))};
            reader = new Reader(assemblies);
            Assert.AreSame(typeof(Hash), reader.Types["Hash"]);
        }

        [TestCase]
        public void Reader_OneLineHashRead()
        {
            Hash hash = Reader.ReadHash("{'string':'ABC'}");
        }

        [TestCase]
        public void Reader_CurlyBraceInQuotes_Hash()
        {
            Reader reader = new Reader();
            Hash hash = (Hash)reader.Read("{'{a}':'A'}");
            Assert.AreEqual(hash["{a}"], "A");
        }
        [TestCase]
        public void Reader_CurlyBraceInQuotes_Array()
        {
            Reader reader = new Reader();
            Array arr = new Array();
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

        class Foo : Hash
        {
            public Foo() { this["Type"] = "Foo"; }
        }
        class Bar : Hash
        {
            public Bar() { this["Type"] = "Bar"; }
        }
        [TestCase]
        public void Reader_CustomTypes()
        {
            Reader reader = new Reader();
            reader.Types["Foo"] = typeof(Foo);
            reader.Types["Bar"] = typeof(Bar);
            Hash hash = (Hash)reader.Read("{'foo':{'Type':'Foo','bar':{'Type':'Bar'}}}");
            Assert.NotNull(hash);
            Assert.True(hash.ContainsKey("foo"),"hash does not contain key 'Foo'");
            Foo foo = hash["foo"] as Foo;
            Assert.NotNull(foo,"hash['foo'] did not cast to type Foo");
            Assert.True(foo.ContainsKey("bar"),"foo did not contain key 'bar'");
            Bar bar = foo["bar"] as Bar;
            Assert.NotNull(bar, "foo['bar'] did not cast to type Bar");

            hash = (Hash)reader.Read("{'foo':{'Type':'Foo','history':[{'Type':'Bar'}]}}");
            Assert.NotNull(hash);
            Assert.True(hash.ContainsKey("foo"), "hash does not contain key 'Foo'");
            foo = hash["foo"] as Foo;
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
