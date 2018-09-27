using System;
using System.IO;
using System.Text;

namespace Node.Net
{
	public static class TextReaderExtension
	{
		public static void EatWhiteSpace(this TextReader reader)
		{
			while (Char.IsWhiteSpace((char)(reader.Peek())))
			{
				reader.Read();
			}
		}

		public static void FastSeek(this TextReader reader, char value, bool ignoreEscaped = false)
		{
			//if (reader.Peek() == -1) return;
			if ((char)reader.Peek() == value) { return; }
			while (true)
			{
				var ichar = reader.Read();
				if (ichar < 0) { return; }
				else
				{
					if ((char)reader.Peek() == value)
					{
						if (ignoreEscaped && ((char)(ichar)) == '\\') { }// ignore
						else { return; }
					}
				}
			}
		}

		public static void FastSeek(this TextReader reader, char[] values, bool ignoreEscaped = false)
		{
			//if (reader.Peek() == -1) return;
			foreach (char ch in values) { if ((char)reader.Peek() == ch) { return; } }
			while (true)
			{
				var ichar = reader.Read();
				if (ichar < 0) { return; }
				else
				{
					foreach (char ch in values)
					{
						if ((char)reader.Peek() == ch)
						{
							if (ignoreEscaped && ((char)(ichar)) == '\\') { }// ignore
							else { return; }
						}
					}
				}
			}
		}

		//private static StringBuilder builder = new StringBuilder();
		//private static int iPeek = -1;
		//private static int iChar = -1;
		//private static char backslash = '\\';
		//private static int iBackslash = (int)'\\';

		public static string SeekIgnoreEscaped(this TextReader reader, char value)
		{
			int iBackslash = '\\';
			int iPeek = reader.Peek();
			if ((char)iPeek == value) { return string.Empty; }
			var done = false;
			var builder = new StringBuilder();
			while (!done)
			{
				int iChar = reader.Read();
				if (iChar < 0) { done = true; }
				else
				{
					if (iChar != iBackslash)
					{
						builder.Append((char)iChar);
					}

					iPeek = reader.Peek();
					if ((char)iPeek == value && iChar != iBackslash)
					{
						done = true;
					}
				}
			}
			if (builder.Length == 0)
			{
				return string.Empty;
			}

			return builder.ToString();
		}

		//public static string Seek(this TextReader reader, char value, bool ignoreEscaped = false) => SeekB(reader, value, ignoreEscaped);
		public static string Seek(this TextReader reader, char value, bool ignoreEscaped = false)
		{
			char backslash = '\\';
			int iPeek = reader.Peek();
			if ((char)iPeek == value) { return string.Empty; }
			var done = false;
			var builder = new StringBuilder();
			while (!done)
			{
				int iChar = reader.Read();
				if (iChar < 0) { done = true; }
				else
				{
					if (ignoreEscaped)
					{
						if (((char)iChar) == backslash) { }
						else { builder.Append((char)iChar); }
					}
					else { builder.Append((char)iChar); }

					iPeek = reader.Peek();
					if ((char)iPeek == value)
					{
						if (ignoreEscaped && ((char)(iChar)) == backslash) { } // ignore
						else { done = true; }
					}
				}
			}
			if (builder.Length == 0)
			{
				return string.Empty;
			}

			return builder.ToString();
		}

		/*
        public static string SeekA(this TextReader reader, char value, bool ignoreEscaped = false)
        {
            var done = false;
            if ((char)reader.Peek() == value) { done = true; return ""; }
            builder.Clear();
            while (!done)
            {
                var ichar = reader.Read();
                if (ichar < 0) { done = true; }
                else
                {
                    if (ignoreEscaped)
                    {
                        if (((char)ichar) == '\\') { }
                        else { builder.Append((char)ichar); }
                    }
                    else { builder.Append((char)ichar); }
                    if ((char)reader.Peek() == value)
                    {
                        if (ignoreEscaped && ((char)(ichar)) == '\\') { } // ignore
                        else { done = true; }
                    }
                }
            }
            if (builder.Length == 0) return "";
            return builder.ToString();
        }*/

		public static string Seek(this TextReader reader, char[] values, bool ignoreEscaped = false)
		{
			var done = false;
			foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
			var builder = new StringBuilder();
			while (!done)
			{
				var ichar = reader.Read();
				if (ichar < 0) { done = true; }
				else
				{
					if (ignoreEscaped)
					{
						if (((char)ichar) == '\\') { }
						else { builder.Append((char)ichar); }
					}
					else { builder.Append((char)ichar); }
					foreach (char ch in values)
					{
						if ((char)reader.Peek() == ch)
						{
							if (ignoreEscaped && ((char)(ichar)) == '\\') { } // ignore
							else { done = true; }
						}
					}
				}
			}
			return builder.ToString();
		}
	}
}