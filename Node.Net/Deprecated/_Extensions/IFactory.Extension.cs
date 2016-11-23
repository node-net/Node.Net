﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net._Extensions
{
    static class IFactoryExtension
    {
        public static T Create<T>(IFactory factory, object value)
        {
            return (T)(object)factory.Create(typeof(T), value);
        }
    }
}