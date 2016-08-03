using System;
namespace Node.Net.Deprecated.Collections
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
        public new static string ToJson(System.Collections.IDictionary source) => HashBase.ToJson(source);

        public static Hash Parse(string[] args)
        {
            var hash = new Hash();
            foreach (string arg in args)
            {
                if (arg.IndexOf('=') > -1)
                {
                    char[] delimiters = { '=' };
                    var words = arg.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    var key = words[0].Trim();
                    var value = words[1].Trim().Replace("\"", "");
                    if (key.Length > 0 && value.Length > 0)
                    {
                        hash[key] = value;
                    }
                }
            }
            return hash;
        }

        public new static System.Collections.IList GetChildren(System.Collections.IDictionary value) => HashBase.GetChildren(value);

        public static System.ComponentModel.PropertyDescriptorCollection GetProperties(System.Collections.IDictionary dictionary, System.Attribute[] attributes)
        {
            var descriptors
                = new System.Collections.Generic.List<System.ComponentModel.PropertyDescriptor>();
            foreach (object key in dictionary.Keys)
            {
                var propertyDescriptor = Internal.PropertyDescriptor.Get(dictionary, key, attributes);
                if (!object.ReferenceEquals(null, propertyDescriptor)) descriptors.Add(propertyDescriptor);
            }
            return new System.ComponentModel.PropertyDescriptorCollection(descriptors.ToArray());
        }

        public new static System.Collections.IDictionary Convert(System.Collections.IDictionary source, System.Collections.IDictionary typeConversions) => HashBase.Convert(source, typeConversions);


        #region ICustomTypeDescriptor interface
        public System.ComponentModel.PropertyDescriptorCollection GetProperties(System.Attribute[] attributes) => GetProperties(this, attributes);
        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) => this;
        public string GetClassName() => System.ComponentModel.TypeDescriptor.GetClassName(this, true);
        public System.ComponentModel.AttributeCollection GetAttributes() => System.ComponentModel.TypeDescriptor.GetAttributes(this, true);
        public System.ComponentModel.PropertyDescriptorCollection GetProperties() => GetProperties(null);
        public System.ComponentModel.EventDescriptorCollection GetEvents() => System.ComponentModel.TypeDescriptor.GetEvents(this, true);
        public System.ComponentModel.EventDescriptorCollection GetEvents(System.Attribute[] attributes) => System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true);
        public object GetEditor(System.Type editorBaseType) => null;
        public System.ComponentModel.PropertyDescriptor GetDefaultProperty() => System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true);
        public System.ComponentModel.EventDescriptor GetDefaultEvent() => System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true);
        public System.ComponentModel.TypeConverter GetConverter() => System.ComponentModel.TypeDescriptor.GetConverter(this, true);
        public string GetComponentName() => System.ComponentModel.TypeDescriptor.GetComponentName(this, true);
        #endregion
    }
}
