namespace Node.Net.Documents
{
    public interface IDocument
    {
        void Open(System.IO.Stream stream);
        void Save(System.IO.Stream stream);
    }
}
