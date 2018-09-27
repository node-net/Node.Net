using System.IO;
using System.Reflection;

namespace Node.Net
{
	/// <summary>
	/// Extension Methods for System.Reflection.Assembly
	/// </summary>
	public static class AssemblyExtension
	{
		/// <summary>
		/// Find a ManifestResourceStream in an Assembly
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Stream FindManifestResourceStream(this Assembly assembly, string name)
		{
			foreach (var resourceName in assembly.GetManifestResourceNames())
			{
				if (resourceName == name)
				{
					return assembly.GetManifestResourceStream(resourceName);
				}
			}
			foreach (var resourceName in assembly.GetManifestResourceNames())
			{
				if (resourceName.Contains(name))
				{
					return assembly.GetManifestResourceStream(resourceName);
				}
			}
			return null;
		}

		/// <summary>
		/// Gets a manifest resource stream from an assembly
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Stream GetStream(this Assembly assembly, string name)
		{
			foreach (var resourceName in assembly.GetManifestResourceNames())
			{
				if (resourceName.Contains(name))
				{
					return assembly.GetManifestResourceStream(resourceName);
				}
			}
			return null;
		}
	}
}