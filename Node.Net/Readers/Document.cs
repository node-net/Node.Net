using System.Collections.Generic;

namespace Node.Net.Readers
{
    public class Document : Dictionary<string, dynamic>
    {
        public string FileName { get; set; }
    }
}
