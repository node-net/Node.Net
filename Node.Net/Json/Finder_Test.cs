using NUnit.Framework;
namespace Node.Net.Json
{
    [TestFixture,Category("Node.Net.Json.Finder")]
    class Finder_Test
    {
        [TestCase]
        public void Finder_Find_With_String()
        {
            Hash hash = new Hash();
            hash["abc"] = "ABC";
            hash["cde"] = "CDE";

            object[] keys = Finder.Find(hash, "fgh");
            Assert.AreEqual(0, keys.Length, "expected zero results for 'fgh'");
            keys = Finder.Find(hash, "cd");
            Assert.AreEqual(1, keys.Length, "expected one result for 'cd'");
            keys = Finder.Find(hash, "CD");
            Assert.AreEqual(1, keys.Length, "expected one result for 'CD'");

            object[] items = Finder.Collect(hash, "CD");
            Assert.AreEqual(1, items.Length, "expected one result for collect 'CD'");
            Assert.AreEqual(items[0], "CDE", "expected 'CDE' to match collect result[0]");

            Array array = new Array();
            int[] indices = Finder.Find(array, "cd");
            Assert.AreEqual(0, indices.Length, "expected zero result in array for 'cd'");
            array.Add(hash);
            indices = Finder.Find(array, "cd");
            Assert.AreEqual(1, indices.Length, "expected one result in array for 'cd'");

            items = Finder.Collect(array, "CD");
            Assert.AreEqual(1, items.Length, "expected one result for array collect 'CD'");
            Assert.AreSame(items[0], hash, "expected hash ref to match collect result[0]");
        }

        [TestCase]
        public void Finder_GetValue()
        {
            Hash hash = new Hash("{'a':{'b':{'c':3}}}");

            Hash hashA = (Hash)Finder.GetValue(hash,"a");
            Assert.NotNull(hashA, "hashA is null");

            Hash hashB = (Hash)Finder.GetValue(hash, "a/b");
            Assert.NotNull(hashB, "hashB is null");

            double value = (double)Finder.GetValue(hash,"a/b/c");
            Assert.AreEqual(3, value, "Finder.GetValue(\"a/b/c\") did not return correct value");
        }
    }
}
