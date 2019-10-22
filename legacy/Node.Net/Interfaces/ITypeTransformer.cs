namespace Node.Net
{
    public interface ITypeTransformer
    {
        ITransformer Transformer { get; set; }
        object Transform(object item);
    }
}
