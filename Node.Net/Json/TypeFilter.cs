namespace Node.Net.Json
{
    class TypeFilter : Node.Net.Json.IFilter
    {
        private System.Collections.Generic.List<string> includePatterns = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.List<string> excludePatterns = new System.Collections.Generic.List<string>();

        public TypeFilter(string include_pattern)
        {
            includePatterns.Add(include_pattern);
        }
        public TypeFilter(string ip0, string ip1)
        {
            includePatterns.Add(ip0);
            includePatterns.Add(ip1);
        }
        public TypeFilter(string ip0, string ip1, string ip2, string ip3, string ip4)
        {
            includePatterns.Add(ip0);
            includePatterns.Add(ip1);
            includePatterns.Add(ip2);
            includePatterns.Add(ip3);
            includePatterns.Add(ip4);
        }

        public System.Collections.Generic.List<string> IncludePatterns
        {
            get { return includePatterns; }
        }
        public System.Collections.Generic.List<string> ExcludePatterns
        {
            get { return excludePatterns; }
        }

        public bool Include(object item)
        {
            System.Collections.IDictionary dictionary = Node.Net.Json.KeyValuePair.GetValue(item) as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary) && dictionary.Contains("Type"))
            {
                string typename = dictionary["Type"].ToString();
                foreach (string pattern in excludePatterns)
                {
                    if (typename.Contains(pattern)) return false;
                }
                foreach (string pattern in includePatterns)
                {
                    if (typename.Contains(pattern)) return true;
                }
            }
            return false;
        }
    }
}