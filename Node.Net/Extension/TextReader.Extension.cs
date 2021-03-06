﻿using System;
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
                int ichar = reader.Read();
                if (ichar < 0) { return; }
                else
                {
                    if ((char)reader.Peek() == value)
                    {
                        if (!ignoreEscaped || ((char)(ichar)) != '\\') { return; }
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
                int ichar = reader.Read();
                if (ichar < 0) { return; }
                else
                {
                    foreach (char ch in values)
                    {
                        if ((char)reader.Peek() == ch)
                        {
                            if (!ignoreEscaped || ((char)(ichar)) != '\\') { return; }
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
            const int iBackslash = '\\';
            int iPeek = reader.Peek();
            if ((char)iPeek == value) { return string.Empty; }
            bool done = false;
            StringBuilder? builder = new StringBuilder();
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
                    // Added 3/21/2019 to correct parsing of \\ embedded in a json string
                    else
                    {
                        if (iChar == iBackslash)
                        {
                            builder.Append((char)iChar);
                        }
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
            const char backslash = '\\';
            int iPeek = reader.Peek();
            if ((char)iPeek == value) { return string.Empty; }
            bool done = false;
            StringBuilder? builder = new StringBuilder();
            while (!done)
            {
                int iChar = reader.Read();
                if (iChar < 0) { done = true; }
                else
                {
                    if (ignoreEscaped)
                    {
                        if (((char)iChar) != backslash) { builder.Append((char)iChar); }
                    }
                    else { builder.Append((char)iChar); }

                    iPeek = reader.Peek();
                    if ((char)iPeek == value)
                    {
                        if (!ignoreEscaped || ((char)(iChar)) != backslash) { done = true; }
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
            bool done = false;
            foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            StringBuilder? builder = new StringBuilder();
            while (!done)
            {
                int ichar = reader.Read();
                if (ichar < 0) { done = true; }
                else
                {
                    if (ignoreEscaped)
                    {
                        if (((char)ichar) != '\\') { builder.Append((char)ichar); }
                    }
                    else { builder.Append((char)ichar); }
                    foreach (char ch in values)
                    {
                        if ((char)reader.Peek() == ch)
                        {
                            if (!ignoreEscaped || ((char)(ichar)) != '\\') { done = true; }
                        }
                    }
                }
            }
            return builder.ToString();
        }
    }
}