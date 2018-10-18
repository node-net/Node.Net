using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
	public static class ActionExtension
	{
		public static void Invoke<T>(this Action<T> action,object[] parameters)
		{
			action((T)parameters[0]);
		}

		public static void Invoke<T1,T2>(this Action<T1,T2> action, object[] parameters)
		{
			action((T1)parameters[0],(T2)parameters[1]);
		}
	}
}
