using System;
using System.IO;
using System.Text;

namespace Node.Net.Beta.Internal.Readers
{
    class Signature : IStreamSignature
    {
        public Signature(byte[] bytesSignature)
        {
            bytes = bytesSignature;
        }

        private byte[] bytes = null;
        private string text = "";
        public string Text
        {
            get
            {
                if (text.Length == 0 && bytes != null)
                {
                    var sb = new StringBuilder();
                    var memory = new MemoryStream(bytes);
                    using (StreamReader sr = new StreamReader(memory))
                    {
                        var all_text = sr.ReadToEnd();

                        for (int i = 0; i < all_text.Length; ++i)
                        {
                            var ch = all_text[i];
                            if (!Char.IsWhiteSpace(ch))
                            {
                                sb.Append(ch);
                            }
                        }
                    }
                    text = sb.ToString();
                }
                return text;
            }
        }
        public bool IsText
        {
            get
            {
                var t = Text;
                foreach (var ch in Text)
                {
                    /*
                    bool isDigit = Char.IsLetterOrDigit(ch);
                    bool isSymbol = Char.IsSymbol(ch);
                    bool isWhiteSpace = Char.IsWhiteSpace(ch);
                    bool isSeperator = Char.IsSeparator(ch);
                    bool isPunctuation = Char.IsPunctuation(ch);
                    */
                    var isControl = Char.IsControl(ch);
                    if (isControl) return false;
                }

                return true;
            }
        }

        public string HexString
        {
            get
            {
                var hex = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                    hex.AppendFormat("{0:x2} ", b);
                return hex.ToString().ToUpper().Trim();
            }
        }

        public override string ToString()
        {
            return IsText ? Text : HexString;
        }
    }
}
