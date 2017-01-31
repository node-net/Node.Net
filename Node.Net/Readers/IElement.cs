using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    public interface IElement
    {
        bool Contains(string name);
        dynamic Get(string name);
        void Set(string name, dynamic value);
    }
}
