using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class FrameworkElements_Test
    {
        [TestCase]
        public void FrameworkElements_Usage()
        {
            FrameworkElements elements = new FrameworkElements();
            elements.DataContext = null;
            NUnit.Framework.Assert.IsNull(elements.DataContext);
        }

        [TestCase]
        public void FrameworkElements_GetAvailableTypes()
        {
            System.Collections.Generic.List<System.Type> types 
                = new System.Collections.Generic.List<System.Type>(FrameworkElements.GetAvailableTypes());
            NUnit.Framework.Assert.True(types.Count > 1);
        }
    }
}
