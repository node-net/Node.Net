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

		public static object Construct(this Type type, object[] parameters = null)
		{
			var types = Type.EmptyTypes;
			if (parameters != null)
			{
				var typesList = new List<Type>();
				foreach (var item in parameters)
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
			var constructor = type.GetConstructor(types);
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
			return null;
		}
	}
}