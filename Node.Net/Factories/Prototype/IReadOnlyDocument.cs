namespace Node.Net.Factories.Prototype
{
    public interface IReadOnlyDocument : IReadOnlyElement
    {
        string FileName { get; }
    }
}
