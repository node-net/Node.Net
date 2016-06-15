using System.Collections.Generic;

namespace Node.Net.Model
{
    public class Parent : Dictionary<string, dynamic>, IParent
    {
        public Dictionary<string,IChild> GetChildren()
        {
            var children = new Dictionary<string,IChild>();
            foreach (string key in Keys)
            {
                var child = this[key] as IChild;
                if (child != null)
                {
                    child.Parent = this;
                    children.Add(key,child);
                }
            }
            return children;
        }
    }
}
