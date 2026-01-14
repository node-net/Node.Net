using System;
using System.IO;
using LiteDB;
using Node.Net.Service.FileSystem.FileSystem;

namespace Node.Net.Service.FileSystem;

/// <summary>
/// LiteDB-based implementation of IFileSystem that stores files in a LiteDB database.
/// Files are stored as BSON documents with their path as the key and binary data as the value.
/// </summary>
/// <remarks>
/// This implementation provides a virtual file system backed by LiteDB, allowing files to be
/// stored in a single database file rather than the physical file system. This is useful for
/// embedded applications, portable applications, or scenarios where file system access is restricted.
/// </remarks>
public class LiteDbFileSystem : IFileSystem, IDisposable
{
    private readonly string _databasePath;
    private readonly Lazy<ILiteDatabase> _database;
    private const string FilesCollectionName = "files";

    /// <summary>
    /// Initializes a new instance of the LiteDbFileSystem class with the specified database path.
    /// </summary>
    /// <param name="databasePath">The path to the LiteDB database file. If null or empty, uses an in-memory database.</param>
    /// <exception cref="ArgumentException">Thrown when the database path contains invalid characters.</exception>
    public LiteDbFileSystem(string? databasePath = null)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
        {
            // Use in-memory database if no path provided
            _databasePath = ":memory:";
        }
        else
        {
            // Validate path characters
            if (databasePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException("Database path contains invalid characters.", nameof(databasePath));
            }

            _databasePath = databasePath;
        }

        _database = new Lazy<ILiteDatabase>(() => CreateDatabase(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
    }

    /// <summary>
    /// Creates and initializes the LiteDB database instance.
    /// </summary>
    private ILiteDatabase CreateDatabase()
    {
        if (_databasePath == ":memory:")
        {
            return new LiteDatabase(new MemoryStream());
        }

        // Ensure directory exists
        var directory = Path.GetDirectoryName(_databasePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return new LiteDatabase(_databasePath);
    }

    /// <summary>
    /// Gets the LiteDB database instance.
    /// </summary>
    private ILiteDatabase Database => _database.Value;

    /// <summary>
    /// Gets the files collection from the database.
    /// </summary>
    private ILiteCollection<FileDocument> Files => Database.GetCollection<FileDocument>(FilesCollectionName);

    /// <summary>
    /// Normalizes the file path for use as a key in the database.
    /// </summary>
    /// <param name="path">The file path to normalize.</param>
    /// <returns>A normalized path string.</returns>
    private static string NormalizePath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return path;
        }

        // Normalize path separators and remove leading/trailing separators
        var normalized = path.Replace('\\', '/').Trim('/');
        
        // Remove any ".." or "." segments for security
        var parts = normalized.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var result = new System.Collections.Generic.List<string>();
        
        foreach (var part in parts)
        {
            if (part == ".")
            {
                continue;
            }
            if (part == "..")
            {
                if (result.Count > 0)
                {
                    result.RemoveAt(result.Count - 1);
                }
                continue;
            }
            result.Add(part);
        }
        
        return string.Join("/", result);
    }

    /// <summary>
    /// Validates the file path and throws appropriate exceptions if invalid.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <param name="allowEmpty">Whether to allow empty paths (default: false).</param>
    /// <exception cref="ArgumentNullException">Thrown when path is null.</exception>
    /// <exception cref="ArgumentException">Thrown when path is empty or contains invalid characters.</exception>
    private static void ValidatePath(string? path, bool allowEmpty = false)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (!allowEmpty && string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be empty.", nameof(path));
        }

        if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            throw new ArgumentException("Path contains invalid characters.", nameof(path));
        }
    }

    /// <summary>
    /// Determines whether the specified file exists in the LiteDB database.
    /// </summary>
    /// <param name="path">The path to the file to check.</param>
    /// <returns>true if the file exists; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    public bool Exists(string path)
    {
        ValidatePath(path);
        var normalizedPath = NormalizePath(path);
        return Files.Exists(f => f.Path == normalizedPath);
    }

    /// <summary>
    /// Opens a file from the LiteDB database, reads all contents into a byte array, and then closes the file.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>A byte array containing the contents of the file.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file specified by <paramref name="path"/> does not exist.</exception>
    public byte[] ReadAllBytes(string path)
    {
        ValidatePath(path);
        var normalizedPath = NormalizePath(path);
        
        var fileDoc = Files.FindOne(f => f.Path == normalizedPath);
        if (fileDoc == null)
        {
            throw new FileNotFoundException($"File not found: {path}", path);
        }

        return fileDoc.Data ?? Array.Empty<byte>();
    }

    /// <summary>
    /// Creates a new file in the LiteDB database, writes the specified byte array to the file, and then closes the file.
    /// If the target file already exists, it is overwritten.
    /// </summary>
    /// <param name="path">The path to the file to write.</param>
    /// <param name="data">The bytes to write to the file.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> or <paramref name="data"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    public void WriteAllBytes(string path, byte[] data)
    {
        ValidatePath(path);
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var normalizedPath = NormalizePath(path);
        
        var fileDoc = new FileDocument
        {
            Path = normalizedPath,
            Data = data,
            CreatedUtc = DateTimeOffset.UtcNow,
            ModifiedUtc = DateTimeOffset.UtcNow
        };

        // Check if file exists and update, otherwise insert
        var existing = Files.FindOne(f => f.Path == normalizedPath);
        if (existing != null)
        {
            fileDoc.Id = existing.Id;
            fileDoc.CreatedUtc = existing.CreatedUtc;
            fileDoc.ModifiedUtc = DateTimeOffset.UtcNow;
            Files.Update(fileDoc);
        }
        else
        {
            Files.Insert(fileDoc);
        }
    }

    /// <summary>
    /// Deletes the specified file from the LiteDB database.
    /// </summary>
    /// <param name="path">The path to the file to delete.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is empty or contains invalid characters.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file specified by <paramref name="path"/> does not exist.</exception>
    public void Delete(string path)
    {
        ValidatePath(path);
        var normalizedPath = NormalizePath(path);
        
        var fileDoc = Files.FindOne(f => f.Path == normalizedPath);
        if (fileDoc == null)
        {
            throw new FileNotFoundException($"File not found: {path}", path);
        }

        Files.Delete(fileDoc.Id);
    }

    /// <summary>
    /// Disposes the LiteDB database instance.
    /// </summary>
    public void Dispose()
    {
        if (_database.IsValueCreated)
        {
            _database.Value.Dispose();
        }
    }

    /// <summary>
    /// Internal document class for storing file data in LiteDB.
    /// </summary>
    private class FileDocument
    {
        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public byte[]? Data { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
        public DateTimeOffset ModifiedUtc { get; set; }
    }
}
