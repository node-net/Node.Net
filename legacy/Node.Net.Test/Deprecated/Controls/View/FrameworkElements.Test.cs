using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class FrameworkElements_Test
    {
        [TestCase]
        public void FrameworkElements_Usage()
        {
            var elements = new FrameworkElements
            {
                DataContext = null
            };
            Assert.IsNull(elements.DataContext);
        }

        [TestCase]
        public void FrameworkElements_GetAvailableTypes()
        {
            var types
                = new System.Collections.Generic.List<System.Type>(FrameworkElements.GetAvailableTypes());
            Assert.True(types.Count > 1);
        }
    }
}
