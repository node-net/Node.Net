using System.Collections.Generic;
namespace Node.Net.Deprecated
{
    public interface IParent
    {
        Dictionary<string, IChild> GetChildren();
    }
}
