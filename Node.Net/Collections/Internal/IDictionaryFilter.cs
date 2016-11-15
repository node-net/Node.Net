using System.Collections;

namespace Node.Net.Collections.Internal
{
    class IDictionaryFilter<T> : ValueFilter<T> where T : IDictionary
    {
        public string Key { get; set; }
        public string KeyStringValue { get; set; }
        public string KeyStringPattern { get; set; }

        public override bool? Include(object value)
        {
            var base_result = base.Include(value);
            if (base_result.HasValue && base_result.Value) return true;
            var dictionary = value as IDictionary;
            if (dictionary != null && Key != null)
            {
                if (dictionary.Contains(Key))
                {
                    var i = dictionary[Key];
                    if (i != null && i.GetType() == typeof(string))
                    {
                        if (KeyStringPattern != null)
                        {
                            if (i.ToString().Contains(KeyStringPattern)) return true;
                        }
                        if (KeyStringValue == i.ToString()) return true;
                    }
                }
            }

            return DefaultResult;
        }
    }
}
