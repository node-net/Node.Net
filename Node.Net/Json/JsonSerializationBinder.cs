using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Node.Net.Json
{
    public class JsonSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {

            object instance = AppDomain.CurrentDomain.CreateInstance(assemblyName, typeName);
            return instance.GetType();
        }

        public static Type GetType(string name)
        {
            if (name.Length > 0)
            {
                Type type = Type.GetType(name);
                if (!ReferenceEquals(null, type)) return type;
                // FullName pass
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type atype in assembly.GetTypes())
                    {
                        if (atype.FullName == name) return atype;
                    }
                }
                // Name pass
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type atype in assembly.GetTypes())
                    {
                        if (atype.Name == name) return atype;
                    }
                }
            }
            return null;
        }
    }
}
