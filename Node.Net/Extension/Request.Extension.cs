using System.Collections.Generic;

namespace Node.Net
{
	public static class RequestExtension
	{
		public static object[] GetParameterArray(this JsonRPC.Request request)
		{
			var results = new List<object>();
			if (request.Parameters != null)
			{
				foreach (var key in request.Parameters.Keys)
				{
					results.Add(request.Parameters[key]);
				}
			}
			return results.ToArray();
		}

		public static object[] GetParameterArray(this JsonRPC.Request request, string[] names)
		{
			if (names is null)
			{
				return request.GetParameterArray();
			}

			var results = new List<object>();
			if (request.Parameters != null)
			{
				foreach (var name in names)
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
					if (request.Parameters.ContainsKey(i.ToString()))
					{
						results.Add(request.Parameters[i.ToString()]);
					}
				}
			}
			return results.ToArray();
		}

		public static object[] GetParameterArray<T1>(this JsonRPC.Request request, string[] names)
		{
			var parameters = request.GetParameterArray(names);
			return new object[] { parameters.Get<T1>(0) };
		}

		public static object[] GetParameterArray<T1, T2>(this JsonRPC.Request request, string[] names)
		{
			var parameters = request.GetParameterArray(names);
			return new object[] { parameters.Get<T1>(0), parameters.Get<T2>(1) };
		}

		public static object[] GetParameterArray<T1, T2, T3>(this JsonRPC.Request request, string[] names)
		{
			var parameters = request.GetParameterArray(names);
			return new object[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2) };
		}

		public static object[] GetParameterArray<T1, T2, T3, T4>(this JsonRPC.Request request, string[] names)
		{
			var parameters = request.GetParameterArray(names);
			return new object[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2), parameters.Get<T4>(3) };
		}

		public static object[] GetParameterArray<T1, T2, T3, T4, T5>(this JsonRPC.Request request, string[] names)
		{
			var parameters = request.GetParameterArray(names);
			return new object[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2), parameters.Get<T4>(3), parameters.Get<T5>(4) };
		}

		public static object[] GetParameterArray<T1, T2, T3, T4, T5, T6>(this JsonRPC.Request request, string[] names)
		{
			var parameters = request.GetParameterArray(names);
			return new object[] { parameters.Get<T1>(0), parameters.Get<T2>(1), parameters.Get<T3>(2), parameters.Get<T4>(3), parameters.Get<T5>(4), parameters.Get<T6>(5) };
		}

	}
}