namespace Node.Net
{
    public static class ObjectExtension
    {
        public static object GetKey(this object instance) => Beta.Internal.ObjectExtension.GetKey(instance);
        public static object GetValue(this object instance) => Beta.Internal.ObjectExtension.GetValue(instance);
        public static bool IsKeyValuePair(this object instance) => Beta.Internal.ObjectExtension.IsKeyValuePair(instance);
        public static string GetFullName(this object instance) => Beta.Internal.ObjectExtension.GetFullName(instance);
        public static void SetFullName(this object instance, string fullname) => Beta.Internal.ObjectExtension.SetFullName(instance, fullname);
        public static string GetName(this object instance) => Beta.Internal.ObjectExtension.GetName(instance);
        public static void SetFileName(this object instance, string filename) => Beta.Internal.ObjectExtension.SetFileName(instance, filename);
        public static string GetFileName(this object instance) => Beta.Internal.ObjectExtension.GetFileName(instance);
        public static void SetMetaData(this object instance, string name, object value) => Beta.Internal.ObjectExtension.SetMetaData(instance, name, value);
        public static T GetMetaData<T>(this object instance, string name) => Beta.Internal.ObjectExtension.GetMetaData<T>(instance, name);
        public static bool HasPropertyValue(this object item, string propertyName) => Beta.Internal.ObjectExtension.HasPropertyValue(item, propertyName);
        public static T GetPropertyValue<T>(this object item, string propertyName) => Beta.Internal.ObjectExtension.GetPropertyValue<T>(item, propertyName);
        public static void SetPropertyValue(this object item, string propertyName, object propertyValue) => Beta.Internal.ObjectExtension.SetPropertyValue(item, propertyName, propertyValue);
        public static void SetParent(this object source, object parent)
        {
            Beta.Internal.Collections.MetaData.Default.GetMetaData(source)["Parent"] = parent;
        }
        public static object GetParent(this object source) { return Beta.Internal.Collections.MetaData.Default.GetMetaData(source, "Parent"); }
    }
}
