using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Model3D
{
    public class Transform3DHelper
    {
        private Collections.MetaData _metaData = new Collections.MetaData();
        public void Clear() { _metaData.Clear(); }
        public void Traverse(object value)
        {
            Traverse(value as IDictionary);
        }

        private void Traverse(IDictionary dictionary)
        {
            if(!object.ReferenceEquals(null,dictionary))
            {

            }
        }
    }
}
