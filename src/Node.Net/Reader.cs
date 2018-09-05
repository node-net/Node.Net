using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net
{
	/// <summary>
	/// Reader
	/// </summary>
	public sealed class Reader : Dictionary<string, Func<Stream, object>>, IRead
	{
		public Reader()
		{
			Add("<", ReadXml);
			Add("[", ReadJSON);
			Add("{", ReadJSON);
			foreach (var signature in new Internal.ImageSourceReader().Signatures)
			{
				Add(signature, ReadImageSource);
			}
		}

		~Reader()
		{
			jsonReader = null;
			Clear();
		}

		//public static Reader Default { get; } = new Reader();
		public object Read(Stream original_stream)
		{
			using (var signatureReader = new Internal.SignatureReader(original_stream))
			{
				var stream = signatureReader.Stream;
				var signature = signatureReader.Signature;
				foreach (string signature_key in Keys)
				{
					if (signature.IndexOf(signature_key) == 0)
					{
						return this[signature_key](stream);
					}
				}
				throw new UnrecognizedSignatureException($"unrecognized signature '{signature.Substring(0, 24)}'");
			}
		}

		public object Read(string filename) => IReadExtension.Read(this, filename);

		private T Convert<T>(object instance)
		{
			if (instance == null)
			{
				return default(T);
			}

			if (instance is T)
			{
				return (T)instance;
			}
			return default(T);
		}

		public T Read<T>(Stream stream) => Convert<T>(Read(stream));

		public T Read<T>(string filename) => Convert<T>(Read(filename));

		private static readonly List<string> xaml_markers = new List<string>
		{
			"http://schemas.microsoft.com/winfx/2006/xaml/presentation",
			"<MeshGeometry3D",
		};

		public static object ReadXml(Stream original_stream)
		{
			using (var signatureReader = new Internal.SignatureReader(original_stream))
			{
				var filename = original_stream.GetFileName();
				var stream = signatureReader.Stream;
				var signature_string = signatureReader.Signature;
				foreach (var marker in xaml_markers)
				{
					if (signature_string.Contains(marker))
					{
						var item = System.Windows.Markup.XamlReader.Load(stream);
						item.SetFileName(filename);
						return item;
					}
				}
				/*
				if (signature_string.Contains("http://schemas.microsoft.com/winfx/2006/xaml/presentation"))
				{
					var item = System.Windows.Markup.XamlReader.Load(stream);
					item.SetFileName(filename);
					return item;
				}*/
				if (signature_string.IndexOf("<") == 0)
				{
					var xdoc = new System.Xml.XmlDocument();
					xdoc.Load(stream);
					xdoc.SetFileName(filename);
					return xdoc;
				}
			}
			return null;
		}

		public object ReadJSON(Stream stream)
		{
			var i = jsonReader.Read(stream);
			var dictionary = i as IDictionary;
			if (dictionary != null)
			{
				dictionary.DeepUpdateParents();
			}

			return i;
		}

		public static object ReadImageSource(Stream stream) => new Internal.ImageSourceReader().Read(stream);

		public Type DefaultDocumentType
		{
			get { return jsonReader.DefaultDocumentType; }
			set { jsonReader.DefaultDocumentType = value; }
		}

		public Type DefaultObjectType
		{
			get { return jsonReader.DefaultObjectType; }
			set { jsonReader.DefaultObjectType = value; }
		}

		public Dictionary<string, Type> ConversionTypeNames
		{
			get { return jsonReader.ConversionTypeNames; }
			set { jsonReader.ConversionTypeNames = value; }
		}

		private Internal.JSONReader jsonReader = new Internal.JSONReader();

		public object Open(string name)
		{
			if (name.IsFileDialogFilter())
			{
				var openFileDialogFilter = name;
				var ofd = new Microsoft.Win32.OpenFileDialog { Filter = openFileDialogFilter };
				var result = ofd.ShowDialog();
				if (result == true)
				{
					try
					{
						return this.Read(ofd.FileName);
					}
					catch (Exception ex)
					{
						throw new InvalidOperationException($"Unable to open file {ofd.FileName}", ex);
					}
				}
			}
			else
			{
				if (name.IsValidFileName())
				{
					return this.Read(name);
				}
			}
			return null;
		}
	}
}