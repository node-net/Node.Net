namespace Node.Net.View
{
    public class NotePadDocument : System.Collections.Generic.List<string>
    {
        private System.IO.StreamReader streamReader;
        public void Open(System.IO.Stream stream)
        {
            Clear();
            streamReader = new System.IO.StreamReader(stream);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                Add(line);
            }
        }
        private System.IO.StreamWriter streamWriter;
        public void Save(System.IO.Stream stream)
        {
            streamWriter = new System.IO.StreamWriter(stream);
            foreach (string line in this)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Flush();
        }
    }
    class NotePadDocument_Test
    {
    }
}
