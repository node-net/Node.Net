using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Deprecated.Extensions
{
    [TestFixture, Category(nameof(IDictionaryExtension))]
    class IDictionaryExtensionTest
    {
        [Test]
        public void IDictionary_Find()
        {
            var colors = new Dictionary<string, dynamic>();
            colors.Set("blue/Type", "Color");
            colors.Set("red/Type", "Color");
            colors.Set("green/Type", "Color");

            var keys = new List<string>(colors.Find(new Filters.TypeFilter("Color")));
            Assert.AreEqual(3, keys.Count);
            Assert.True(keys.Contains("blue"));

            var result = colors.Collect(new Filters.TypeFilter("Color"));
            Assert.True(result.Contains("red"));
            var green = result["green"] as IDictionary;
            Assert.NotNull(green);
        }
    }
}
