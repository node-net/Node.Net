﻿using System;
using System.Collections;
using System.IO;

namespace Node.Net.Internal
{
	internal sealed class CollectionsFactory : IFactory
	{
		public object Create(Type targetType, object source)
		{
			if (source != null && source is string)
			{
				var stream = StreamFactory.Create(source.ToString());
				if (stream != null)
				{
					var instance = Create(targetType, stream);
					if (instance is IDictionary idictionary)
					{
						idictionary.SetFileName(source.ToString());
					}

					return instance;
				}
			}
			if (targetType == typeof(IDictionary))
			{
				if (source is Stream stream)
				{
					return JSONReader.Read(stream) as IDictionary;
				}
			}
			if (targetType == typeof(IList))
			{
				if (source is Stream stream)
				{
					return JSONReader.Read(stream) as IList;
				}
			}
			return null;
		}

		public StreamFactory StreamFactory { get; set; } = new StreamFactory();
		private readonly Internal.JsonReader JSONReader = new Internal.JsonReader();
	}
}