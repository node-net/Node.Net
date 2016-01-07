using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace Node.Net.Json
{
    [TestFixture,Category("Node.Net.Json.HashBase")]
    class HashBase_Test
    {
        [TestCase]
        public void HashBase_Usage()
        {
            HashBase hash = new HashBase();
            hash["string"] = "ABC";
            Assert.AreEqual("ABC", hash["string"] as string);
            hash["false"] = false;
            Assert.AreEqual(false, (bool)hash["false"]);
            hash["true"] = true;
            Assert.AreEqual(true, (bool)hash["true"]);
            hash["null"] = null;
            NUnit.Framework.Assert.AreEqual(null, hash["null"]);
            NUnit.Framework.Assert.IsNull(hash["null"]);
            hash["double"] = 1.23;
            Assert.AreEqual(1.23, (double)hash["double"]);

            HashBase hash2 = new HashBase();
            HashBase.Copy(hash, hash2);
            Assert.True(hash.Equals(hash2));
            Assert.AreEqual("ABC", hash2["string"] as string);
            Assert.AreEqual(false, (bool)hash2["false"]);
            Assert.AreEqual(true, (bool)hash2["true"]);
            NUnit.Framework.Assert.AreEqual(null, hash2["null"]);
            NUnit.Framework.Assert.IsNull(hash2["null"]);
            Assert.AreEqual(1.23, (double)hash2["double"]);

            string json = HashBase.ToJson(hash);
            Assert.True(json.Contains("ABC"));

            HashBase hash3 = new HashBase(json);
            Assert.AreEqual("ABC", hash3["string"] as string);
            Assert.AreEqual(false, (bool)hash3["false"]);
            Assert.AreEqual(true, (bool)hash3["true"]);
            Assert.AreEqual(null, hash3["null"]);
            Assert.IsNull(hash3["null"]);
            Assert.AreEqual(1.23, (double)hash3["double"]);

            Assert.AreEqual(hash.GetHashCode(), hash3.GetHashCode());
            Assert.AreEqual(0, hash.CompareTo(hash3));
            Assert.True(hash.Equals(hash3));


            using (MemoryStream memory = new MemoryStream())
            {
                hash3.Save(memory);
                memory.Seek(0, SeekOrigin.Begin);
                HashBase hash4 = new HashBase();
                hash4.Open(memory);
                Assert.True(hash4.Equals(hash3), "hash4 and hash3 are not equal");
            }
        }

        [TestCase]
        public void HashBase_Stream_Constructor()
        {
            Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Reader_Test));
            //HashBase hash = new HashBase(assembly.GetManifestResourceStream("Node.Net.Node.Net.Json.Hash_Test.Sample.json"));
            HashBase hash = new HashBase(IO.StreamExtension.GetStream("Json.Hash_Test.Sample.json"));
            Assert.AreEqual("Sample", hash["Name"].ToString());
        }
        [TestCase]
        public void HashBase_Save_Open()
        {
            HashBase hash = new HashBase("{'null':null,'string':'abc','false':false}");
            Assert.AreEqual(3, hash.Keys.Count);

            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\tmp\Hash_Test\SaveLoad.json";
            if (File.Exists(filename)) File.Delete(filename);
            hash.Save(filename);
            HashBase hash2 = new HashBase();
            hash2.Open(filename);
            File.Delete(filename);
            Assert.True(hash.Equals(hash2));
        }

        [TestCase]
        public void HashBase_GetChildren()
        {
            HashBase hash = new HashBase("{'null':null,'string':'abc','false':false}");
            System.Collections.IList children = Hash.GetChildren(hash);
            Assert.AreEqual(0, children.Count);

            HashBase hash2 = new HashBase();
            hash2["childA"] = hash;
            children = Hash.GetChildren(hash2);
            Assert.AreEqual(1, children.Count);
        }

        [TestCase]
        public void HashBase_Copy()
        {
            HashBase src = new HashBase("{'null':null,'string':'abc','double':1.23,'child':{'name':'child'},'array':[0,1]}");
            Assert.AreEqual(5, src.Count);

            HashBase cpy = new HashBase();
            HashBase.Copy(src, cpy);
            Assert.True(src.Equals(cpy));
            Assert.AreNotSame(src["child"], cpy["child"]);
            Assert.AreNotSame(src["array"], cpy["array"]);
        }

        [TestCase]
        public void HashBase_Integer_Indexing()
        {
            HashBase hash = new HashBase();
            hash["a"] = "A";
            hash["b"] = "B";
            Assert.AreEqual("A", hash[0].ToString());
        }

        class Foo : HashBase { }
        [TestCase]
        public void HashBase_Convert()
        {
            HashBase hash = new HashBase("{'foo':{'Type':'Foo'}}");
            Assert.AreSame(hash["foo"].GetType(), typeof(Hash));

            HashBase conversions = new HashBase();
            conversions["Foo"] = typeof(Foo);
            HashBase hash2 = HashBase.Convert(hash, conversions) as HashBase;
            Assert.AreSame(hash2["foo"].GetType(), typeof(Foo));
        }

        [TestCase]
        public void HashBase_Sort()
        {
            Hash hash = new Hash("{'a':{'order':'Z'},'b':{'order':'Y'}}");
            //Hash sorted = Hash.Sort(hash, "order");
            //NUnit.Framework.Assert.AreEqual(2, sorted.Count);
            //NUnit.Framework.Assert.AreEqual('b', sorted[0]);
            //NUnit.Framework.Assert.AreEqual('a', sorted.Keys[1]);
        }
        
        [TestCase]
        public void HashBase_Count_By_Type()
        {
            HashBase hash = new HashBase();
            hash["Name"] = "test";
            hash["foo1"] = new Foo();
            hash["foo2"] = new Foo();
            Assert.AreEqual(3, hash.Count);
            Assert.AreEqual(2, hash.GetCount(typeof(Foo)));
            Assert.AreEqual(2, hash.GetCount<Foo>());

            Foo[] foos = hash.Collect<Foo>();
            Assert.AreEqual(2, foos.Length);

            string[] keys = hash.CollectKeys<Foo>();
            Assert.AreEqual(2, keys.Length);

            Foo foo2 = hash.Get<Foo>("foo2");
            NUnit.Framework.Assert.AreSame(foo2, hash["foo2"]);

            Foo foo2A = hash.Get<Foo>(1);
            Assert.AreSame(foo2,foo2A);
        }

        [TestCase]
        public void HashBase_ToArray()
        {
            HashBase hash = new HashBase();
            hash["Name"] = "test";
            hash["foo1"] = new Foo();
            hash["foo2"] = new Foo();
            Assert.AreEqual(3, hash.Count);
            Foo[] foos = hash.ToArray<Foo>();
            Assert.AreEqual(2, foos.Length);
        }

        [TestCase]
        public void HashBase_Key()
        {
            HashBase hash = new HashBase();
            Assert.AreEqual("", hash.Key);

            HashBase child = new HashBase();
            hash["Child"] = child;
        }

        [TestCase]
        public void HashBase_Collect()
        {
            HashBase hash = new HashBase();
            hash["foo1"] = new Foo();
            hash["foo2"] = new Foo();
            Assert.AreEqual(2, hash.Collect<Foo>().Length);
        }

        [TestCase]
        public void HashBase_DeepCollect()
        {
            HashBase hash = new HashBase();
            Foo foo1 = new Foo();
            hash["foo1"] = foo1;
            foo1["foo2"] = new Foo();
            Assert.AreEqual(2, hash.DeepCollect<Foo>().Length);
        }
    }
}
