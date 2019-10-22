using NUnit.Framework;

namespace Node.Net.Deprecated.Collections
{
    [TestFixture,Category(nameof(Collections))]
    class DictionaryTest
    {
        [TestCase]
        public void Dictionary_Sample()
        {
            var dictionary = new Deprecated.Collections.Dictionary();
            dictionary["Table A"] = new Dictionary();
            dictionary["Table A"]["Type"] = "Table";
        }
    }
}
