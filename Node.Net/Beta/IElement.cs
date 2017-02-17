namespace Node.Net.Beta
{
    public interface IElement : IReadOnlyElement
    {
        void Clear();
        void Set(string name, dynamic value);
    }
}
