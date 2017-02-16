namespace Node.Net.Documents
{
    public class TextDocument : System.Collections.Generic.List<string>,
                                IDocument,
                                System.IDisposable
    {
        private System.IO.StreamWriter streamWriter = null;
        private System.IO.StreamReader streamReader = null;
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (streamWriter != null) streamWriter.Dispose();
                if (streamReader != null) streamReader.Dispose();
            }
        }

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

        public void Save(System.IO.Stream stream)
        {
            streamWriter = new System.IO.StreamWriter(stream);
            foreach(string line in this)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Flush();
        }
    }
}
