using System.Collections.Generic;

namespace Node.Net.Collections
{
    public class Dictionary : Dictionary<string, dynamic>
    {
        public Dictionary() { }
        public Dictionary(string type) { Add("Type", type); }
        public string Type
        {
            get
            {
                var value = IDictionaryExtension.Get<string>(this, nameof(Type));
                return (value == null) ? string.Empty : value.ToString();
            }
        }
        public string Key
        {
            get
            {
                var key = ObjectExtension.GetKey(this);
                return (key == null) ? string.Empty : key.ToString();
            }
        }
        public string FullKey => ObjectExtension.GetFullKey(this);
        public string[] Types => IDictionaryExtension.CollectValues<string>(this, nameof(Type));
        public void UpdateParentReferences() => IDictionaryExtension.DeepUpdateParents(this);
        
    }
}
