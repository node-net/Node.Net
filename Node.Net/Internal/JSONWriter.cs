using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using static System.Environment;

namespace Node.Net.Internal
{
	internal enum JSONFormat { Compact = 0, Indented = 1 };

	internal sealed class JsonWriter : IWrite
	{
		public JSONFormat Format { get; set; } = JSONFormat.Indented;
		private int IndentLevel;
		public string IndentString { get; set; } = "  ";
		public List<Type> IgnoreTypes { get; set; } = new List<Type>();
		private bool WritingArray { get; set; } = false;

		public void Write(Stream stream, object value)
		{
			using (StreamWriter writer = new StreamWriter(stream, Encoding.Default, 1024, true))
			{
				Write(writer, value);
			}
		}

		public string WriteToString(object value)
		{
			var result = "";
			MemoryStream memory = new MemoryStream();
			Write(memory, value);
			memory.Flush();
			memory.Seek(0, SeekOrigin.Begin);
			using (StreamReader sr = new StreamReader(memory))
			{
				result = sr.ReadToEnd();
			}
			return result;
		}

		private void PushIndent()
		{
			IndentLevel++;
		}

		private void PopIndent()
		{
			IndentLevel--;
		}

		private string GetIndent()
		{
			if (Format == JSONFormat.Indented)
			{
				var sb = new StringBuilder();
				while (sb.Length < IndentLevel * 2) { sb.Append(IndentString); }
				return sb.ToString();
			}
			return string.Empty;
		}

		private string GetLineFeed()
		{
			if (Format == JSONFormat.Indented)
			{
				return NewLine;
			}
			return string.Empty;
		}

		private void Write(System.IO.TextWriter writer, object value)
		{
			if (value is null)
			//if (ReferenceEquals(null, value))
			{
				WriteNull(writer);
			}
			else if (value is byte[] x)
			{
				WriteBytes(writer, x);
			}
			else if (value is string)
			{
				WriteString(writer, value);
			}
			else if (value is IDictionary)
			{
				WriteIDictionary(writer, value);
			}
			else if (value is double[,])
			{
				WriteDoubleArray2D(writer, value);
			}
			else if (value is IEnumerable)
			{
				WriteIEnumerable(writer, value);
			}
            else if (value is DateTime dateTime)
            {
                WriteString(writer, dateTime.ToString("o"));
            }
            else if (value is ISerializable)
			{
				WriteISerializable(writer, value as ISerializable);
			}
			else
			{
				WriteValueType(writer, value);
			}
		}

		private static void WriteNull(System.IO.TextWriter writer)
		{
			writer.Write("null");
		}

		private void WriteBytes(System.IO.TextWriter writer, byte[] bytes)
		{
			WriteString(writer, $"base64:{Convert.ToBase64String(bytes)}");
		}

		private void WriteString(System.IO.TextWriter writer, object value)
		{
			var svalue = value.ToString();
			//var escaped_value = svalue;
			// Escape '\' first
			var escaped_value = svalue.Replace("\\", "\\u005c");
			// Escape '"'
			escaped_value = escaped_value.Replace("\"", "\\u0022");
			if (writingPrimitiveValue)
			{
				writer.Write($"\"{escaped_value}\"");
			}
			else
			{
				writer.Write($"{GetIndent()}\"{escaped_value}\"");
			}
		}

		private void WriteValueType(System.IO.TextWriter writer, object value)
		{
			if (value is bool)
			{
				if (writingPrimitiveValue)
				{
					writer.Write(value.ToString().ToLower());
				}
				else
				{
					writer.Write($"{GetIndent()}{value.ToString().ToLower()}");
				}
			}
			else
			{
				if (writingPrimitiveValue)
				{
					writer.Write(value.ToString());
				}
				else
				{
					if ((value is float) || (value is double)
|| (value is int) || (value is long)
|| (value is string))
					{
						writer.Write(value.ToString());
					}
					else { writer.Write($"{GetIndent()}{value}"); }
				}
			}
		}

		private void WriteDoubleArray2D(System.IO.TextWriter writer, object value)
		{
			var array = value as double[,];
			var length0 = array.GetLength(0);
			var length1 = array.GetLength(1);
			var equivalentList = new List<List<double>>();
			for (int i = 0; i < length0; ++i)
			{
				equivalentList.Add(new List<double>());
				for (int j = 0; j < length1; ++j)
				{
					equivalentList[i].Add(array[i, j]);
				}
			}
			WriteIEnumerable(writer, equivalentList);
		}

		private void WriteIEnumerable(System.IO.TextWriter writer, object value)
		{
			WritingArray = true;
			writer.Write("[");
			PushIndent();
			var enumerable = value as System.Collections.IEnumerable;
			var writeCount = 0;
			foreach (object item in enumerable)
			{
				var skip = false;
				if (!(item is null) && IgnoreTypes.Contains(item.GetType()))
				{
					skip = true;
				}

				if (!skip && ((item?.GetType().IsValueType != false)
|| (value is System.Collections.IEnumerable)))
				{
					if (writeCount > 0)
					{
						if (!WritingArray)
						{
							writer.Write($",{GetLineFeed()}{GetIndent()}");
						}
						else { writer.Write(","); }
					}
					Write(writer, item);
					++writeCount;
				}
			}
			PopIndent();
			if (!WritingArray)
			{
				writer.Write($"{GetLineFeed()}{GetIndent()}]{GetLineFeed()}");
			}
			else
			{
				writer.Write($"]");
			}

			WritingArray = false;
		}

		private bool writingPrimitiveValue;

		private void WriteIDictionary(System.IO.TextWriter writer, object value)
		{
			WritingArray = false;
			var index = 0;
			writer.Write("{");
			PushIndent();

			var dictionary = value as System.Collections.IDictionary;

			var keys = new List<string>();
			foreach (string key in dictionary.Keys) { keys.Add(key); }
			//foreach (object key in dictionary.Keys)
			foreach (string key in keys)
			{
				var item = dictionary[key];
				var skip = false;
				if (!(item is null) && IgnoreTypes.Contains(item.GetType()))
				{
					skip = true;
				}

				if (!skip)
				{
					if (index > 0)
					{
						writer.Write($",{GetLineFeed()}{GetIndent()}");
					}
					else
					{
						writer.Write($"{GetLineFeed()}{GetIndent()}");
					}
					writingPrimitiveValue = true;
					Write(writer, key);
					writingPrimitiveValue = false;

					var tmp = dictionary[key];
					if (tmp?.GetType().IsPrimitive != false || (tmp is string))
					{
						writer.Write(":");
						writingPrimitiveValue = true;
						Write(writer, dictionary[key]);
						writingPrimitiveValue = false;
					}
					else
					{
						writer.Write($": ");
						Write(writer, dictionary[key]);
					}

					++index;
				}
			}
			PopIndent();
			if (dictionary.Count > 0)
			{
				writer.Write(GetLineFeed());
			}

			writer.Write($"{GetIndent()}}}");
		}

		private void WriteISerializable(TextWriter writer, ISerializable serializable)
		{
			var info = serializable.GetSerializationInfo();
			WriteIDictionary(writer, info.GetPersistentData());
		}
	}
}