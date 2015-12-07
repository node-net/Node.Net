using NUnit.Framework;
using System;
using System.Reflection;
using System.IO;
using System.ComponentModel;

namespace Node.Net.Json
{
    [TestFixture, NUnit.Framework.Category("Node.Net.Json.Hash")]
    public class Hash_Test
    {
        [TestCase]
        public void Hash_Usage()
        {
            Hash hash = new Hash();
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

            Hash hash2 = new Hash();
            Hash.Copy(hash, hash2);
            Assert.True(hash.Equals(hash2));
            Assert.AreEqual("ABC", hash2["string"] as string);
            Assert.AreEqual(false, (bool)hash2["false"]);
            Assert.AreEqual(true, (bool)hash2["true"]);
            NUnit.Framework.Assert.AreEqual(null, hash2["null"]);
            NUnit.Framework.Assert.IsNull(hash2["null"]);
            Assert.AreEqual(1.23, (double)hash2["double"]);

            string json = Hash.ToJson(hash);
            Assert.True(json.Contains("ABC"));

            Hash hash3 = new Hash(json);
            Assert.AreEqual("ABC", hash3["string"] as string);
            Assert.AreEqual(false, (bool)hash3["false"]);
            Assert.AreEqual(true, (bool)hash3["true"]);
            NUnit.Framework.Assert.AreEqual(null, hash3["null"]);
            NUnit.Framework.Assert.IsNull(hash3["null"]);
            Assert.AreEqual(1.23, (double)hash3["double"]);

            Assert.AreEqual(hash.GetHashCode(), hash3.GetHashCode());
            Assert.AreEqual(0, hash.CompareTo(hash3));
            Assert.True(hash.Equals(hash3));

            
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                hash3.Save(memory);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                Hash hash4 = new Hash();
                hash4.Open(memory);
                Assert.True(hash4.Equals(hash3), "hash4 and hash3 are not equal");
            }
        }

        [TestCase]
        public void Hash_Stream_Constructor()
        {
            Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Reader_Test));
            Hash hash = new Hash(assembly.GetManifestResourceStream("Node.Net.Json.Hash_Test.Sample.json"));
            NUnit.Framework.Assert.AreEqual("Sample", hash["Name"].ToString());
        }
        [TestCase]
        public void Hash_Save_Open()
        {
            Hash hash = new Hash("{'null':null,'string':'abc','false':false}");
            Assert.AreEqual(3, hash.Keys.Count);

            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\tmp\Hash_Test\SaveLoad.json";
            if(File.Exists(filename)) File.Delete(filename);
            hash.Save(filename);
            Hash hash2 = new Hash();
            hash2.Open(filename);
            File.Delete(filename);
            Assert.True(hash.Equals(hash2));
        }

        [TestCase]
        public void Hash_GetChildren()
        {
            Hash hash = new Hash("{'null':null,'string':'abc','false':false}");
            System.Collections.IList children = Hash.GetChildren(hash);
            Assert.AreEqual(0, children.Count);

            Hash hash2 = new Hash();
            hash2["childA"] = hash;
            children = Hash.GetChildren(hash2);
            Assert.AreEqual(1, children.Count);
        }

        [TestCase]
        public void Hash_Write_Read()
        {
            /*
            Hash hash = new Hash(@"{'path':'A:\tmp\'}");
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\tmp\Hash_Test\WriteRead.json";
            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
            hash.Save(filename);
            Hash hash2 = new Hash();
            hash2.Open(filename);
           NUnit.Framework.Assert.AreEqual("A:\\tmp\\", hash2["path"].ToString());
            System.IO.File.Delete(filename);*/
        }

        [TestCase]
        public void Hash_GetProperties()
        {
            Hash hash = new Hash();
            Assert.AreEqual(0, Hash.GetProperties(hash, null).Count);

            hash["string"] = "abc";
            Assert.AreEqual(1, Hash.GetProperties(hash, null).Count);
            PropertyDescriptorCollection pdc = Hash.GetProperties(hash, null);
            PropertyDescriptor pd = pdc[0];
            Assert.AreEqual("string", pd.Name);
            Assert.AreEqual("abc", pd.GetValue(hash).ToString());
            Assert.AreSame(typeof(string), pd.PropertyType);
            pd.SetValue(hash, "def");
            Assert.AreEqual("def", hash["string"].ToString());

            hash["double"] = 1.23;
            Assert.AreEqual(2, Hash.GetProperties(hash, null).Count);

            hash["bool"] = false;
            Assert.AreEqual(3, Hash.GetProperties(hash, null).Count);
        }

        [TestCase]
        public void Hash_Copy()
        {
            Hash src = new Hash("{'null':null,'string':'abc','double':1.23,'child':{'name':'child'},'array':[0,1]}");
            Assert.AreEqual(5, src.Count);

            Hash cpy = new Hash();
            Hash.Copy(src, cpy);
            Assert.True(src.Equals(cpy));
            Assert.AreNotSame(src["child"], cpy["child"]);
            Assert.AreNotSame(src["array"], cpy["array"]);
        }

        [TestCase]
        public void Hash_Bytes()
        {
            Hash a = new Hash();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("ABC");
            a["Sample.bin"] = bytes;

            string json = a.ToJson();
            Hash b = new Hash(json);
            byte[] bytes2 = (byte[])b["Sample.bin"];
            string text = System.Text.Encoding.UTF8.GetString(bytes2);
            Assert.AreEqual("ABC", text);
        }

        class Foo : Hash { }
        [TestCase]
        public void Hash_Convert()
        {
            Hash hash = new Hash("{'foo':{'Type':'Foo'}}");
            NUnit.Framework.Assert.AreSame(hash["foo"].GetType(), typeof(Hash));

            Hash conversions = new Hash();
            conversions["Foo"] = typeof(Foo);
            Hash hash2 = Hash.Convert(hash, conversions) as Hash;
            NUnit.Framework.Assert.AreSame(hash2["foo"].GetType(), typeof(Foo));
        }

        [TestCase]
        public void Hash_Sort()
        {
            Hash hash = new Hash("{'a':{'order':'Z'},'b':{'order':'Y'}}");
            //Hash sorted = Hash.Sort(hash, "order");
            //NUnit.Framework.Assert.AreEqual(2, sorted.Count);
            //NUnit.Framework.Assert.AreEqual('b', sorted[0]);
            //NUnit.Framework.Assert.AreEqual('a', sorted.Keys[1]);
        }

        [TestCase]
        public void Hash_Parse()
        {
            string[] args = { "filename=Z:/wrk/test.json", "enabled=true" };
            Hash hash = Hash.Parse(args);
            Assert.AreEqual("Z:/wrk/test.json", hash["filename"]);
            Assert.AreEqual("true", hash["enabled"]);
        }
    }
}
