using NUnit.Framework;
namespace Node.Net.Environment
{
    [TestFixture,NUnit.Framework.Category("Node.Net.Environment.Resources")]
    class Resources_Test
    {
        [TestCase]
        public void Resources_GetStream()
        {
            var stream = Resources.GetStream(typeof(Resources_Test).Assembly,"Environment.Test.Tree.json");
            Assert.NotNull(stream);

            //NUnit.Framework.Assert.Throws<System.ArgumentException>(()=>Resources_GetStream_InvalidName());
        }

        public static void Resources_GetStream_InvalidName()
        {
            var stream = Resources.GetStream("BadStream");
        }

        public static void Resources_GetResourceCount()
        {
            // Given a DLL get the count of ManifestResourceNames
        }

        public static void Resources_Dll_To_Files()
        {
            // Given a Dll, copy all ManifestResourceStream datat
            // to files in MyDocuments
        }
    }
}
