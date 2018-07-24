using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Node.Net.Internal
{
	internal class ResourceFactory : IFactory
	{
		public object Create(Type targetType, object source)
		{
			if (source != null && Resources.Contains(source))
			{
				var instance = Resources[source];
				if (instance != null && targetType.IsAssignableFrom(instance.GetType()))
				{
					return instance;
				}

			}
			return null;
		}
		public ResourceDictionary Resources { get; set; } = new ResourceDictionary();
	}
}
