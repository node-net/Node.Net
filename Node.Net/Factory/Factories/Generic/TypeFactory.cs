using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factory.Factories.Generic
{
    public class TypeFactory<T> 
    {
        public Type TargetType { get { return typeof(T); } }
    }
}
