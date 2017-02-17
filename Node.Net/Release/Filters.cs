//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class Filters
    {
        public static Func<object, bool?> GetValueFilter<T>(T value, bool? default_result = false) => Collections.Filters.GetValueFilter<T>(value, default_result);
        public static Func<object, bool?> GetStringFilter(string value, string pattern = null, bool? default_result = false) => Collections.Filters.GetStringFilter(value, pattern, default_result);
        public static Func<object, bool?> GetIDictionaryFilter<T>(string key, string keyStringValue, string keyStringPattern = null, bool? default_result = false) where T : IDictionary => Collections.Filters.GetIDictionaryFilter<T>(key, keyStringValue, keyStringPattern, default_result);
        public static Func<object, bool?> GetTypeFilter<T>(string typename,bool exact) where T : IDictionary
        {
            if (exact) return GetIDictionaryFilter<T>("Type", typename);
            else return GetIDictionaryFilter<T>("Type", null, typename);
        }
        public static Func<object, bool?> GetTypeFilter(string typename, bool exact = true) => GetTypeFilter<IDictionary>(typename, exact);
    }
}
