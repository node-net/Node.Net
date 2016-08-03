using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public interface IChild { IParent Parent { get; set; } }
    public interface IParent { Dictionary<string, IChild> GetChildren(); }
}
