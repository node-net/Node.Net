#nullable enable
extern alias NodeNet;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using NodeNet::Node.Net.Diagnostic;
using NodeNet::Node.Net.Service.FileSystem;
using NodeNet::Node.Net.Service.FileSystem.FileSystem;

namespace Node.Net.Test.Service.FileSystem;

/// <summary>
/// Unit tests for LiteDbFileSystem implementation of IFileSystem.
/// </summary>
[TestFixture]
internal class LiteDbFileSystemTests : TestHarness
{
    public LiteDbFileSystemTests() : base("LiteDbFileSystem")
    {
    }

    private LiteDbFileSystem? _fileSystem;
    private string? _testDatabasePath;

    [SetUp]
    public void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _fileSystem = new LiteDbFileSystem(_testDatabasePath);
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose the file system
        _fileSystem?.Dispose();
        _fileSystem = null;

        // Clean up test database file if it exists
        if (_testDatabasePath != null && File.Exists(_testDatabasePath))
        {
            try
            {
                File.Delete(_testDatabasePath);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    #region Exists Tests

    [Test]
    public void Exists_NonExistentFile_ReturnsFalse()
    {
        // Arrange
        var path = "nonexistent/file.txt";

        // Act
        var result = _fileSystem!.Exists(path);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Exists_ExistingFile_ReturnsTrue()
    {
        // Arrange
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("test content");
        _fileSystem!.WriteAllBytes(path, data);

        // Act
        var result = _fileSystem.Exists(path);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Exists_NullPath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _fileSystem!.Exists(null!));
    }

    [Test]
    public void Exists_EmptyPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _fileSystem!.Exists(string.Empty));
    }

    [Test]
    public void Exists_WhitespacePath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _fileSystem!.Exists("   "));
    }

    [Test]
    public void Exists_InvalidPathCharacters_ThrowsArgumentException()
    {
        // Act & Assert - Use null character which is always invalid
        var invalidPath = "test\0file.txt";
        Assert.Throws<ArgumentException>(() => _fileSystem!.Exists(invalidPath));
    }

    [Test]
    public void Exists_AfterDelete_ReturnsFalse()
    {
        // Arrange
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("test content");
        _fileSystem!.WriteAllBytes(path, data);
        _fileSystem.Delete(path);

        // Act
        var result = _fileSystem.Exists(path);

        // Assert
        Assert.That(result, Is.False);
    }

    #endregion

    #region ReadAllBytes Tests

    [Test]
    public void ReadAllBytes_ExistingFile_ReturnsCorrectData()
    {
        // Arrange
        var path = "test/file.txt";
        var expectedData = Encoding.UTF8.GetBytes("Hello, World!");
        _fileSystem!.WriteAllBytes(path, expectedData);

        // Act
        var result = _fileSystem.ReadAllBytes(path);

        // Assert
        Assert.That(result, Is.EqualTo(expectedData));
    }

    [Test]
    public void ReadAllBytes_NonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var path = "nonexistent/file.txt";

        // Act & Assert
        var ex = Assert.Throws<FileNotFoundException>(() => _fileSystem!.ReadAllBytes(path));
        Assert.That(ex!.Message, Does.Contain(path));
    }

    [Test]
    public void ReadAllBytes_NullPath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _fileSystem!.ReadAllBytes(null!));
    }

    [Test]
    public void ReadAllBytes_EmptyPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _fileSystem!.ReadAllBytes(string.Empty));
    }

    [Test]
    public void ReadAllBytes_LargeFile_ReturnsCorrectData()
    {
        // Arrange
        var path = "test/largefile.bin";
        var largeData = new byte[1024 * 1024]; // 1 MB
        new Random(42).NextBytes(largeData);
        _fileSystem!.WriteAllBytes(path, largeData);

        // Act
        var result = _fileSystem.ReadAllBytes(path);

        // Assert
        Assert.That(result, Is.EqualTo(largeData));
    }

    [Test]
    public void ReadAllBytes_EmptyFile_ReturnsEmptyArray()
    {
        // Arrange
        var path = "test/empty.txt";
        _fileSystem!.WriteAllBytes(path, Array.Empty<byte>());

        // Act
        var result = _fileSystem.ReadAllBytes(path);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public void ReadAllBytes_AfterOverwrite_ReturnsNewData()
    {
        // Arrange
        var path = "test/file.txt";
        var originalData = Encoding.UTF8.GetBytes("original");
        var newData = Encoding.UTF8.GetBytes("updated");
        _fileSystem!.WriteAllBytes(path, originalData);
        _fileSystem.WriteAllBytes(path, newData);

        // Act
        var result = _fileSystem.ReadAllBytes(path);

        // Assert
        Assert.That(result, Is.EqualTo(newData));
        Assert.That(result, Is.Not.EqualTo(originalData));
    }

    #endregion

    #region WriteAllBytes Tests

    [Test]
    public void WriteAllBytes_NewFile_CreatesFile()
    {
        // Arrange
        var path = "test/newfile.txt";
        var data = Encoding.UTF8.GetBytes("new content");

        // Act
        _fileSystem!.WriteAllBytes(path, data);

        // Assert
        Assert.That(_fileSystem.Exists(path), Is.True);
        var result = _fileSystem.ReadAllBytes(path);
        Assert.That(result, Is.EqualTo(data));
    }

    [Test]
    public void WriteAllBytes_ExistingFile_OverwritesFile()
    {
        // Arrange
        var path = "test/file.txt";
        var originalData = Encoding.UTF8.GetBytes("original");
        var newData = Encoding.UTF8.GetBytes("new content");
        _fileSystem!.WriteAllBytes(path, originalData);

        // Act
        _fileSystem.WriteAllBytes(path, newData);

        // Assert
        var result = _fileSystem.ReadAllBytes(path);
        Assert.That(result, Is.EqualTo(newData));
        Assert.That(result, Is.Not.EqualTo(originalData));
    }

    [Test]
    public void WriteAllBytes_NullPath_ThrowsArgumentNullException()
    {
        // Arrange
        var data = Encoding.UTF8.GetBytes("test");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _fileSystem!.WriteAllBytes(null!, data));
    }

    [Test]
    public void WriteAllBytes_EmptyPath_ThrowsArgumentException()
    {
        // Arrange
        var data = Encoding.UTF8.GetBytes("test");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _fileSystem!.WriteAllBytes(string.Empty, data));
    }

    [Test]
    public void WriteAllBytes_NullData_ThrowsArgumentNullException()
    {
        // Arrange
        var path = "test/file.txt";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _fileSystem!.WriteAllBytes(path, null!));
    }

    [Test]
    public void WriteAllBytes_LargeData_WritesCorrectly()
    {
        // Arrange
        var path = "test/largefile.bin";
        var largeData = new byte[5 * 1024 * 1024]; // 5 MB
        new Random(42).NextBytes(largeData);

        // Act
        _fileSystem!.WriteAllBytes(path, largeData);

        // Assert
        var result = _fileSystem.ReadAllBytes(path);
        Assert.That(result, Is.EqualTo(largeData));
    }

    [Test]
    public void WriteAllBytes_EmptyData_WritesEmptyFile()
    {
        // Arrange
        var path = "test/empty.txt";

        // Act
        _fileSystem!.WriteAllBytes(path, Array.Empty<byte>());

        // Assert
        Assert.That(_fileSystem.Exists(path), Is.True);
        var result = _fileSystem.ReadAllBytes(path);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public void WriteAllBytes_NestedPath_CreatesFile()
    {
        // Arrange
        var path = "level1/level2/level3/file.txt";
        var data = Encoding.UTF8.GetBytes("nested content");

        // Act
        _fileSystem!.WriteAllBytes(path, data);

        // Assert
        Assert.That(_fileSystem.Exists(path), Is.True);
        var result = _fileSystem.ReadAllBytes(path);
        Assert.That(result, Is.EqualTo(data));
    }

    [Test]
    public void WriteAllBytes_PathWithBackslashes_NormalizesPath()
    {
        // Arrange
        var path1 = "test\\file.txt";
        var path2 = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");

        // Act
        _fileSystem!.WriteAllBytes(path1, data);

        // Assert - Both paths should refer to the same file
        Assert.That(_fileSystem.Exists(path1), Is.True);
        Assert.That(_fileSystem.Exists(path2), Is.True);
        var result1 = _fileSystem.ReadAllBytes(path1);
        var result2 = _fileSystem.ReadAllBytes(path2);
        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void WriteAllBytes_PathWithDotSegments_NormalizesPath()
    {
        // Arrange
        var path1 = "test/../test/file.txt";
        var path2 = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");

        // Act
        _fileSystem!.WriteAllBytes(path1, data);

        // Assert - Both paths should refer to the same file
        Assert.That(_fileSystem.Exists(path2), Is.True);
        var result = _fileSystem.ReadAllBytes(path2);
        Assert.That(result, Is.EqualTo(data));
    }

    #endregion

    #region Delete Tests

    [Test]
    public void Delete_ExistingFile_RemovesFile()
    {
        // Arrange
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");
        _fileSystem!.WriteAllBytes(path, data);
        Assert.That(_fileSystem.Exists(path), Is.True);

        // Act
        _fileSystem.Delete(path);

        // Assert
        Assert.That(_fileSystem.Exists(path), Is.False);
    }

    [Test]
    public void Delete_NonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var path = "nonexistent/file.txt";

        // Act & Assert
        var ex = Assert.Throws<FileNotFoundException>(() => _fileSystem!.Delete(path));
        Assert.That(ex!.Message, Does.Contain(path));
    }

    [Test]
    public void Delete_NullPath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _fileSystem!.Delete(null!));
    }

    [Test]
    public void Delete_EmptyPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _fileSystem!.Delete(string.Empty));
    }

    [Test]
    public void Delete_AfterDelete_CannotReadFile()
    {
        // Arrange
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");
        _fileSystem!.WriteAllBytes(path, data);
        _fileSystem.Delete(path);

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => _fileSystem.ReadAllBytes(path));
    }

    [Test]
    public void Delete_MultipleFiles_DeletesCorrectly()
    {
        // Arrange
        var path1 = "test/file1.txt";
        var path2 = "test/file2.txt";
        var data = Encoding.UTF8.GetBytes("content");
        _fileSystem!.WriteAllBytes(path1, data);
        _fileSystem.WriteAllBytes(path2, data);

        // Act
        _fileSystem.Delete(path1);

        // Assert
        Assert.That(_fileSystem.Exists(path1), Is.False);
        Assert.That(_fileSystem.Exists(path2), Is.True);
    }

    #endregion

    #region Integration Tests

    [Test]
    public void Integration_WriteReadDelete_WorksCorrectly()
    {
        // Arrange
        var path = "integration/test.txt";
        var data = Encoding.UTF8.GetBytes("integration test content");

        // Act & Assert - Write
        _fileSystem!.WriteAllBytes(path, data);
        Assert.That(_fileSystem.Exists(path), Is.True);

        // Act & Assert - Read
        var readData = _fileSystem.ReadAllBytes(path);
        Assert.That(readData, Is.EqualTo(data));

        // Act & Assert - Delete
        _fileSystem.Delete(path);
        Assert.That(_fileSystem.Exists(path), Is.False);
    }

    [Test]
    public void Integration_MultipleFiles_IndependentOperations()
    {
        // Arrange
        var path1 = "test/file1.txt";
        var path2 = "test/file2.txt";
        var path3 = "test/file3.txt";
        var data1 = Encoding.UTF8.GetBytes("file1");
        var data2 = Encoding.UTF8.GetBytes("file2");
        var data3 = Encoding.UTF8.GetBytes("file3");

        // Act - Write all files
        _fileSystem!.WriteAllBytes(path1, data1);
        _fileSystem.WriteAllBytes(path2, data2);
        _fileSystem.WriteAllBytes(path3, data3);

        // Assert - All files exist
        Assert.That(_fileSystem.Exists(path1), Is.True);
        Assert.That(_fileSystem.Exists(path2), Is.True);
        Assert.That(_fileSystem.Exists(path3), Is.True);

        // Act - Delete middle file
        _fileSystem.Delete(path2);

        // Assert - Other files still exist
        Assert.That(_fileSystem.Exists(path1), Is.True);
        Assert.That(_fileSystem.Exists(path2), Is.False);
        Assert.That(_fileSystem.Exists(path3), Is.True);

        // Assert - Can still read other files
        Assert.That(_fileSystem.ReadAllBytes(path1), Is.EqualTo(data1));
        Assert.That(_fileSystem.ReadAllBytes(path3), Is.EqualTo(data3));
    }

    [Test]
    public void Integration_OverwriteMultipleTimes_LastWriteWins()
    {
        // Arrange
        var path = "test/file.txt";
        var data1 = Encoding.UTF8.GetBytes("version1");
        var data2 = Encoding.UTF8.GetBytes("version2");
        var data3 = Encoding.UTF8.GetBytes("version3");

        // Act
        _fileSystem!.WriteAllBytes(path, data1);
        _fileSystem.WriteAllBytes(path, data2);
        _fileSystem.WriteAllBytes(path, data3);

        // Assert
        var result = _fileSystem.ReadAllBytes(path);
        Assert.That(result, Is.EqualTo(data3));
    }

    #endregion

    #region In-Memory Database Tests

    [Test]
    public void InMemoryDatabase_WorksCorrectly()
    {
        // Arrange
        using var inMemoryFileSystem = new LiteDbFileSystem(null);
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("in-memory content");

        // Act
        inMemoryFileSystem.WriteAllBytes(path, data);

        // Assert
        Assert.That(inMemoryFileSystem.Exists(path), Is.True);
        var result = inMemoryFileSystem.ReadAllBytes(path);
        Assert.That(result, Is.EqualTo(data));
    }

    [Test]
    public void InMemoryDatabase_EmptyPath_WorksCorrectly()
    {
        // Arrange
        using var inMemoryFileSystem = new LiteDbFileSystem(string.Empty);
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");

        // Act
        inMemoryFileSystem.WriteAllBytes(path, data);

        // Assert
        Assert.That(inMemoryFileSystem.Exists(path), Is.True);
    }

    #endregion

    #region Path Normalization Tests

    [Test]
    public void PathNormalization_RemovesLeadingSlashes()
    {
        // Arrange
        var path1 = "/test/file.txt";
        var path2 = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");

        // Act
        _fileSystem!.WriteAllBytes(path1, data);

        // Assert - Both paths should refer to the same file
        Assert.That(_fileSystem.Exists(path2), Is.True);
    }

    [Test]
    public void PathNormalization_RemovesTrailingSlashes()
    {
        // Arrange
        var path1 = "test/file.txt/";
        var path2 = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");

        // Act
        _fileSystem!.WriteAllBytes(path1, data);

        // Assert - Both paths should refer to the same file
        Assert.That(_fileSystem.Exists(path2), Is.True);
    }

    [Test]
    public void PathNormalization_PreventsDirectoryTraversal()
    {
        // Arrange
        var path1 = "../../../etc/passwd";
        var path2 = "etc/passwd";
        var data = Encoding.UTF8.GetBytes("content");

        // Act
        _fileSystem!.WriteAllBytes(path1, data);

        // Assert - Path should be normalized, preventing directory traversal
        // Both paths should refer to the same normalized file
        Assert.That(_fileSystem.Exists(path2), Is.True);
        // The normalized path should also work (path normalization makes both equivalent)
        var result1 = _fileSystem.ReadAllBytes(path2);
        Assert.That(result1, Is.EqualTo(data));
    }

    #endregion

    #region Disposal Tests

    [Test]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var fileSystem = new LiteDbFileSystem(_testDatabasePath);
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");
        fileSystem.WriteAllBytes(path, data);

        // Act & Assert - Should not throw
        fileSystem.Dispose();
        fileSystem.Dispose();
    }

    [Test]
    public void Dispose_AfterDisposal_OperationsFail()
    {
        // Arrange
        var fileSystem = new LiteDbFileSystem(_testDatabasePath);
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");
        fileSystem.WriteAllBytes(path, data);
        fileSystem.Dispose();

        // Act & Assert - Operations should fail after disposal
        // LiteDB throws LiteException when database is disposed
        Assert.Throws<LiteDB.LiteException>(() => fileSystem.Exists(path));
        Assert.Throws<LiteDB.LiteException>(() => fileSystem.ReadAllBytes(path));
        Assert.Throws<LiteDB.LiteException>(() => fileSystem.WriteAllBytes(path, data));
        Assert.Throws<LiteDB.LiteException>(() => fileSystem.Delete(path));
    }

    #endregion
}
