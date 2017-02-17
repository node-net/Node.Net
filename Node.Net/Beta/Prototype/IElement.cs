namespace Node.Net.Factories.Prototype
{
    public interface IElement : IReadOnlyElement
    {
        void Clear();
        void Set(string name, dynamic value);
    }
}
