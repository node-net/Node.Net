﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Node.Net.Internal
{
	internal sealed class AbstractFactory : Dictionary<Type, Type>, IFactory
	{
		public IFactory ParentFactory { get; set; }
		public Func<Stream, object> ReadFunction { get; set; } = new Internal.JSONReader().Read;
		public Dictionary<string, Type> IDictionaryTypes { get; set; } = new Dictionary<string, Type>();
		public Type DefaultObjectType { get; set; } = typeof(Dictionary<string, dynamic>);
		public string TypeKey = "Type";

		private bool callingParent = false;
		private CloneFactory CloneFactory { get; } = new CloneFactory();

		public object Create(Type targetType, object source)
		{
			if (targetType == null) return null;

			if (source != null && Resources.Contains(source.ToString()))
			{
				var result = Resources[source.ToString()];
				if (result != null && targetType.IsAssignableFrom(result.GetType())) return result;
			}
			else
			{
				var stream = CreateStream(source);
				if (stream != null && source != null)
				{
					var s = source.ToString();
					var result = CreateFromStream(targetType, stream, source);
					if (result != null)
					{
						if (typeof(string).IsAssignableFrom(source.GetType()))
						{
							Resources.Add(source.ToString(), result);
						}
						return result;
					}
				}
			}

			foreach (var _targetType in Keys)
			{
				var concreteType = this[_targetType];
				if (_targetType.IsAssignableFrom(targetType))
				{
					return concreteType.Construct(source);
				}
			}
			var instance = targetType.Construct(source);
			if (instance != null) return instance;

			instance = ResourceFactory.Create(targetType, source);

			if (instance == null) instance = CloneFactory.Create(targetType, source);
			return instance;
		}

		private Stream CreateStream(object source)
		{
			var stream = source as Stream;
			if (stream == null && source != null && ParentFactory != null && !callingParent)
			{
				callingParent = true;
				try
				{
					stream = ParentFactory.Create(typeof(Stream), source) as Stream;
				}
				finally { callingParent = false; }
			}
			return stream;
		}

		public ResourceDictionary Resources { get; set; } = new ResourceDictionary();
		private readonly ResourceFactory ResourceFactory = new ResourceFactory();

		private object CreateFromStream(Type target_type, Stream stream, object source)
		{
			if (stream == null) return null;
			if (ReadFunction != null)
			{
				var instance = ReadFunction(stream);
				stream.Close();

				var dictionary = instance as IDictionary;
				if (dictionary != null)
				{
					var new_dictionary = IDictionaryExtension.ConvertTypes(dictionary, IDictionaryTypes, DefaultObjectType, TypeKey);
					new_dictionary.DeepUpdateParents();
					if (source != null && source.GetType() == typeof(string))
					{
						string filename = stream.GetFileName();
						if (filename.Length > 0) new_dictionary.SetFileName(filename);
						else new_dictionary.SetFileName(source.ToString());
					}
					instance = new_dictionary;
				}
				if (instance != null)
				{
					if (target_type.IsAssignableFrom(instance.GetType())) return instance;
					if (ParentFactory != null) return ParentFactory.Create(target_type, instance);
				}
			}
			return null;
		}
	}
}