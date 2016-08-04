using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Transformers
{
    public abstract class TypeTransformer : ITypeTransformer
    {
        public ITransformer Transformer { get; set; }

        public abstract object Transform(object item);
    }
}
