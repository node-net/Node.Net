using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.IO
{
    public class WordReader : System.IDisposable
    {
        private System.IO.StreamReader _sr = null;
        private System.Collections.Generic.List<string> _words = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.List<char> _delimiters = new System.Collections.Generic.List<char>();

        /*
        public WordReader(string fileName)
        {
            _sr = new System.IO.StreamReader(fileName);
            _delimiters.Add(' ');
            _delimiters.Add('\t');
        }*/

        public WordReader(System.IO.StreamReader sr)
        {
            _sr = sr;
            _delimiters.Add(' ');
            _delimiters.Add('\t');
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_sr != null) _sr.Close();
            }
        }
        #endregion


        public string ReadLine()
        {
            if (_words.Count > 0)
            {
                throw new System.InvalidOperationException("method may not be called with words in the buffer.");
            }
            return _sr.ReadLine();
        }

        public string ReadWord()
        {
            string word = "";
            if (_words.Count > 0)
            {
                word = _words[0];
                _words.RemoveAt(0);
            }
            else
            {
                string line = _sr.ReadLine();
                while (line != null && word.Length == 0)
                {
                    string[] wordsA = line.Split(_delimiters.ToArray());
                    System.Collections.Generic.List<string> wordsB = new System.Collections.Generic.List<string>();
                    foreach (string w in wordsA)
                    {
                        if (w.Length > 0) wordsB.Add(w);
                    }
                    string[] words = wordsB.ToArray();

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

        public long ReadInt32()
        {
            return System.Convert.ToInt32(ReadWord(), System.Globalization.CultureInfo.CurrentCulture);
        }

        public double ReadDouble()
        {
            return System.Convert.ToDouble(ReadWord(), System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
