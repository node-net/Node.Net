﻿using System;
using System.ComponentModel;

namespace Node.Net.Deprecated.Collections
{
    class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        public DictionaryPropertyDescriptor(MemberDescriptor member_descriptor) : base(member_descriptor)
        {
            //System.ComponentModel.CustomTypeDescriptor
        }
        public override bool CanResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override Type ComponentType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override Type PropertyType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override object GetValue(object component)
        {
            throw new NotImplementedException();
        }
        public override void SetValue(object component, object value)
        {
            throw new NotImplementedException();
        }

        public override bool ShouldSerializeValue(object component)
        {
            throw new NotImplementedException();
        }
    }
}
