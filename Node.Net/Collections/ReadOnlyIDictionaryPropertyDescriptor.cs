using System;
using System.Collections;
using System.ComponentModel;

namespace Node.Net.Collections
{
    public class ReadOnlyIDictionaryPropertyDescriptor : PropertyDescriptor
    {
        private IDictionary idictionary = null;
        public IDictionary IDictionary
        {
            get { return idictionary; }
            set { idictionary = value; }
        }

        public ReadOnlyIDictionaryPropertyDescriptor(string keyValue, Attribute[] attributes) : base(keyValue, attributes)
        {
        }
        public override bool CanResetValue(object component) { return true; }
        public override void ResetValue(object component) { idictionary = component as IDictionary; }
        public override bool ShouldSerializeValue(object component) { idictionary = component as IDictionary; return false; }
        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
        public override void SetValue(object component, object value)
        {
            idictionary = component as IDictionary;
        }
        public override Type PropertyType
        {
            get
            {
                return idictionary[Name].GetType();
            }
        }

        public override object GetValue(object component)
        {
            var d = component as IDictionary;
            if (!object.ReferenceEquals(null, d))
            {
                return d[Name];
            }
            return null;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(IDictionary);
            }
        }
    }
}
