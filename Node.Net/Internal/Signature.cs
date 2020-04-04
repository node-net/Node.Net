using System;
using System.IO;
using System.Text;

namespace Node.Net.Internal
{
    internal interface IStreamSignature
    { }

    internal class Signature : IStreamSignature
    {
        public Signature(byte[] bytesSignature)
        {
            bytes = bytesSignature;
        }

        private readonly byte[] bytes = new byte[0];// null;
        private string text = "";

        public string Text
        {
            get
            {
                if (text.Length == 0 && bytes != null)
                {
                    StringBuilder? sb = new StringBuilder();
                    MemoryStream? memory = new MemoryStream(bytes);
                    using (StreamReader sr = new StreamReader(memory))
                    {
                        string? all_text = sr.ReadToEnd();

                        for (int i = 0; i < all_text.Length; ++i)
                        {
                            char ch = all_text[i];
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
                const int maxCount = 16;
                string? t = Text;
                int i = 0;
                foreach (char ch in Text)
                {
                    bool isControl = Char.IsControl(ch);
                    if (isControl)
                    {
                        return false;
                    }

                    ++i;
                    if (i >= maxCount)
                    {
                        return true;
                    }
                }

                return true;
            }
        }

        public string HexString
        {
            get
            {
                StringBuilder? hex = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                {
                    hex.AppendFormat("{0:x2} ", b);
                }

                return hex.ToString().ToUpper().Trim();
            }
        }

        public override string ToString()
        {
            return IsText ? Text : HexString;
        }
    }
}