using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net
{
	public class Element
	{
		public Element() { }
		public Element(IDictionary model)
		{
			_model = model;
		}

		public IDictionary Model { get { return _model; } }
		private IDictionary _model = new Dictionary<string,object>();

		public static Element? Open(Stream stream)
		{
			var reader = new JsonReader
			{
				ConvertFunction = ConvertIDictionaryToElement
			};
			return reader.Read(stream) as Element;
		}

		public static object ConvertIDictionaryToElement(IDictionary dictionary)
		{
			return new Element(dictionary);
		}
	}
}
