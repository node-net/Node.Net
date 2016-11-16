﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net
{
    public static class IEnumerableExtension
    {
        //////////////////////////////////////////////////////////////////
        /// Readers
        public static IEnumerable ConvertTypes(IEnumerable source, Dictionary<string, Type> types, string typeKey = "Type") => Readers.IEnumerableExtension.ConvertTypes(source, types, typeKey);
        public static IEnumerable Simplify(IEnumerable source) => Readers.IEnumerableExtension.Simplify(source);

    }
}