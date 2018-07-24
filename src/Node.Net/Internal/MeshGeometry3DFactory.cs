using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
	sealed class MeshGeometry3DFactory : IFactory
	{
		public object Create(Type targetType, object source)
		{
			if (source != null)
			{
				if (typeof(IDictionary).IsAssignableFrom(source.GetType())) return CreateFromDictionary(source as IDictionary);
			}
			if (ParentFactory != null)
			{
				if (source != null)
				{
					return Create(targetType, ParentFactory.Create<IDictionary>(source));
				}
			}
			return null;
		}

		public IFactory ParentFactory { get; set; }

		private Dictionary<string, MeshGeometry3D> cache = new Dictionary<string, MeshGeometry3D>();
		private MeshGeometry3D CreateFromDictionary(IDictionary source)
		{
			if (ParentFactory != null)
			{
				if (source != null)
				{
					var type = source.Get<string>("Type");
					var name = $"MeshGeometry3D.{type}.xaml";
					if (cache.ContainsKey(name)) return cache[name];
					var mesh = ParentFactory.Create<MeshGeometry3D>(name);
					cache.Add(name, mesh);
					return mesh;
				}
			}

			return null;
		}
	}
}
