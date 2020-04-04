using System;
using System.Collections.Generic;

namespace Node.Net.Internal
{
    internal static class TypeExtension
    {
        public static object Construct(this Type type, object value)
        {
            object[] parameters = { value };
            return Construct(type, parameters);
        }

        public static object Construct(this Type type, object[]? parameters = null)
        {
            Type[]? types = Type.EmptyTypes;
            if (parameters != null)
            {
                List<Type>? typesList = new List<Type>();
                foreach (object? item in parameters)
                {
                    if (item != null)
                    {
                        typesList.Add(item.GetType());
                    }
                }
                if (typesList.Count == parameters.Length)
                {
                    types = typesList.ToArray();
                }
            }
            System.Reflection.ConstructorInfo? constructor = type.GetConstructor(types);
            if (constructor != null)
            {
                if (parameters == null || parameters.Length == 0 || parameters[0] == null)
                {
                    return constructor.Invoke(null);
                }
                else
                {
                    return constructor.Invoke(parameters);
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}