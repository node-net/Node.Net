﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Internal
{
	public sealed class JSONReader : IDisposable
	{
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~JSONReader()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				DefaultArrayType = null;
				DefaultObjectType = null;
				DefaultDocumentType = null;
				CreateDefaultObject = null;
				ConversionTypeNames = null;
				ObjectCount = 0;
			}
		}

		public Type DefaultArrayType = typeof(List<dynamic>);
		public Type DefaultObjectType = typeof(Dictionary<string, dynamic>);
		public Type DefaultDocumentType = typeof(Dictionary<string, dynamic>);
		public Func<IDictionary> CreateDefaultObject { get; set; } = null;
		public Dictionary<string, Type> ConversionTypeNames { get; set; } = new Dictionary<string, Type>();
		public int ObjectCount { get; set; }

		public object Read(Stream stream)
		{
			using (System.IO.TextReader reader = new StreamReader(stream, Encoding.Default, true, 1024, true))
			{
				try
				{
					ObjectCount = 0;
					var item = Read(reader);
					return item;
				}
				catch (Exception e)
				{
					var exception_info = $"JsonRead.Load raised an exception at stream position {stream.Position}";
					throw new Exception(exception_info, e);
				}
			}
		}

		private object Read(System.IO.TextReader reader)
		{
			char objectOpenCharacter = '{';
			char arrayOpenCharacter = '[';
			char doubleQuote = '"';
			char singleQuote = '\'';
			reader.EatWhiteSpace();
			var ichar = reader.Peek();
			if (ichar < 0) throw new InvalidDataException(@"end of stream reached");
			var c = (char)ichar;
			// char type
			//  'n'  null
			//  '\d' number
			//  '"' or '\'' string
			//  'f' or 't' bool
			//  '{' object (hash)
			//  '[' array
			if (c == doubleQuote || c == singleQuote) return ReadString(reader);
			if (c == objectOpenCharacter) return ReadObject(reader);
			if (c == arrayOpenCharacter) return ReadArray(reader);
			if (c == 'f' || c == 't') return ReadBool(reader);
			if (c == 'n') return ReadNull(reader);
			return ReadNumber(reader);
		}

		private object ReadNull(System.IO.TextReader reader)
		{
			reader.EatWhiteSpace();
			var ch = (char)reader.Peek();
			if (ch == 'n')
			{
				reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars n,u,l,l
			}
			return null;
		}

		private object ReadBool(System.IO.TextReader reader)
		{
			reader.EatWhiteSpace();
			var ch = (char)reader.Peek();
			if (ch == 't')
			{
				reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars t,r,u,e
				return true;
			}
			reader.Read(); reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read char f,a,l,s,e
			return false;
		}

		private object ReadNumber(System.IO.TextReader reader)
		{
			reader.EatWhiteSpace();
			char[] endchars = { '}', ']', ',', ' ' };
			var nstr = reader.Seek(endchars);
			if (nstr.Contains(@"."))
			{
				var value = Convert.ToDouble(nstr);
				if (value <= Single.MaxValue) return Convert.ToSingle(nstr);
				return value;
			}
			else
			{
				var value = Convert.ToInt64(nstr);
				if (value <= Int32.MaxValue) return Convert.ToInt32(nstr);
				return value;
			}
		}

		private object ReadString(System.IO.TextReader reader)
		{
			string unicodeDoubleQuotes = @"\u0022";
			string doubleQuotes = @"""";
			string unicodeBackslash = @"\u005c";
			string backslash = @"\";
			reader.EatWhiteSpace();
			string stringResult = reader.SeekIgnoreEscaped((char)reader.Read());
			reader.Read(); // consume escaped character
			return stringResult.Replace(unicodeDoubleQuotes, doubleQuotes).Replace(unicodeBackslash, backslash);
		}

		private object ReadArray(System.IO.TextReader reader)
		{
			var list = Activator.CreateInstance(DefaultArrayType) as IList;
			reader.FastSeek('[');
			var ch = ' ';
			reader.Read(); // consume the '['
			reader.EatWhiteSpace();
			var done = false;
			ch = (char)reader.Peek();
			if (ch == ']')
			{
				done = true;
				reader.Read(); // consume the ']'
			}
			else
			{
				if (ch != 't' && ch != 'f' && ch != 'n')
				{
					if (Char.IsLetter(ch))
					{
						throw new InvalidDataException($"LoadArray char {ch} is not allowed after [");
					}
				}
			}

			while (!done)
			{
				reader.EatWhiteSpace();
				list.Add(Read(reader));
				reader.EatWhiteSpace();
				var ichar = reader.Peek();
				ch = (char)reader.Peek();
				if (ch == ',') reader.Read(); // consume ','

				reader.EatWhiteSpace();
				ch = (char)reader.Peek();
				if (ch == ']')
				{
					reader.Read(); // consume ']'
					done = true;
				}
			}
			return list.Simplify();
		}

		private object ReadObject(System.IO.TextReader reader)
		{
			char objectOpenCharacter = '{';
			char objectCloseCharacter = '}';
			char comma = ',';
			IDictionary dictionary = null;
			if (ObjectCount == 0)
			{
				if (DefaultDocumentType == null) throw new Exception("DefaultDocumentType is null");
				dictionary = Activator.CreateInstance(DefaultDocumentType) as IDictionary;
				if (dictionary == null)
				{
					throw new Exception($"Unable to create instance of {DefaultDocumentType.FullName}");
				}
			}
			else
			{
				if (CreateDefaultObject != null)
				{
					dictionary = CreateDefaultObject();
					if (dictionary == null) { throw new Exception("CreateDefaultObject returned null"); }
				}
				else
				{
					if (DefaultObjectType == null) throw new Exception("DefaultObjectType is null");
					dictionary = Activator.CreateInstance(DefaultObjectType) as IDictionary;
					if (dictionary == null)
					{
						throw new Exception($"Unable to create isntance of {DefaultObjectType.FullName}");
					}
				}
			}

			ObjectCount++;
			reader.FastSeek(objectOpenCharacter);// '{');
			reader.Read(); // consume the '{'
			reader.EatWhiteSpace();
			var done = false;
			if ((char)(reader.Peek()) == objectCloseCharacter)// '}')
			{
				done = true;
				reader.Read(); // consume the '}'
			}
			while (!done)
			{
				reader.EatWhiteSpace();
				var key = ReadString(reader) as string;
				var lastKey = key;
				reader.EatWhiteSpace();
				var ch = (char)reader.Peek();
				reader.Read(); //consume ':'
				dictionary[key] = Read(reader);
				reader.EatWhiteSpace();
				ch = (char)reader.Peek();
				if (ch == comma) reader.Read(); // consume ','

				reader.EatWhiteSpace();
				ch = (char)reader.Peek();
				if (ch == objectCloseCharacter)//'}')
				{
					reader.Read();
					done = true;
				}
			}

			var type = dictionary.Get<string>("Type", "");
			if (type.Length > 0 && ConversionTypeNames.ContainsKey(type))
			{
				if (!ConversionTypeNames[type].IsAssignableFrom(dictionary.GetType()))
				{
					var converted = Activator.CreateInstance(ConversionTypeNames[type]) as IDictionary;
					if (converted == null)
					{
						throw new Exception($"Unable to create instance of {ConversionTypeNames[type].FullName}");
					}
					foreach (var key in dictionary.Keys)
					{
						if (!converted.Contains(key))
						{
							converted.Add(key, dictionary[key]);
						}
					}
					return converted;
				}
			}
			return dictionary;
		}
	}
}