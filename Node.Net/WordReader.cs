﻿using System;
using System.Collections.Generic;

namespace Node.Net
{
    public sealed class WordReader : IDisposable
    {
        private List<char> _delimiters;

        public List<char> Delimiters
        {
            get
            {
                return _delimiters ?? (_delimiters = new List<char>
                    {
                        ' ',
                        '\t'
                    });
            }
            set { _delimiters = value; }
        }

        private readonly System.IO.StreamReader _sr;
        private readonly System.Collections.Generic.List<string> _words = new System.Collections.Generic.List<string>();

        public WordReader(System.IO.StreamReader sr)
        {
            _sr = sr;
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && _sr != null)
            {
                _sr.Close();
            }
        }

        #endregion Dispose

        public double ReadDouble()
        {
            return Convert.ToDouble(ReadWord(), System.Globalization.CultureInfo.CurrentCulture);
        }

        public int ReadInt32()
        {
            return Convert.ToInt32(ReadWord(), System.Globalization.CultureInfo.CurrentCulture);
        }

        public string? ReadLine()
        {
            if (_words.Count > 0)
            {
                throw new System.InvalidOperationException("method may not be called with words in the buffer.");
            }
            return _sr.ReadLine();
        }

        public string ReadWord()
        {
            string? word = "";
            if (_words.Count > 0)
            {
                word = _words[0];
                _words.RemoveAt(0);
            }
            else
            {
                string? line = _sr.ReadLine();
                while (line != null && word.Length == 0)
                {
                    string[]? wordsA = line.Split(Delimiters.ToArray());
                    List<string>? wordsB = new System.Collections.Generic.List<string>();
                    foreach (string w in wordsA)
                    {
                        if (w.Length > 0)
                        {
                            wordsB.Add(w);
                        }
                    }
                    string[]? words = wordsB.ToArray();

                    if (words.Length > 0)
                    {
                        word = words[0];
                        for (int i = 1; i < words.Length; ++i)
                        {
                            _words.Add(words[i]);
                        }
                    }
                    else
                    {
                        line = _sr.ReadLine();
                    }
                }
            }

            return word;
        }
    }
}