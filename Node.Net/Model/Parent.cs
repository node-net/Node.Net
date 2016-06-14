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
                if (child != null)
                {
                    child.Parent = this;
                    children.Add(child);
                }
            }
            return children.ToArray();
        }
    }
}
