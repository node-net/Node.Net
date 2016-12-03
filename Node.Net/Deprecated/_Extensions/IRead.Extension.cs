//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Extensions
{
    static class IReadExtension
    {
        public static T Create<T>(IFactory factory, object value)
        {
            return (T)(object)factory.Create(typeof(T), value);
        }
    }
}
