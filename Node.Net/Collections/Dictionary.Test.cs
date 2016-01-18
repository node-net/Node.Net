using NUnit.Framework;

namespace Node.Net.Collections
{
    [TestFixture,Category("Node.Net.Collections.Dictionary")]
    class DictionaryTest
    {
        [TestCase]
        public void Dictionary_Sample()
        {
            Dictionary dictionary = new Dictionary();
            dictionary["Table A"] = new Dictionary();
            dictionary["Table A"]["Type"] = "Table";
        }
    }
}
