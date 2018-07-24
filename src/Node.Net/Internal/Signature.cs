using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Internal
{
	interface IStreamSignature { }
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
				var maxCount = 16;
				var t = Text;
				var i = 0;
				foreach (var ch in Text)
				{
					var isControl = Char.IsControl(ch);
					if (isControl) return false;
					++i;
					if (i >= maxCount) return true;
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
