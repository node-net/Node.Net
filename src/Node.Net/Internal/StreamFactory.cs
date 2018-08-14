using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Node.Net.Internal
{
	internal sealed class StreamFactory : IFactory
	{
		public static List<Assembly> GlobalResourceAssemblies { get; } = new List<Assembly>();
		public List<Assembly> ResourceAssemblies { get; set; } = new List<Assembly>();
		public bool ExactMatch { get; set; } = false;
		private string ignoreFilter;

		public void Refresh()
		{
			ignoreFilter = null;
		}

		public object Create(Type targetType, object source)
		{
			if (targetType == typeof(Stream))
			{
				if (source != null)
				{
					if (source.GetType() == typeof(string)) return Create(source.ToString());
				}
			}
			if (typeof(IStreamSignature).IsAssignableFrom(targetType))
			{
				if (source != null)
				{
					if (typeof(Stream).IsAssignableFrom(source.GetType()))
					{
						return Internal.SignatureReader.GetSignature(source as Stream);
					}
					return Create(targetType, Create(typeof(Stream), source));
				}
			}
			return null;
		}

		public IFactory ParentFactory { get; set; }

		public Stream Create(string name)
		{
			if (File.Exists(name)) return new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);
			foreach (var assembly in ResourceAssemblies)
			{
				foreach (var manifestResourceName in assembly.GetManifestResourceNames())
				{
					if (manifestResourceName == name)
					{
						return assembly.GetManifestResourceStream(manifestResourceName);
					}
					if (!ExactMatch && manifestResourceName.Contains(name))
					{
						return assembly.GetManifestResourceStream(manifestResourceName);
					}
				}
			}
			foreach (var assembly in GlobalResourceAssemblies)
			{
				foreach (var manifestResourceName in assembly.GetManifestResourceNames())
				{
					if (manifestResourceName == name)
					{
						return assembly.GetManifestResourceStream(manifestResourceName);
					}
					if (!ExactMatch && manifestResourceName.Contains(name))
					{
						return assembly.GetManifestResourceStream(manifestResourceName);
					}
				}
			}
			if (name.Contains("(") || name.Contains("*") && name.Contains(".") && name.Contains(")") || name.Contains("|"))
			{
				if (ignoreFilter != name)
				{
					// open file dialog filter
					var ofd = new Microsoft.Win32.OpenFileDialog { Filter = name };
					var result = ofd.ShowDialog();
					if (result == true)
					{
						if (File.Exists(ofd.FileName))
						{
							var stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
							stream.SetFileName(ofd.FileName);
							return stream;
						}
					}
					else
					{
						// cancelled
						ignoreFilter = name;
					}
				}
			}
			if (name.Contains(":"))
			{
				try
				{
					var uri = new Uri(name);
					switch (uri.Scheme)
					{
						case "http":
						case "https":
							{
								using (var webClient = new WebClient())
								{
									return webClient.OpenRead(name);
								}
								//break;
							}
						case "file":
							{
								return new FileStream(uri.LocalPath, FileMode.Open);
								//break;
							}
					}
				}
				catch { }
			}
			if (name.Contains("{"))
			{
				var memory = new MemoryStream(Encoding.UTF8.GetBytes(name));
				return memory;
			}
			return new StackTrace().GetStream(name);
		}
	}
}