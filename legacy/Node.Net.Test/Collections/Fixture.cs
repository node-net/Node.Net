using System.IO;

namespace Node.Net.Collections
{
    public class Fixture
    {
        public static object Read(string name) => Node.Net.Readers.JsonReader.Default.Read(GetStream(name));
        public static Stream GetStream(string name)
        {
            var assembly = typeof(Fixture).Assembly;
            foreach (string manifestResourceName in assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name))
                {
                    return assembly.GetManifestResourceStream(manifestResourceName);
                }
            }

            return null;
        }
    }
}
