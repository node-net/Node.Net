namespace Node.Net.Extensions
{
    public static class IChildExtension
    {
        public static T GetFirstAncestor<T>(IChild child)
        {
            if (child != null)
            {
                var ancestor = (T)child.Parent;
                if (ancestor != null) return ancestor;
                return GetFirstAncestor<T>(child.Parent as IChild);
            }
            return default(T);
        }
    }
}
