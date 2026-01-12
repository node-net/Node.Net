using System.Collections.Generic;

namespace Node.Net
{
    public static class RequestExtension
    {
        public static object?[] GetParameterArray(this JsonRPC.Request request)
        {
            List<object?> results = new List<object?>();
            if (request.Parameters != null)
            {
                foreach (string? key in request.Parameters.Keys)
                {
                    results.Add(request.Parameters[key]);
                }
            }
            return results.ToArray();
        }

        public static object?[] GetParameterArray(this JsonRPC.Request request, string[] names)
        {
            if (names is null)
            {
                return request.GetParameterArray();
            }

            List<object>? results = new List<object>();
            if (request.Parameters != null)
            {
                foreach (string? name in names)
                {
                    if (request.Parameters.ContainsKey(name))
                    {
                        results.Add(request.Parameters[name]);
                    }
                }
            }
            if (results.Count == 0)
            {
                for (int i = 0; i < names.Length; ++i)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (request.Parameters.ContainsKey(i.ToString()))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    {
                        results.Add(request.Parameters[i.ToString()]);
                    }
                }
            }
            return results.ToArray();
        }

        public static object?[] GetParameterArray<T1>(this JsonRPC.Request request, string[] names)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray(names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            return new object?[] { parameters.Get<T1>(0) };
        }

        public static object?[] GetParameterArray<T1, T2>(this JsonRPC.Request request, string[] names)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray(names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            return new object?[] { parameters.Get<T1>(0), parameters.Get<T2>(1) };
        }

        public static object?[] GetParameterArray<T1, T2, T3>(this JsonRPC.Request request, string[] names)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray(names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            return new object?[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2) };
        }

        public static object?[] GetParameterArray<T1, T2, T3, T4>(this JsonRPC.Request request, string[] names)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray(names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            return new object?[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2), parameters.Get<T4>(3) };
        }

        public static object?[] GetParameterArray<T1, T2, T3, T4, T5>(this JsonRPC.Request request, string[] names)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray(names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            return new object?[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2), parameters.Get<T4>(3), parameters.Get<T5>(4) };
        }

        public static object?[] GetParameterArray<T1, T2, T3, T4, T5, T6>(this JsonRPC.Request request, string[] names)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray(names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            return new object?[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2), parameters.Get<T4>(3), parameters.Get<T5>(4), parameters.Get<T6>(5) };
        }
    }
}