using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
	internal class Factory : IFactory
	{
		public static Factory Default { get; } = new Factory();

		public Factory()
		{
			Model3DFactory.ParentFactory = this;
			CollectionsFactory.StreamFactory = StreamFactory;
			AbstractFactory.ParentFactory = this;
			FactoryFunctions = new Dictionary<Type, Func<Type, object, object>>
			{
				{typeof(Stream), StreamFactory.Create },
				{typeof(IStreamSignature),StreamFactory.Create },
				{typeof(Color),new ColorFactory().Create },
				{typeof(Brush),new BrushFactory {ParentFactory=this}.Create },
				{typeof(Material),new MaterialFactory {ParentFactory=this}.Create },
				{typeof(Matrix3D),new Matrix3DFactory().Create },
				{typeof(Transform3D), new Transform3DFactory {ParentFactory = this }.Create },
				{typeof(MeshGeometry3D),new MeshGeometry3DFactory {ParentFactory=this }.Create },
				{typeof(GeometryModel3D),new GeometryModel3DFactory {ParentFactory=this }.Create },
				{typeof(Model3D),Model3DFactory.Create },
				{typeof(Visual3D), new Visual3DFactory {ParentFactory = this }.Create },
				{typeof(object), AbstractFactory.Create }
			};
			Resources = new ResourceDictionary();
		}

		public ResourceDictionary Resources
		{
			get { return AbstractFactory.Resources; }
			set { AbstractFactory.Resources = value; }
		}

		public Dictionary<object, Model3D> Model3DCache
		{
			get { return Model3DFactory.Model3DCache; }
			set { Model3DFactory.Model3DCache = value; }
		}

		public List<Type> Model3DIgnoreTypes
		{
			get { return Model3DFactory.IgnoreTypes; }
		}

        public Dictionary<Type, Func<Type, object, object>> FactoryFunctions { get; }

        public List<Assembly> ManifestResourceAssemblies
		{
			get { return StreamFactory.ResourceAssemblies; }
			set { StreamFactory.ResourceAssemblies = value; }
		}

		public CollectionsFactory CollectionsFactory { get; } = new CollectionsFactory();

		public Func<Stream, object> ReadFunction
		{
			get { return AbstractFactory.ReadFunction; }
			set { AbstractFactory.ReadFunction = value; }
		}

		public Dictionary<Type, Type> AbstractTypes { get { return AbstractFactory; } }

		public Dictionary<string, Type> IDictionaryTypes
		{
			get { return AbstractFactory.IDictionaryTypes; }
			set { AbstractFactory.IDictionaryTypes = value; }
		}

		public Type DefaultObjectType
		{
			get { return AbstractFactory.DefaultObjectType; }
			set { AbstractFactory.DefaultObjectType = value; }
		}

		public Func<IDictionary, Model3D> PrimaryModel3DHelperFunction
		{
			get { return Model3DFactory.PrimaryModel3DHelperFunction; }
			set { Model3DFactory.PrimaryModel3DHelperFunction = value; }
		}

		public bool ScalePrimaryModel3D
		{
			get { return Model3DFactory.ScalePrimaryModel; }
			set { Model3DFactory.ScalePrimaryModel = value; }
		}

		public Dictionary<Type, int> InstanceCounts { get; } = new Dictionary<Type, int>();

		public object Create(Type targetType, object source)
		{
			StreamFactory.Refresh();
			if (source != null && Resources.Contains(source))
			{
				var instance = Resources[source];
				if (targetType.IsInstanceOfType(instance))
				{
					if (!InstanceCounts.ContainsKey(targetType))
					{
						InstanceCounts.Add(targetType, 1);
					}
					else
					{
                        InstanceCounts[targetType]++;
					}

					return instance;
				}
			}

			foreach (var type in FactoryFunctions.Keys)
			{
				if (type.IsAssignableFrom(targetType))
				{
					var instance = FactoryFunctions[type](targetType, source);
					if (instance != null)
					{
						if (!InstanceCounts.ContainsKey(targetType))
						{
							InstanceCounts.Add(targetType, 1);
						}
						else
						{
                            InstanceCounts[targetType]++;
						}

						if (source != null && (source is string) && IsResourceType(targetType) && !Resources.Contains(source.ToString()))
						{
							Resources.Add(source.ToString(), instance);
						}
						return instance;
					}
				}
			}
			return null;
		}

		public bool IsResourceType(Type type)
		{
			if (typeof(Model3D).IsAssignableFrom(type))
			{
				return true;
			}

			if (typeof(MeshGeometry3D).IsAssignableFrom(type))
			{
				return true;
			}

			return false;
		}

		public bool Cache
		{
			get { return Model3DFactory.Cache; }
			set { Model3DFactory.Cache = value; }
		}

		public void ClearCache()
		{
			Model3DFactory.ClearCache();
		}

		public void ClearCache(object model)
		{
			Model3DFactory.ClearCache(model);
		}

		public bool Logging { get; set; } = false;
		public Action<string> LogFunction { get; set; }

		private Model3DFactory Model3DFactory { get; } = new Model3DFactory();
		private StreamFactory StreamFactory { get; } = new StreamFactory();

		private AbstractFactory AbstractFactory { get; } = new AbstractFactory
		{
			{typeof(IDictionary),typeof(Dictionary<string,dynamic>) },
			{typeof(IList),typeof(List<dynamic>) }
		};
	}
}