namespace Node.Net.Beta
{
    public interface IReadOnlyDocument : IReadOnlyElement
    {
        string FileName { get; }
    }
}
