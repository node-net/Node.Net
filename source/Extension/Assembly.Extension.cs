using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

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
        public static Stream? FindManifestResourceStream(this Assembly assembly, string name)
        {
            foreach (string? resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName == name)
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            foreach (string? resourceName in assembly.GetManifestResourceNames())
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
        public static Stream? GetStream(this Assembly assembly, string name)
        {
            foreach (string? resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.Contains(name))
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            return null;
        }

        public static Dictionary<string, byte[]> GetManifestResourceData(this Assembly assembly, string prefix)
        {
            Dictionary<string, byte[]>? data = new Dictionary<string, byte[]>();
            foreach (string? resource_name in assembly.GetManifestResourceNames())
            {
                if (resource_name.Contains(prefix))
                {
                    string? name = resource_name.Replace(prefix, string.Empty);
                    using MemoryStream? memory = new MemoryStream();
                    Stream? stream = assembly.GetManifestResourceStream(resource_name);
                    if (stream != null)
                    {
                        stream.CopyTo(memory);
                        data.Add(name, memory.ToArray());
                    }
                }
            }
            return data;
        }

		public static string GetFolderPath(this Assembly assembly, Environment.SpecialFolder specialFolder)
		{
			return System.IO.Path.Combine(Environment.GetFolderPath(specialFolder), assembly.GetRelativeDataDirectory());
		}
		public static string GetRelativeDataDirectory(this Assembly assembly)
		{
			string company = string.Empty;
			if (assembly.GetCustomAttribute<AssemblyCompanyAttribute>() is AssemblyCompanyAttribute companyAttribute)
			{
				company = companyAttribute.Company;
			}
			string assemblyName = assembly.GetName().Name ?? "Unknown";
			if (company == assemblyName && company.Contains("."))
			{
				company = company.Substring(0, company.IndexOf('.'));
			}
			string version = assembly.GetName().Version?.ToString() ?? "Unknown";
			return System.IO.Path.Combine(company, assemblyName, version);
		}

		/// <summary>
		/// Gets the project directory information for the specified assembly by looking for a .gitignore file in the directory hierarchy.
		/// </summary>
		/// <param name="assembly">The assembly to find the project directory for.</param>
		/// <returns>A DirectoryInfo object representing the project directory.</returns>
		/// <exception cref="System.IO.DirectoryNotFoundException">Thrown when the project directory cannot be found.</exception>
		public static System.IO.DirectoryInfo GetProjectDirectoryInfo(this Assembly assembly)
		{
			if (new System.IO.FileInfo(assembly.Location).Directory is System.IO.DirectoryInfo di)
			{
				if (di.Exists && di.FindAncestorWithFile(".gitignore") is System.IO.DirectoryInfo projectDir)
				{
					return projectDir;
				}
			}
			throw new System.IO.DirectoryNotFoundException($"Could not find the project directory for Assembly {assembly.GetName().Name}. Ensure the assembly is part of a project with a .gitignore file.");
		}

		/// <summary>
		/// Gets the text content of a manifest resource from the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly containing the manifest resource.</param>
		/// <param name="resourceName">The name of the manifest resource to retrieve.</param>
		/// <returns>The text content of the manifest resource.</returns>
		/// <exception cref="ArgumentNullException">Thrown when assembly or resourceName is null.</exception>
		/// <exception cref="ArgumentException">Thrown when the resource is not found in the assembly.</exception>
		public static string GetManifestResourceText(this Assembly assembly, string resourceName)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			if (string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException(nameof(resourceName));
			var resourceStream = assembly.GetManifestResourceStream(resourceName);
			if (resourceStream == null)
			{
				throw new ArgumentException($"Resource '{resourceName}' not found in assembly '{assembly.GetName().Name}'.");
			}
			using var reader = new System.IO.StreamReader(resourceStream, Encoding.UTF8);
			return reader.ReadToEnd();
		}

		public static byte[] GetManifestResourceBytes(this Assembly assembly, string resourceName)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			if (string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException(nameof(resourceName));
			// Search for exact match first, then partial matches
			if(assembly.GetManifestResourceStream(resourceName) is Stream exactMatchStream)
			{
				using var memoryStream = new MemoryStream();
				exactMatchStream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}

			// If no exact match, search for partial matches
			foreach (string? resource in assembly.GetManifestResourceNames())
			{
				if (resource.Contains(resourceName))
				{
					var resourceStream = assembly.GetManifestResourceStream(resource);
					if (resourceStream != null)
					{
						using var memoryStream = new MemoryStream();
						resourceStream.CopyTo(memoryStream);
						return memoryStream.ToArray();
					}
				}
			}

			throw new ArgumentException($"Resource '{resourceName}' not found in assembly '{assembly.GetName().Name}'.");
		}
	}
}