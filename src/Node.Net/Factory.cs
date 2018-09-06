using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net
{
	public sealed class Factory : IFactory
	{
		public Factory()
		{
			ReadFunction = new Reader().Read;
		}

		public Factory(Assembly assembly)
		{
			ReadFunction = new Reader().Read;
			ManifestResourceAssemblies.Add(assembly);
		}

		public T Create<T>()
		{
			T result = IFactoryExtension.Create<T>(this);
			return result;
		}

		public T Create<T>(object source)
		{
			T result = IFactoryExtension.Create<T>(this, source);
			return result;
		}

		public object Create(Type targetType, object source)
		{
			var result = factory.Create(targetType, source);
			if (Logging)
			{
				var sourceInfo = "null";
				if (source != null)
				{
					sourceInfo = source.ToString();
				}

				Log.Add($"Create(typeof({targetType.FullName}),{sourceInfo}");
			}
			return result;
		}

		public ResourceDictionary Resources
		{
			get { return factory.Resources; }
			set { factory.Resources = value; }
		}

		public Dictionary<object, Model3D> Model3DCache
		{
			get { return factory.Model3DCache; }
			set { factory.Model3DCache = value; }
		}

		public List<Type> Model3DIgnoreTypes
		{
			get { return factory.Model3DIgnoreTypes; }
		}

		public Dictionary<Type, Func<Type, object, object>> FactoryFunctions
		{
			get { return factory.FactoryFunctions; }
		}

		public List<Assembly> ManifestResourceAssemblies
		{
			get { return factory.ManifestResourceAssemblies; }
			set { factory.ManifestResourceAssemblies = value; }
		}

		public Dictionary<string, Type> IDictionaryTypes
		{
			get { return factory.IDictionaryTypes; }
			set { factory.IDictionaryTypes = value; }
		}

		public Dictionary<Type, Type> AbstractTypes
		{
			get { return factory.AbstractTypes; }
			set
			{
				factory.AbstractTypes.Clear();
				foreach (var type in value.Keys)
				{
					factory.AbstractTypes[type] = value[type];
				}
			}
		}

		public Type DefaultObjectType
		{
			get { return factory.DefaultObjectType; }
			set { factory.DefaultObjectType = value; }
		}

		public Func<Stream, object> ReadFunction
		{
			get { return factory.ReadFunction; }
			set { factory.ReadFunction = value; }
		}

		public Func<IDictionary, Model3D> PrimaryModel3DHelperFunction
		{
			get { return factory.PrimaryModel3DHelperFunction; }
			set { factory.PrimaryModel3DHelperFunction = value; }
		}

		public bool ScalePrimaryModel3D
		{
			get { return factory.ScalePrimaryModel3D; }
			set { factory.ScalePrimaryModel3D = value; }
		}

		public bool Cache
		{
			get { return factory.Cache; }
			set { factory.Cache = value; }
		}

		public void ClearCache()
		{
			factory.ClearCache();
		}

		public void ClearCache(object model)
		{
			factory.ClearCache(model);
		}

		public bool Logging { get; set; } = false;
		public List<string> Log { get; } = new List<string>();

		public static Transform3D GetScalingTransform(IDictionary source) => Internal.Model3DFactory.GetScalingTransform(source);

		private readonly Internal.Factory factory = new Internal.Factory();
	}
}