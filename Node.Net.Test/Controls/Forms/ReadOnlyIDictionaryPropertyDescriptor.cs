using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls.Test.Forms
{
    public class ReadOnlyIDictionaryPropertyDescriptor : PropertyDescriptor
    {
        private IDictionary idictionary = null;
        public IDictionary IDictionary
        {
            get { return idictionary; }
            set { idictionary = value; }
        }

        public ReadOnlyIDictionaryPropertyDescriptor(IDictionary dictionary,string keyValue, Attribute[] attributes) : base(keyValue, attributes)
        {
            idictionary = dictionary;
        }
        public override bool CanResetValue(object component) { return false; }
        public override void ResetValue(object component) { idictionary = component as IDictionary; }
        public override bool ShouldSerializeValue(object component) { idictionary = component as IDictionary; return false; }
        public override bool IsReadOnly { get { return true; } }
        public override void SetValue(object component, object value)
        {
            idictionary = component as IDictionary;
        }
        public override Type PropertyType
        {
            get
            {
                if(idictionary != null) return idictionary[Name].GetType();
                return typeof(string);
            }
        }

        public override object GetValue(object component)
        {
            IDictionary d = component as IDictionary;
            if (!object.ReferenceEquals(null, d))
            {
                return d[Name];
            }
            if (idictionary != null)
            {
                return idictionary[Name];
            }
            return null;
        }

        public override Type ComponentType
        {
            get
            {
                if (idictionary != null) return idictionary.GetType();
                return typeof(IDictionary);
            }
        }
    }
}
