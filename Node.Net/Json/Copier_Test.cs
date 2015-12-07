using NUnit.Framework;

namespace Node.Net.Json.Test
{
    [TestFixture,Category("Node.Net.Json.Copier")]
    class Copier_Test
    {
        [TestCase]
        public void Copier_Hash_Usage()
        {
            Hash hash = new Hash("{'a':'A','b':'B'}");
            Hash hash2 = new Hash();
            Copier.Copy(hash, hash2);
            Assert.True(hash.Equals(hash2));
        }

        [TestCase]
        public void Copier_Array_Usage()
        {
            Array array = new Array("['a','b',3]");
            Array array2 = new Array();
            Copier.Copy(array, array2);
            Assert.AreEqual(array.Count, array2.Count, "array counts do not match");
            Assert.True(array.Equals(array2), "arrays are not equivalent");
        }

        class ValueTypeFilter : IFilter
        {
            public bool Include(object value)
            {
                if (KeyValuePair.IsKeyValuePair(value))
                {
                    if (!Include(KeyValuePair.GetKey(value))) return false;
                    if (!Include(KeyValuePair.GetValue(value))) return false;
                }
                else
                {
                    if (!object.ReferenceEquals(null, value))
                    {
                        if (value.GetType().IsValueType) return false;
                    }
                }
                return true;
            }
        }
        [TestCase]
        public void Copier_Array_Filter_Usage()
        {
            Array array = new Array("['a','b',3]");
            Array array2 = new Array();
            Copier.Copy(array, array2, new ValueTypeFilter());
            Assert.AreNotEqual(array.Count, array2.Count, "array counts match");
            Assert.AreEqual(2, array2.Count, "array2.Count is not as expected");
        }

        [TestCase]
        public void Copier_Hash_Filter_Usage()
        {
            Hash hash = new Hash("{'a':'A','b':'B','c':3}");
            Hash hash2 = new Hash();
            Copier.Copy(hash, hash2,new ValueTypeFilter());
            Assert.AreNotEqual(hash.Count, hash2.Count, "hash and hash2 counts should be different");
            Assert.AreEqual(2, hash2.Count, "hash2.Count is not as expected");
        }
    }
}
