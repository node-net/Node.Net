namespace Node.Net
{
    public static class ObjectExtension
    {
        public static object GetKey(this object item) => Collections.ObjectExtension.GetKey(item);
        public static object GetValue(this object item) => Collections.ObjectExtension.GetValue(item);
        public static bool IsKeyValuePair(this object item) => Collections.ObjectExtension.IsKeyValuePair(item);
        public static object GetParent(this object item) => Collections.ObjectExtension.GetParent(item);
        public static void SetParent(this object item, object parent) => Collections.ObjectExtension.SetParent(item, parent);
        public static T GetNearestAncestor<T>(this object child) => Collections.ObjectExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this object child) => Collections.ObjectExtension.GetFurthestAncestor<T>(child);
        public static object GetRootAncestor(this object child) => Collections.ObjectExtension.GetRootAncestor(child);
    }
}
