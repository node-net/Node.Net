using System.Collections.Generic;

namespace Node.Net.Model
{
    public class Parent : Dictionary<string, dynamic>, IParent
    {
        public IChild[] GetChildren()
        {
            var children = new List<IChild>();
            foreach (string key in Keys)
            {
                var child = this[key] as IChild;
                if (child != null) children.Add(child);
            }
            return children.ToArray();
        }
    }
}
