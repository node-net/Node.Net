using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Transformers
{
    public abstract class TypeTransformer : ITypeTransformer
    {
        private ITransformer _itransform;
        public ITransformer Transformer
        {
            get { return _itransform; }
            set { _itransform = value; }
        }

        public abstract object Transform(object item);
    }
}
