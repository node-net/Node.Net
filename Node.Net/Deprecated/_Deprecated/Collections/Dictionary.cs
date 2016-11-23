﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Node.Net.Deprecated.Collections
{
    public class Dictionary : System.Collections.Generic.Dictionary<string,dynamic>,
        Framework.IDocument,
        ICustomTypeDescriptor
    {
        public Dictionary() { }
        public Dictionary(Stream stream)
        {
            Open(stream);
        }
        private bool readOnly = true;
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = true; }
        }

        public void Open(string name,Stream stream)
        {
            Open(stream);
            this["OpenedFrom"] = name;
        }
        public void Open(Stream stream)
        {
            Clear();
            var reader = new Json.Reader();
            var dictionary = (IDictionary)reader.Read(stream);
            Deprecated.Collections.Copier.Copy(dictionary, this);
        }

        #region ICustomTypeDescriptor interface
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var alist = new List<Attribute>(attributes);
            alist.Add(new ReadOnlyAttribute(true));

            var descriptors =
                new System.Collections.Generic.List<PropertyDescriptor>();
            foreach (string key in Keys)
            {
                var value = this[key];
                if (!object.ReferenceEquals(null, value))
                {
                    if (value.GetType() == typeof(string)||
                        value.GetType() == typeof(double) ||
                        value.GetType() == typeof(long) ||
                        value.GetType() == typeof(bool))
                    {
                        var pd
                            = new ReadOnlyIDictionaryPropertyDescriptor(key, alist.ToArray())
                            {
                                IDictionary = this
                            };
                        descriptors.Add(pd);
                    }
                }

            }
            return new System.ComponentModel.PropertyDescriptorCollection(descriptors.ToArray());
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