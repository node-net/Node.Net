using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Diagnostic;

/// <summary>
/// A utility class for testing and diagnostic purposes that provides functionality for working with protobuf messages.
/// </summary>
/// <remarks>This class is designed to facilitate testing operations for a given type. It provides access to the
/// type's metadata and its associated project directory, enabling integration with testing frameworks or custom test
/// setups.</remarks>
public class TestHarness
{
	/// <summary>
	/// Initializes a new instance of the TestHarness class for the specified target type.
	/// </summary>
	/// <param name="targetType">The type of protobuf message to work with.</param>
	/// <exception cref="ArgumentNullException">Thrown when the target type is null.</exception>
	public TestHarness(Type targetType)
	{
		TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
	}

	public TestHarness(string name)
	{
		Name = name ?? throw new ArgumentNullException(nameof(name));
	}

	/// <summary>
	/// Gets the type of protobuf message that this test harness is configured for.
	/// </summary>
	public Type? TargetType { get; }
	public string? Name { get; }

	/// <summary>
	/// Gets the project directory information for the current assembly.
	/// </summary>
	/// <returns>A DirectoryInfo object representing the project directory.</returns>
	public DirectoryInfo GetProjectDirectoryInfo()
	{
		return Assembly.GetExecutingAssembly().GetProjectDirectoryInfo();
	}

	/// <summary>
	/// Gets the target framework moniker (TFM) for the current executing assembly.
	/// </summary>
	/// <returns>The target framework moniker (e.g., "net8.0", "net8.0-windows", "net48"), or "unknown" if not available.</returns>
	private string GetTargetFramework()
	{
		try
		{
			var assembly = Assembly.GetExecutingAssembly();
			
			// Method 1: Try to extract from assembly location path (most reliable for test assemblies)
			// Test assemblies are typically in: bin/Debug/{TargetFramework}/ or bin/Release/{TargetFramework}/
			var assemblyLocation = assembly.Location;
			if (!string.IsNullOrEmpty(assemblyLocation))
			{
				var assemblyDir = Path.GetDirectoryName(assemblyLocation);
				if (!string.IsNullOrEmpty(assemblyDir))
				{
					var dirName = Path.GetFileName(assemblyDir);
					// Check if directory name looks like a TFM (e.g., "net8.0", "net8.0-windows", "net48")
					if (dirName.StartsWith("net", StringComparison.OrdinalIgnoreCase))
					{
						return dirName;
					}
					
					// Check parent directory
					var parentDir = Path.GetDirectoryName(assemblyDir);
					if (!string.IsNullOrEmpty(parentDir))
					{
						var parentDirName = Path.GetFileName(parentDir);
						if (parentDirName.StartsWith("net", StringComparison.OrdinalIgnoreCase))
						{
							return parentDirName;
						}
					}
				}
			}
			
			// Method 2: Use TargetFrameworkAttribute
			var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
			if (targetFrameworkAttribute != null && !string.IsNullOrEmpty(targetFrameworkAttribute.FrameworkName))
			{
				var frameworkName = targetFrameworkAttribute.FrameworkName;
				
				if (frameworkName.Contains(".NETCoreApp"))
				{
					// Extract version and determine TFM
					var versionMatch = System.Text.RegularExpressions.Regex.Match(frameworkName, @"Version=v(\d+)\.(\d+)");
					if (versionMatch.Success)
					{
						var major = int.Parse(versionMatch.Groups[1].Value);
						var minor = int.Parse(versionMatch.Groups[2].Value);
						
						// For .NET 8.0+, check if we have Windows-specific features
						if (major >= 8)
						{
							// Check if we're running on net8.0-windows by looking for Windows-specific types
							try
							{
								var windowsBaseType = Type.GetType("System.Windows.Application, WindowsBase, PresentationFramework");
								if (windowsBaseType != null)
								{
									return $"net{major}.{minor}-windows";
								}
							}
							catch
							{
								// Ignore - not a Windows-specific framework
							}
							
							return $"net{major}.{minor}";
						}
						
						return $"net{major}.{minor}";
					}
				}
				else if (frameworkName.Contains(".NETFramework"))
				{
					// Extract version for .NET Framework
					var versionMatch = System.Text.RegularExpressions.Regex.Match(frameworkName, @"Version=v(\d+)\.(\d+)");
					if (versionMatch.Success)
					{
						var major = int.Parse(versionMatch.Groups[1].Value);
						var minor = int.Parse(versionMatch.Groups[2].Value);
						return $"net{major}{minor}";
					}
				}
			}
		}
		catch
		{
			// If we can't determine the framework, return "unknown"
		}
		
		return "unknown";
	}

	/// <summary>
	/// Retrieves a <see cref="DirectoryInfo"/> object representing the artifacts directory for the current target type
	/// within the project.
	/// </summary>
	/// <remarks>The artifacts directory is located under the "artifacts/test/{TargetFramework}" subdirectory of the project
	/// directory, with a folder named after the fully qualified name of the target type. If the directory does not already
	/// exist, it will be created.</remarks>
	/// <returns>A <see cref="DirectoryInfo"/> object representing the artifacts directory.</returns>
	public DirectoryInfo GetArtifactsDirectoryInfo()
	{
		var projectDirectory = GetProjectDirectoryInfo();
		var targetFramework = GetTargetFramework();
		var directoryName = TargetType?.FullName ?? Name ?? throw new InvalidOperationException("Either TargetType or Name must be set.");
		var artifactsPath = Path.Combine(projectDirectory.FullName, "artifacts", "test", targetFramework, directoryName);
		if (!Directory.Exists(artifactsPath))
		{
			Directory.CreateDirectory(artifactsPath);
		}
		return new DirectoryInfo(artifactsPath);
	}

	/// <summary>
	/// Retrieves a <see cref="FileInfo"/> object representing a file within the artifacts directory.
	/// </summary>
	/// <remarks>This method combines the specified <paramref name="fileName"/> with the path of the artifacts
	/// directory to construct the full file path. The returned <see cref="FileInfo"/> object provides access to file
	/// metadata and operations but does not guarantee the file's existence.</remarks>
	/// <param name="fileName">The name of the file to locate within the artifacts directory. This value cannot be null or empty.</param>
	/// <returns>A <see cref="FileInfo"/> object representing the specified file. The file may not exist; callers should verify its
	/// existence if necessary.</returns>
	public FileInfo GetArtifactFileInfo(string fileName)
	{
		var artifactsDirectory = GetArtifactsDirectoryInfo();
		var filePath = Path.Combine(artifactsDirectory.FullName, fileName);
		return new FileInfo(filePath);
	}


}

