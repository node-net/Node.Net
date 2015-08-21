namespace Node.Net.Json.Internal
{
    class PropertyDescriptor : System.ComponentModel.PropertyDescriptor
    {
        public static System.ComponentModel.PropertyDescriptor Get(System.Collections.IDictionary dictionary,object key,System.Attribute[] attributes)
        {
            object value = dictionary[key];
            if(!object.ReferenceEquals(null,value) && key.GetType() == typeof(string))
            {
                if (value.GetType() == typeof(string)) return new PropertyDescriptor(dictionary,key.ToString(), attributes);
                if (value.GetType().IsValueType) return new PropertyDescriptor(dictionary,key.ToString(), attributes);
            }

            return null;
        }

        private System.Collections.IDictionary idictionary = null;
        public PropertyDescriptor(string name,System.Attribute[] attributes) : base(name,attributes) { }
        public PropertyDescriptor(System.Collections.IDictionary dictionary, string name, System.Attribute[] attributes) : base(name, attributes) { idictionary = dictionary; }
        public override bool CanResetValue(object component) { return true; }
        public override void ResetValue(object component)
        {
            idictionary = component as System.Collections.IDictionary;
        }
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
        public override void SetValue(object component, object value)
        {
            System.Collections.IDictionary dictionary = component as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                dictionary[Name] = value;
            }
        }
        public override System.Type PropertyType
        {
            get
            {
                return idictionary[Name].GetType();
            }
        }

        public override object GetValue(object component)
        {
            System.Collections.IDictionary dictionary = component as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                return dictionary[Name];
            }
            return null;
        }

        public override System.Type ComponentType { get { return null; } }
        public override bool IsReadOnly
        {
            get { return false; }
        }
    }
}
