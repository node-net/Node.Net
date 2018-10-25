﻿using System.Collections.Generic;

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
		public static object[] GetParameterArray(this JsonRPC.Request request,string[] names)
		{
			if (names is null) return request.GetParameterArray();
			var results = new List<object>();
			if (request.Parameters != null)
			{
				foreach(var name in names)
				{
					if(request.Parameters.ContainsKey(name))
					{
						results.Add(request.Parameters[name]);
					}
					else
					{
						results.Add(null);
					}
				}
			}
			return results.ToArray();
		}
	}
}