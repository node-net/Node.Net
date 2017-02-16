using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls.Test.Forms
{
    public class IDictionaryPropertyAdapter : ICustomTypeDescriptor
    {
        private IDictionary _dictionary;
        public IDictionaryPropertyAdapter(IDictionary dictionary)
        {
            _dictionary = dictionary;
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<Attribute> alist = new List<Attribute>(attributes);
            alist.Add(new ReadOnlyAttribute(true));

            System.Collections.Generic.List<PropertyDescriptor> descriptors =
                new System.Collections.Generic.List<PropertyDescriptor>();
            foreach (string key in _dictionary.Keys)
            {
                object value = _dictionary[key];
                if (!object.ReferenceEquals(null, value))
                {
                    if (value.GetType() == typeof(string) ||
                        value.GetType() == typeof(double) ||
                        value.GetType() == typeof(long) ||
                        value.GetType() == typeof(bool))
                    {

                        ReadOnlyIDictionaryPropertyDescriptor pd
                            = new ReadOnlyIDictionaryPropertyDescriptor(_dictionary,key, alist.ToArray());
                        //pd.IDictionary = _dictionary;
                        descriptors.Add(pd);
                    }
                }

            }
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }
        public object GetPropertyOwner(PropertyDescriptor pd) { return this; }
        public string GetClassName() { return TypeDescriptor.GetClassName(this, true); }
        public AttributeCollection GetAttributes() { return TypeDescriptor.GetAttributes(this, true); }
        public PropertyDescriptorCollection GetProperties() { return GetProperties(null); }
        public EventDescriptorCollection GetEvents() { return TypeDescriptor.GetEvents(this, true); }
        public EventDescriptorCollection GetEvents(System.Attribute[] attributes) { return TypeDescriptor.GetEvents(this, attributes, true); }
        public object GetEditor(Type editorBaseType) { return null; }
        public PropertyDescriptor GetDefaultProperty() { return TypeDescriptor.GetDefaultProperty(this, true); }
        public EventDescriptor GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(this, true); }
        public TypeConverter GetConverter() { return TypeDescriptor.GetConverter(this, true); }
        public string GetComponentName() { return TypeDescriptor.GetComponentName(this, true); }
    }
}
