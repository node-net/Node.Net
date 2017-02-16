using NUnit.Framework;
using System.IO;
namespace Node.Net.Json
{
    [TestFixture,Category("Json"),Category("Array")]
    class Array_Test
    {
        [TestCase]
        public void Array_String_Values()
        {

            string[] values = { "\\","\"a", "\"", "a",  };// "\\",
            foreach(string value in values)
            {
                Array array = new Array();
                array.Add(value);
                string json = Writer.ToString(array);
                Array array2 = new Array(json);
                Assert.AreEqual(1, array2.Count);
                Assert.AreEqual(value, array2[0].ToString());

                using(MemoryStream memory = new MemoryStream())
                {
                    array2.Save(memory);
                    memory.Seek(0, SeekOrigin.Begin);
                    Array array3 = new Array();
                    array3.Open(memory);
                    Assert.True(array3.Equals(array2), "array3 and array2 are not equal");
                }
            }
             
        }
    }
}
