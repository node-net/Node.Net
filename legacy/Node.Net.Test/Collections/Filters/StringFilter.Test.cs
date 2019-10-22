using NUnit.Framework;

namespace Node.Net.Collections.Filters
{
    [TestFixture]
    class StringFilterTest
    {
        [Test]
        [TestCase("Type", "x", false)]
        [TestCase("Type", "T", true)]
        public void StringFilter_Pattern(string test_value, string filter_pattern, object expected_result)
        {
            var includeFunction = Filters.GetStringFilter(null, filter_pattern);
            Assert.AreEqual(
                expected_result, includeFunction(test_value));
            /*
                IncludeFunctions.GetStringFilter(null,filter_pattern).Inc
                new StringFilter
                {
                    Pattern = filter_pattern
                }.Include(test_value));*/
        }
    }
}
