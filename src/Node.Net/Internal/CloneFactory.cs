using System;
using System.Collections;
using System.IO;
using System.Windows.Markup;

namespace Node.Net.Internal
{
	internal class CloneFactory : IFactory
	{
		public Func<Stream, object> ReadFunction { get; set; } = new Node.Net.Reader().Read;
		public Action<Stream, object> WriteFunction { get; set; } = Write;

		public object Create(Type targetType, object source)
		{
			if (source == null) return null;
			if (targetType.IsAssignableFrom(source.GetType()))
			{
				using (MemoryStream memory = new MemoryStream())
				{
					WriteFunction(memory, source);
					memory.Flush();
					memory.Seek(0, SeekOrigin.Begin);
					var clone = ReadFunction(memory);
					if (clone != null && targetType.IsAssignableFrom(clone.GetType())) return clone;
				}
			}
			return null;
		}

		private static void Write(Stream stream, object value)
		{
			if (typeof(IDictionary).IsAssignableFrom(value.GetType()))
			{
				new JSONWriter().Write(stream, value);
			}
			else
			{
				XamlWriter.Save(value, stream);
			}
		}
	}
}