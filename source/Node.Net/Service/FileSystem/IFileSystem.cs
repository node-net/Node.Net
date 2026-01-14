namespace Node.Net.Service.FileSystem.FileSystem;

/// <summary>
/// Combined interface for file system operations that provides both read-only and mutable capabilities.
/// This interface inherits from both <see cref="IReadOnlyFileSystem"/> and <see cref="IMutableFileSystem"/>,
/// allowing a single interface to be used when both read and write operations are needed.
/// </summary>
/// <remarks>
/// This interface combines the functionality of <see cref="IReadOnlyFileSystem"/> and <see cref="IMutableFileSystem"/>.
/// All methods from both parent interfaces are available through this interface. The methods are explicitly documented
/// below to ensure maximum IntelliSense visibility and make the interface self-contained without requiring navigation
/// to parent interfaces.
/// </remarks>
public interface IFileSystem 
{
    // Methods inherited from IReadOnlyFileSystem

    /// <summary>
    /// Determines whether the specified file exists.
    /// Inherited from <see cref="IReadOnlyFileSystem"/>.
    /// </summary>
    /// <param name="path">The path to the file to check.</param>
    /// <returns>true if the file exists; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when <paramref name="path"/> is outside the allowed directory.</exception>
    new bool Exists(string path);

    /// <summary>
    /// Opens a binary file, reads all contents into a byte array, and then closes the file.
    /// Inherited from <see cref="IReadOnlyFileSystem"/>.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>A byte array containing the contents of the file.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file specified by <paramref name="path"/> does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when <paramref name="path"/> is outside the allowed directory.</exception>
    new byte[] ReadAllBytes(string path);

    // Methods inherited from IMutableFileSystem

    /// <summary>
    /// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten. If the parent directory does not exist, it is created.
    /// Inherited from <see cref="IMutableFileSystem"/>.
    /// </summary>
    /// <param name="path">The path to the file to write.</param>
    /// <param name="data">The bytes to write to the file.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> or <paramref name="data"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when <paramref name="path"/> is outside the allowed directory.</exception>
    new void WriteAllBytes(string path, byte[] data);

    /// <summary>
    /// Deletes the specified file.
    /// Inherited from <see cref="IMutableFileSystem"/>.
    /// </summary>
    /// <param name="path">The path to the file to delete.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file specified by <paramref name="path"/> does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when <paramref name="path"/> is outside the allowed directory.</exception>
    new void Delete(string path);
}
