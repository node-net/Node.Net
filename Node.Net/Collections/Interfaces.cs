using System.Collections.Generic;

namespace Node.Net.Collections
{
    public interface IFilter { bool? Include(object value); }
    //public interface IChild { IParent Parent { get; set; } }
    //public interface IParent { Dictionary<string, IChild> GetChildren(); }
}
