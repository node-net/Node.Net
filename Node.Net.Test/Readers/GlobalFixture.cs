using System.IO;

namespace Node.Net.Readers.Test
{
    class GlobalFixture
    {
        public static Stream GetStream(string name)
        {
            foreach (var manifestResourceName in typeof(GlobalFixture).Assembly.GetManifestResourceNames())
            {
                if (manifestResourceName == name || manifestResourceName.Contains(name))
                {
                    return typeof(GlobalFixture).Assembly.GetManifestResourceStream(manifestResourceName);
                }
            }
            return null;
        }
    }
}
