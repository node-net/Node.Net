namespace Node.Net.Model
{
    public class Element : Parent, IChild
    {
        public IParent Parent { get; set; }
    }
}
