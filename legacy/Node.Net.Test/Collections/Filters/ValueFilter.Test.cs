using NUnit.Framework;

namespace Node.Net.Collections.Filters
{
    [TestFixture]
    public class ValueFilterTest
    {
        [Test]
        [TestCase(12, 1, false)]
        [TestCase(12, 12, true)]
        public void ValueFilter_Int(int test_value, int filter_value, object expected_result)
        {
            var valueInclude = Filters.GetValueFilter<int>(filter_value);
            Assert.AreEqual(expected_result, valueInclude(test_value));
            /*
                expected_result,
                new ValueFilter<int>
                {
                    Value = filter_value
                }.Include(test_value));*/
        }
        [Test]
        [TestCase("X", "Type", false)]
        [TestCase("Type", "Type", true)]
        public void ValueFilter_String(string test_value, string filter_value, object expected_result)
        {
            var valueInclude = Filters.GetValueFilter<string>(filter_value);
            Assert.AreEqual(expected_result, valueInclude(test_value));
            /*
                expected_result,
                new ValueFilter<string>
                {
                    Value = filter_value
                }.Include(test_value));*/
        }
    }
}
