namespace Node.Net.Json
{
    public class Hash : HashBase,System.ComponentModel.ICustomTypeDescriptor,System.Collections.IDictionary
    {
        public Hash() { }
        public Hash(string json) : base(json) { }

        public Hash(System.IO.Stream stream) : base(stream) { }
        public Hash(System.Collections.IDictionary source) : base(source){}

        public new static void Copy(System.Collections.IDictionary source, System.Collections.IDictionary destination, IFilter filter = null)
        {
            HashBase.Copy(source, destination, filter);
        }
        public new static string ToJson(System.Collections.IDictionary source)
        {
            return HashBase.ToJson(source);
        }
        /*
        public new static int GetHashCode(object value)
        {
            return HashBase.GetHashCode(value);
        }*/

        public new static System.Collections.IList GetChildren(System.Collections.IDictionary value)
        {
            return HashBase.GetChildren(value);
        }
   
        public static System.ComponentModel.PropertyDescriptorCollection GetProperties(System.Collections.IDictionary dictionary, System.Attribute[] attributes)
        {
            System.Collections.Generic.List<System.ComponentModel.PropertyDescriptor> descriptors
                = new System.Collections.Generic.List<System.ComponentModel.PropertyDescriptor>();
            foreach (object key in dictionary.Keys)
            {
                System.ComponentModel.PropertyDescriptor propertyDescriptor = Internal.PropertyDescriptor.Get(dictionary, key, attributes);
                if (!object.ReferenceEquals(null, propertyDescriptor)) descriptors.Add(propertyDescriptor);
            }
            return new System.ComponentModel.PropertyDescriptorCollection(descriptors.ToArray());
        }

        public new static System.Collections.IDictionary Convert(System.Collections.IDictionary source,System.Collections.IDictionary typeConversions)
        {
            return HashBase.Convert(source, typeConversions);
        }
        

        #region ICustomTypeDescriptor interface
        public System.ComponentModel.PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
        {
            return GetProperties(this, attributes);
        }
        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) { return this; }
        public string GetClassName() { return System.ComponentModel.TypeDescriptor.GetClassName(this, true); }
        public System.ComponentModel.AttributeCollection GetAttributes() { return System.ComponentModel.TypeDescriptor.GetAttributes(this, true); }
        public System.ComponentModel.PropertyDescriptorCollection GetProperties() { return GetProperties(null); }
        public System.ComponentModel.EventDescriptorCollection GetEvents() { return System.ComponentModel.TypeDescriptor.GetEvents(this, true); }
        public System.ComponentModel.EventDescriptorCollection GetEvents(System.Attribute[] attributes) { return System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true); }
        public object GetEditor(System.Type editorBaseType) { return null; }
        public System.ComponentModel.PropertyDescriptor GetDefaultProperty() { return System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true); }
        public System.ComponentModel.EventDescriptor GetDefaultEvent() { return System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true); }
        public System.ComponentModel.TypeConverter GetConverter() { return System.ComponentModel.TypeDescriptor.GetConverter(this, true); }
        public string GetComponentName() { return System.ComponentModel.TypeDescriptor.GetComponentName(this, true); }
        #endregion
    }
}
