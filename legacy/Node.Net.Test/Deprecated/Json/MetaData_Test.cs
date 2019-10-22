using NUnit.Framework;
using static System.Convert;
namespace Node.Net.Json
{
    [TestFixture,Category("Node.Net.Json.MetaData")]
    class MetaData_Test
    {
        [TestCase]
        public void MetaData_Usage()
        {
            Hash h1 = new Hash("{'Name':'h1'}");
            MetaData.Default[h1] = new Hash();
            MetaData.Default[h1]["Rank"] = 5;
            Assert.AreEqual(5, ToInt32(MetaData.Default[h1]["Rank"]));
        }
    }
}
