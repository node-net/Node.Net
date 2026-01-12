using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

	/// <summary>
	/// Gets the type of protobuf message that this test harness is configured for.
	/// </summary>
	public Type TargetType { get; }

	/// <summary>
	/// Gets the project directory information for the current assembly.
	/// </summary>
	/// <returns>A DirectoryInfo object representing the project directory.</returns>
	public DirectoryInfo GetProjectDirectoryInfo()
	{
		return Assembly.GetExecutingAssembly().GetProjectDirectoryInfo();
	}

	/// <summary>
	/// Retrieves a <see cref="DirectoryInfo"/> object representing the artifacts directory for the current target type
	/// within the project.
	/// </summary>
	/// <remarks>The artifacts directory is located under the "artifacts/test" subdirectory of the project
	/// directory, with a folder named after the fully qualified name of the target type. If the directory does not already
	/// exist, it will be created.</remarks>
	/// <returns>A <see cref="DirectoryInfo"/> object representing the artifacts directory.</returns>
	public DirectoryInfo GetArtifactsDirectoryInfo()
	{
		var projectDirectory = GetProjectDirectoryInfo();
		var artifactsPath = Path.Combine(projectDirectory.FullName, "artifacts", "test", TargetType!.FullName!);
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

