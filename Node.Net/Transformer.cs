using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace Node.Net
{
    public class Transformer : ITransformer
    {
        private Dictionary<string, dynamic> _resources = new Dictionary<string, dynamic>();
        public IDictionary Resources { get { return _resources; } }

        private Dictionary<Type, ITypeTransformer> _typeTransformers;
        public IDictionary TypeTransformers
        {
            get
            {
                if (object.ReferenceEquals(null, _typeTransformers))
                {
                    _typeTransformers = new Dictionary<Type, ITypeTransformer>();
                    _typeTransformers.Add(typeof(Color), new Transformers.ColorTransformer { Transformer = this });
                    _typeTransformers.Add(typeof(Brush), new Transformers.BrushTransformer { Transformer = this });
                }
                return _typeTransformers;
            }
        }

        public object Transform(object item, Type type)
        {
            return null;
        }
    }
}
