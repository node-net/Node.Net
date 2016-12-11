using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Repositories
{
    public interface IGet { object Get(string name); }
    public interface ISet { void Set(string name, object value); }
}
