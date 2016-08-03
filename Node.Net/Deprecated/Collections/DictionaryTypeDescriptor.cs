using System;
using System.ComponentModel;

namespace Node.Net.Collections
{
    public class DictionaryTypeDescriptor : CustomTypeDescriptor
    {
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return base.GetProperties(attributes);
        }
    }
}
