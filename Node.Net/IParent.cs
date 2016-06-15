using System.Collections.Generic;
namespace Node.Net
{
    public interface IParent
    {
        Dictionary<string,IChild> GetChildren();
    }
}
