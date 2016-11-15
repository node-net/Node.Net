using System;
using System.Collections;

namespace Node.Net.Collections
{
    public static class Filters
    {
        public static Func<object, bool?> GetValueFilter<T>(T value, bool? default_result = false)
        {
            return new Internal.ValueFilter<T> { Value = value, DefaultResult = default_result }.Include;
        }
        public static Func<object, bool?> GetStringFilter(string value, string pattern = null, bool? default_result = false)
        {
            return new Internal.StringFilter { Value = value, Pattern = pattern, DefaultResult = default_result }.Include;
        }

        public static Func<object, bool?> GetIDictionaryFilter<T>(string key, string keyStringValue, string keyStringPattern = null, bool? default_result = false) where T : IDictionary
        {
            return new Internal.IDictionaryFilter<T> { Key = key, KeyStringValue = keyStringValue, KeyStringPattern = keyStringPattern, DefaultResult = default_result }.Include;
        }
    }
}
