#nullable enable
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Node.Net.Diagnostic;
using Node.Net.Service.FileSystem;
using Node.Net.Service.FileSystem.FileSystem;

namespace Node.Net.Test.Service.FileSystem;

/// <summary>
/// Unit tests for LiteDbFileSystem implementation of IFileSystem.
/// </summary>
internal class LiteDbFileSystemTests : TestHarness
{
    public LiteDbFileSystemTests() : base("LiteDbFileSystem")
    {
    }

    private LiteDbFileSystem? _fileSystem;
    private string? _testDatabasePath;

    private void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _fileSystem = new LiteDbFileSystem(_testDatabasePath);
    }

    private void TearDown()
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
    public async Task Exists_NonExistentFile_ReturnsFalse()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "nonexistent/file.txt";

            // Act
            var result = _fileSystem!.Exists(path);

            // Assert
            await Assert.That(result).IsFalse();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Exists_ExistingFile_ReturnsTrue()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("test content");
            _fileSystem!.WriteAllBytes(path, data);

            // Act
            var result = _fileSystem.Exists(path);

            // Assert
            await Assert.That(result).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Exists_NullPath_ThrowsArgumentNullException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _fileSystem!.Exists(null!)).Throws<ArgumentNullException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Exists_EmptyPath_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _fileSystem!.Exists(string.Empty)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Exists_WhitespacePath_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _fileSystem!.Exists("   ")).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Exists_InvalidPathCharacters_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Act & Assert - Use null character which is always invalid
            var invalidPath = "test\0file.txt";
            await Assert.That(() => _fileSystem!.Exists(invalidPath)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Exists_AfterDelete_ReturnsFalse()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("test content");
            _fileSystem!.WriteAllBytes(path, data);
            _fileSystem.Delete(path);

            // Act
            var result = _fileSystem.Exists(path);

            // Assert
            await Assert.That(result).IsFalse();
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region ReadAllBytes Tests

    [Test]
    public async Task ReadAllBytes_ExistingFile_ReturnsCorrectData()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/file.txt";
            var expectedData = Encoding.UTF8.GetBytes("Hello, World!");
            _fileSystem!.WriteAllBytes(path, expectedData);

            // Act
            var result = _fileSystem.ReadAllBytes(path);

            // Assert
            await Assert.That(result).IsEqualTo(expectedData);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task ReadAllBytes_NonExistentFile_ThrowsFileNotFoundException()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "nonexistent/file.txt";

            // Act & Assert
            var ex = await Assert.That(() => _fileSystem!.ReadAllBytes(path)).Throws<FileNotFoundException>();
            await Assert.That(ex.Message.Contains(path)).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task ReadAllBytes_NullPath_ThrowsArgumentNullException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _fileSystem!.ReadAllBytes(null!)).Throws<ArgumentNullException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task ReadAllBytes_EmptyPath_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _fileSystem!.ReadAllBytes(string.Empty)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task ReadAllBytes_LargeFile_ReturnsCorrectData()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/largefile.bin";
            var largeData = new byte[1024 * 1024]; // 1 MB
            new Random(42).NextBytes(largeData);
            _fileSystem!.WriteAllBytes(path, largeData);

            // Act
            var result = _fileSystem.ReadAllBytes(path);

            // Assert
            await Assert.That(result).IsEqualTo(largeData);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task ReadAllBytes_EmptyFile_ReturnsEmptyArray()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/empty.txt";
            _fileSystem!.WriteAllBytes(path, Array.Empty<byte>());

            // Act
            var result = _fileSystem.ReadAllBytes(path);

            // Assert
            await Assert.That(result).IsNotNull();
            await Assert.That(result.Length).IsEqualTo(0);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task ReadAllBytes_AfterOverwrite_ReturnsNewData()
    {
        SetUp();
        try
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
            await Assert.That(result).IsEqualTo(newData);
            await Assert.That(result).IsNotEqualTo(originalData);
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region WriteAllBytes Tests

    [Test]
    public async Task WriteAllBytes_NewFile_CreatesFile()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/newfile.txt";
            var data = Encoding.UTF8.GetBytes("new content");

            // Act
            _fileSystem!.WriteAllBytes(path, data);

            // Assert
            await Assert.That(_fileSystem.Exists(path)).IsTrue();
            var result = _fileSystem.ReadAllBytes(path);
            await Assert.That(result).IsEqualTo(data);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_ExistingFile_OverwritesFile()
    {
        SetUp();
        try
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
            await Assert.That(result).IsEqualTo(newData);
            await Assert.That(result).IsNotEqualTo(originalData);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_NullPath_ThrowsArgumentNullException()
    {
        SetUp();
        try
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("test");

            // Act & Assert
            await Assert.That(() => _fileSystem!.WriteAllBytes(null!, data)).Throws<ArgumentNullException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_EmptyPath_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("test");

            // Act & Assert
            await Assert.That(() => _fileSystem!.WriteAllBytes(string.Empty, data)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_NullData_ThrowsArgumentNullException()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/file.txt";

            // Act & Assert
            await Assert.That(() => _fileSystem!.WriteAllBytes(path, null!)).Throws<ArgumentNullException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_LargeData_WritesCorrectly()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/largefile.bin";
            var largeData = new byte[5 * 1024 * 1024]; // 5 MB
            new Random(42).NextBytes(largeData);

            // Act
            _fileSystem!.WriteAllBytes(path, largeData);

            // Assert
            var result = _fileSystem.ReadAllBytes(path);
            await Assert.That(result).IsEqualTo(largeData);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_EmptyData_WritesEmptyFile()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/empty.txt";

            // Act
            _fileSystem!.WriteAllBytes(path, Array.Empty<byte>());

            // Assert
            await Assert.That(_fileSystem.Exists(path)).IsTrue();
            var result = _fileSystem.ReadAllBytes(path);
            await Assert.That(result).IsNotNull();
            await Assert.That(result.Length).IsEqualTo(0);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_NestedPath_CreatesFile()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "level1/level2/level3/file.txt";
            var data = Encoding.UTF8.GetBytes("nested content");

            // Act
            _fileSystem!.WriteAllBytes(path, data);

            // Assert
            await Assert.That(_fileSystem.Exists(path)).IsTrue();
            var result = _fileSystem.ReadAllBytes(path);
            await Assert.That(result).IsEqualTo(data);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_PathWithBackslashes_NormalizesPath()
    {
        SetUp();
        try
        {
            // Arrange
            var path1 = "test\\file.txt";
            var path2 = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");

            // Act
            _fileSystem!.WriteAllBytes(path1, data);

            // Assert - Both paths should refer to the same file
            await Assert.That(_fileSystem.Exists(path1)).IsTrue();
            await Assert.That(_fileSystem.Exists(path2)).IsTrue();
            var result1 = _fileSystem.ReadAllBytes(path1);
            var result2 = _fileSystem.ReadAllBytes(path2);
            await Assert.That(result1).IsEqualTo(result2);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task WriteAllBytes_PathWithDotSegments_NormalizesPath()
    {
        SetUp();
        try
        {
            // Arrange
            var path1 = "test/../test/file.txt";
            var path2 = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");

            // Act
            _fileSystem!.WriteAllBytes(path1, data);

            // Assert - Both paths should refer to the same file
            await Assert.That(_fileSystem.Exists(path2)).IsTrue();
            var result = _fileSystem.ReadAllBytes(path2);
            await Assert.That(result).IsEqualTo(data);
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region Delete Tests

    [Test]
    public async Task Delete_ExistingFile_RemovesFile()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");
            _fileSystem!.WriteAllBytes(path, data);
            await Assert.That(_fileSystem.Exists(path)).IsTrue();

            // Act
            _fileSystem.Delete(path);

            // Assert
            await Assert.That(_fileSystem.Exists(path)).IsFalse();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Delete_NonExistentFile_ThrowsFileNotFoundException()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "nonexistent/file.txt";

            // Act & Assert
            var ex = await Assert.That(() => _fileSystem!.Delete(path)).Throws<FileNotFoundException>();
            await Assert.That(ex.Message.Contains(path)).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Delete_NullPath_ThrowsArgumentNullException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _fileSystem!.Delete(null!)).Throws<ArgumentNullException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Delete_EmptyPath_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _fileSystem!.Delete(string.Empty)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Delete_AfterDelete_CannotReadFile()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");
            _fileSystem!.WriteAllBytes(path, data);
            _fileSystem.Delete(path);

            // Act & Assert
            await Assert.That(() => _fileSystem.ReadAllBytes(path)).Throws<FileNotFoundException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Delete_MultipleFiles_DeletesCorrectly()
    {
        SetUp();
        try
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
            await Assert.That(_fileSystem.Exists(path1)).IsFalse();
            await Assert.That(_fileSystem.Exists(path2)).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region Integration Tests

    [Test]
    public async Task Integration_WriteReadDelete_WorksCorrectly()
    {
        SetUp();
        try
        {
            // Arrange
            var path = "integration/test.txt";
            var data = Encoding.UTF8.GetBytes("integration test content");

            // Act & Assert - Write
            _fileSystem!.WriteAllBytes(path, data);
            await Assert.That(_fileSystem.Exists(path)).IsTrue();

            // Act & Assert - Read
            var readData = _fileSystem.ReadAllBytes(path);
            await Assert.That(readData).IsEqualTo(data);

            // Act & Assert - Delete
            _fileSystem.Delete(path);
            await Assert.That(_fileSystem.Exists(path)).IsFalse();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Integration_MultipleFiles_IndependentOperations()
    {
        SetUp();
        try
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
            await Assert.That(_fileSystem.Exists(path1)).IsTrue();
            await Assert.That(_fileSystem.Exists(path2)).IsTrue();
            await Assert.That(_fileSystem.Exists(path3)).IsTrue();

            // Act - Delete middle file
            _fileSystem.Delete(path2);

            // Assert - Other files still exist
            await Assert.That(_fileSystem.Exists(path1)).IsTrue();
            await Assert.That(_fileSystem.Exists(path2)).IsFalse();
            await Assert.That(_fileSystem.Exists(path3)).IsTrue();

            // Assert - Can still read other files
            await Assert.That(_fileSystem.ReadAllBytes(path1)).IsEqualTo(data1);
            await Assert.That(_fileSystem.ReadAllBytes(path3)).IsEqualTo(data3);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Integration_OverwriteMultipleTimes_LastWriteWins()
    {
        SetUp();
        try
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
            await Assert.That(result).IsEqualTo(data3);
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region In-Memory Database Tests

    [Test]
    public async Task InMemoryDatabase_WorksCorrectly()
    {
        // Arrange
        using var inMemoryFileSystem = new LiteDbFileSystem(null);
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("in-memory content");

        // Act
        inMemoryFileSystem.WriteAllBytes(path, data);

        // Assert
        await Assert.That(inMemoryFileSystem.Exists(path)).IsTrue();
        var result = inMemoryFileSystem.ReadAllBytes(path);
        await Assert.That(result).IsEqualTo(data);
    }

    [Test]
    public async Task InMemoryDatabase_EmptyPath_WorksCorrectly()
    {
        // Arrange
        using var inMemoryFileSystem = new LiteDbFileSystem(string.Empty);
        var path = "test/file.txt";
        var data = Encoding.UTF8.GetBytes("content");

        // Act
        inMemoryFileSystem.WriteAllBytes(path, data);

        // Assert
        await Assert.That(inMemoryFileSystem.Exists(path)).IsTrue();
    }

    #endregion

    #region Path Normalization Tests

    [Test]
    public async Task PathNormalization_RemovesLeadingSlashes()
    {
        SetUp();
        try
        {
            // Arrange
            var path1 = "/test/file.txt";
            var path2 = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");

            // Act
            _fileSystem!.WriteAllBytes(path1, data);

            // Assert - Both paths should refer to the same file
            await Assert.That(_fileSystem.Exists(path2)).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task PathNormalization_RemovesTrailingSlashes()
    {
        SetUp();
        try
        {
            // Arrange
            var path1 = "test/file.txt/";
            var path2 = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");

            // Act
            _fileSystem!.WriteAllBytes(path1, data);

            // Assert - Both paths should refer to the same file
            await Assert.That(_fileSystem.Exists(path2)).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task PathNormalization_PreventsDirectoryTraversal()
    {
        SetUp();
        try
        {
            // Arrange
            var path1 = "../../../etc/passwd";
            var path2 = "etc/passwd";
            var data = Encoding.UTF8.GetBytes("content");

            // Act
            _fileSystem!.WriteAllBytes(path1, data);

            // Assert - Path should be normalized, preventing directory traversal
            // Both paths should refer to the same normalized file
            await Assert.That(_fileSystem.Exists(path2)).IsTrue();
            // The normalized path should also work (path normalization makes both equivalent)
            var result1 = _fileSystem.ReadAllBytes(path2);
            await Assert.That(result1).IsEqualTo(data);
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region Disposal Tests

    [Test]
    public async Task Dispose_CanBeCalledMultipleTimes()
    {
        SetUp();
        try
        {
            // Arrange
            var artifactsDir = GetArtifactsDirectoryInfo();
            var testDbPath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
            var fileSystem = new LiteDbFileSystem(testDbPath);
            var path = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");
            fileSystem.WriteAllBytes(path, data);

            // Act & Assert - Should not throw
            fileSystem.Dispose();
            fileSystem.Dispose();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Dispose_AfterDisposal_OperationsFail()
    {
        SetUp();
        try
        {
            // Arrange
            var artifactsDir = GetArtifactsDirectoryInfo();
            var testDbPath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
            var fileSystem = new LiteDbFileSystem(testDbPath);
            var path = "test/file.txt";
            var data = Encoding.UTF8.GetBytes("content");
            fileSystem.WriteAllBytes(path, data);
            fileSystem.Dispose();

            // Act & Assert - Operations should fail after disposal
            // LiteDB throws ObjectDisposedException when database is disposed
            await Assert.That(() => fileSystem.Exists(path)).Throws<ObjectDisposedException>();
            await Assert.That(() => fileSystem.ReadAllBytes(path)).Throws<ObjectDisposedException>();
            await Assert.That(() => fileSystem.WriteAllBytes(path, data)).Throws<ObjectDisposedException>();
            await Assert.That(() => fileSystem.Delete(path)).Throws<ObjectDisposedException>();
        }
        finally
        {
            TearDown();
        }
    }

    #endregion
}
