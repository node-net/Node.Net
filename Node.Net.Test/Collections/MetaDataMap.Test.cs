using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Node.Net.Collections.Test
{
    [TestFixture]
    class MetaDataMapTest
    {
        [Test]
        public void MetaDataMap_Usage()
        {
            var map = new Node.Net.Collections.MetaDataMap();

            //var date = DateTime.Now;    // value type
            //Assert.IsNotNull(map.GetMetaData(date));
            //var date_MetaData = map.GetMetaData(date);

            //Assert.AreSame(date_MetaData, map.GetMetaData(date));


            var dictionary = new Dictionary<string, dynamic>(); // reference type
            Assert.IsNotNull(map.GetMetaData(dictionary));
            var dictionary_MetaData = map.GetMetaData(dictionary);
            Assert.AreSame(dictionary_MetaData, map.GetMetaData(dictionary)); ;
        }
    }
}
