namespace Node.Net.Environment
{
    [NUnit.Framework.TestFixture,NUnit.Framework.Category("Node.Net.Environment.Resources")]
    class Resources_Test
    {
        [NUnit.Framework.TestCase]
        public void Resources_GetStream()
        {
            System.IO.Stream stream = Resources.GetStream("Node.Net.Node.Net.Environment.Environment_Test.Tree.json");
            NUnit.Framework.Assert.NotNull(stream);

            NUnit.Framework.Assert.Throws<System.ArgumentException>(()=>Resources_GetStream_InvalidName());
        }

        public void Resources_GetStream_InvalidName()
        {
            System.IO.Stream stream = Resources.GetStream("BadStream");
        }

        public void Resources_GetResourceCount()
        {
            // Given a DLL get the count of ManifestResourceNames
            //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(
            //    typeof(Node.Net.EnvirResources.Class));
        }

        public void Resources_Dll_To_Files()
        {
            // Given a Dll, copy all ManifestResourceStream datat
            // to files in MyDocuments
        }
    }
}
