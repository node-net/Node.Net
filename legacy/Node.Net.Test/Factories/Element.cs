using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Test
{
    public class Element : IElement
    {
        public Element() { }
        public Element(IDictionary dictionary)
        {
            foreach(string key in dictionary.Keys)
            {
                data.Add(key, dictionary[key]);
            }
        }
        private Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

        public void Clear() { data.Clear(); }
        public bool Contains(string name) { return data.ContainsKey(name); }
        public dynamic Get(string name) { return data[name]; }
        public T Get<T>(string name) { return IElementExtension.Get<T>(this, name); }
        public void Set(string name, dynamic value)
        {
            data[name] = value;
            var element = value as Element;
            if (element != null && element.Parent != this)
            {
                element.Parent = this;
                element.Name = null;
                element.FullName = null;
            }
        }


        public object Parent { get; set; }
    
        public int Count { get { return data.Count; } }

        public ICollection<string> Keys { get { return data.Keys; } }

        public string Name
        {
            get
            {
                if (name == null) name = IElementExtension.GetName(this);
                return name;
            }
            private set { name = value; }
        }
        private string name = null;
        public string FullName
        {
            get
            {
                if (fullName == null) fullName = IElementExtension.GetFullName(this);
                return fullName;
            }
            private set { fullName = value; }
        }
        private string fullName = null;

        public void DeepUpdateParents()
        {

        }
    }
}
