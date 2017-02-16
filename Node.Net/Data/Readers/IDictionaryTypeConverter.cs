﻿using System;
using System.Collections;
using System.Reflection;

namespace Node.Net.Data.Readers
{
    public interface IDictionaryTypeConverter
    {
        IDictionary Convert(IDictionary source);
        void AddType(Type type);
        void AddTypes(Assembly assembly);
    }
}
