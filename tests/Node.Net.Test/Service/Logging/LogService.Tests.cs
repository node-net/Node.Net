#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Node.Net.Diagnostic;
using Node.Net.Service.Logging;

namespace Node.Net.Test.Service.Logging;

/// <summary>
/// Unit tests for LogService implementation of ILogService.
/// </summary>
internal class LogServiceTests : TestHarness
{
    public LogServiceTests() : base("LogService")
    {
    }

    private LogService? _logService;
    private string? _testDatabasePath;

    private void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _logService = new LogService(_testDatabasePath);
    }

    private void TearDown()
    {
        // Dispose the log service
        _logService?.Dispose();
        _logService = null;

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

    #region Create Tests

    [Test]
    public async Task Create_ValidEntry_ReturnsEntryWithId()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Test message",
                IsManualEntry = true
            };

            // Act
            var result = _logService!.Create(entry);

            // Assert
            await Assert.That(result).IsNotNull();
            await Assert.That(result.Id).IsNotEqualTo(ObjectId.Empty);
            await Assert.That(result.Message).IsEqualTo("Test message");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Create_NullEntry_ThrowsArgumentNullException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _logService!.Create(null!)).Throws<ArgumentNullException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Create_InvalidEntry_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Invalid",
                Message = "Test",
                IsManualEntry = false
            };

            // Act & Assert
            await Assert.That(() => _logService!.Create(entry)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Create_WithStructuredProperties_StoresProperties()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Test message",
                Properties = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "UserId", 12345 },
                    { "RequestId", "abc-123" }
                },
                IsManualEntry = false
            };

            // Act
            var result = _logService!.Create(entry);

            // Assert
            await Assert.That(result.Properties).IsNotNull();
            await Assert.That(result.Properties!.Count).IsEqualTo(2);
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region GetById Tests

    [Test]
    public async Task GetById_ExistingEntry_ReturnsEntry()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Test message",
                IsManualEntry = false
            };
            var created = _logService!.Create(entry);

            // Act
            var result = _logService.GetById(created.Id);

            // Assert
            await Assert.That(result).IsNotNull();
            await Assert.That(result!.Id).IsEqualTo(created.Id);
            await Assert.That(result.Message).IsEqualTo("Test message");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task GetById_NonExistentEntry_ReturnsNull()
    {
        SetUp();
        try
        {
            // Arrange
            var nonExistentId = ObjectId.NewObjectId();

            // Act
            var result = _logService!.GetById(nonExistentId);

            // Assert
            await Assert.That(result).IsNull();
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region Update Tests

    [Test]
    public async Task Update_ManualEntry_UpdatesSuccessfully()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Original message",
                IsManualEntry = true
            };
            var created = _logService!.Create(entry);
            created.Message = "Updated message";

            // Act
            _logService.Update(created);

            // Assert
            var updated = _logService.GetById(created.Id);
            await Assert.That(updated).IsNotNull();
            await Assert.That(updated!.Message).IsEqualTo("Updated message");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Update_AutomaticEntry_ThrowsInvalidOperationException()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Automatic entry",
                IsManualEntry = false
            };
            var created = _logService!.Create(entry);
            created.Message = "Attempted update";

            // Act & Assert
            var ex = await Assert.That(() => _logService.Update(created)).Throws<InvalidOperationException>();
            await Assert.That(ex.Message.Contains("automatically captured")).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Update_NonExistentEntry_ThrowsKeyNotFoundException()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Id = ObjectId.NewObjectId(),
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Non-existent",
                IsManualEntry = true
            };

            // Act & Assert
            await Assert.That(() => _logService!.Update(entry)).Throws<KeyNotFoundException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Update_NullEntry_ThrowsArgumentNullException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _logService!.Update(null!)).Throws<ArgumentNullException>();
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region Delete Tests

    [Test]
    public async Task Delete_ExistingEntry_ReturnsTrue()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "To be deleted",
                IsManualEntry = false
            };
            var created = _logService!.Create(entry);

            // Act
            var result = _logService.Delete(created.Id);

            // Assert
            await Assert.That(result).IsTrue();
            await Assert.That(_logService.GetById(created.Id)).IsNull();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Delete_NonExistentEntry_ReturnsFalse()
    {
        SetUp();
        try
        {
            // Arrange
            var nonExistentId = ObjectId.NewObjectId();

            // Act
            var result = _logService!.Delete(nonExistentId);

            // Assert
            await Assert.That(result).IsFalse();
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region Search Tests

    [Test]
    public async Task Search_NoFilters_ReturnsAllEntries()
    {
        SetUp();
        try
        {
            // Arrange
            _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Message 1", IsManualEntry = false });
            _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Message 2", IsManualEntry = false });

            // Act
            var results = _logService.Search().ToList();

            // Assert
            await Assert.That(results.Count).IsEqualTo(2);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Search_WithSearchTerm_FiltersByMessage()
    {
        SetUp();
        try
        {
            // Arrange
            _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Error occurred", IsManualEntry = false });
            _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Success", IsManualEntry = false });

            // Act
            var results = _logService.Search(searchTerm: "Error").ToList();

            // Assert
            await Assert.That(results.Count).IsEqualTo(1);
            await Assert.That(results[0].Message.Contains("Error")).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Search_WithLevelFilter_FiltersByLevel()
    {
        SetUp();
        try
        {
            // Arrange
            _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Info message", IsManualEntry = false });
            _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Error message", IsManualEntry = false });

            // Act
            var results = _logService.Search(levelFilter: "Error").ToList();

            // Assert
            await Assert.That(results.Count).IsEqualTo(1);
            await Assert.That(results[0].Level).IsEqualTo("Error");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Search_WithPagination_ReturnsCorrectPage()
    {
        SetUp();
        try
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                _logService!.Create(new LogEntry
                {
                    Timestamp = DateTimeOffset.UtcNow.AddSeconds(-i),
                    Level = "Info",
                    Message = $"Message {i}",
                    IsManualEntry = false
                });
            }

            // Act
            var results = _logService!.Search(skip: 2, take: 3).ToList();

            // Assert
            await Assert.That(results.Count).IsEqualTo(3);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Search_WithOrderByOldestFirst_ReturnsOldestFirst()
    {
        SetUp();
        try
        {
            // Arrange
            var now = DateTimeOffset.UtcNow;
            _logService!.Create(new LogEntry { Timestamp = now.AddSeconds(-2), Level = "Info", Message = "Second", IsManualEntry = false });
            _logService.Create(new LogEntry { Timestamp = now.AddSeconds(-1), Level = "Info", Message = "First", IsManualEntry = false });

            // Act
            var results = _logService.Search(orderByNewestFirst: false).ToList();

            // Assert
            await Assert.That(results.Count).IsEqualTo(2);
            await Assert.That(results[0].Message).IsEqualTo("Second"); // Oldest first
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Search_WithSearchTermInProperties_FindsEntry()
    {
        SetUp();
        try
        {
            // Arrange
            var entry = new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Test",
                Properties = new System.Collections.Generic.Dictionary<string, object> { { "UserId", "12345" } },
                IsManualEntry = false
            };
            _logService!.Create(entry);

            // Act
            var results = _logService.Search(searchTerm: "12345").ToList();

            // Assert
            await Assert.That(results.Count).IsEqualTo(1);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Search_InvalidSkip_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _logService!.Search(skip: -1)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Search_InvalidTake_ThrowsArgumentException()
    {
        SetUp();
        try
        {
            // Act & Assert
            await Assert.That(() => _logService!.Search(take: 0)).Throws<ArgumentException>();
            await Assert.That(() => _logService!.Search(take: 1001)).Throws<ArgumentException>();
        }
        finally
        {
            TearDown();
        }
    }

    #endregion

    #region GetCount Tests

    [Test]
    public async Task GetCount_NoFilters_ReturnsTotalCount()
    {
        SetUp();
        try
        {
            // Arrange
            _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Message 1", IsManualEntry = false });
            _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Message 2", IsManualEntry = false });

            // Act
            var count = _logService.GetCount();

            // Assert
            await Assert.That(count).IsEqualTo(2);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task GetCount_WithSearchTerm_ReturnsFilteredCount()
    {
        SetUp();
        try
        {
            // Arrange
            _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Error occurred", IsManualEntry = false });
            _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Success", IsManualEntry = false });

            // Act
            var count = _logService.GetCount(searchTerm: "Error");

            // Assert
            await Assert.That(count).IsEqualTo(1);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task GetCount_WithLevelFilter_ReturnsFilteredCount()
    {
        SetUp();
        try
        {
            // Arrange
            _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Info message", IsManualEntry = false });
            _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Error message", IsManualEntry = false });

            // Act
            var count = _logService.GetCount(levelFilter: "Error");

            // Assert
            await Assert.That(count).IsEqualTo(1);
        }
        finally
        {
            TearDown();
        }
    }

    #endregion
}
