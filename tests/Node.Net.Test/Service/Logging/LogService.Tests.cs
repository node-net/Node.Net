#nullable enable
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using Node.Net.Diagnostic;
using Node.Net.Service.Logging;

namespace Node.Net.Test.Service.Logging;

/// <summary>
/// Unit tests for LogService implementation of ILogService.
/// </summary>
[TestFixture]
internal class LogServiceTests : TestHarness
{
    public LogServiceTests() : base("LogService")
    {
    }

    private LogService? _logService;
    private string? _testDatabasePath;

    [SetUp]
    public void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _logService = new LogService(_testDatabasePath);
    }

    [TearDown]
    public void TearDown()
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
    public void Create_ValidEntry_ReturnsEntryWithId()
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
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.Not.EqualTo(ObjectId.Empty));
        Assert.That(result.Message, Is.EqualTo("Test message"));
    }

    [Test]
    public void Create_NullEntry_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _logService!.Create(null!));
    }

    [Test]
    public void Create_InvalidEntry_ThrowsArgumentException()
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
        Assert.Throws<ArgumentException>(() => _logService!.Create(entry));
    }

    [Test]
    public void Create_WithStructuredProperties_StoresProperties()
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
        Assert.That(result.Properties, Is.Not.Null);
        Assert.That(result.Properties!.Count, Is.EqualTo(2));
    }

    #endregion

    #region GetById Tests

    [Test]
    public void GetById_ExistingEntry_ReturnsEntry()
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
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(created.Id));
        Assert.That(result.Message, Is.EqualTo("Test message"));
    }

    [Test]
    public void GetById_NonExistentEntry_ReturnsNull()
    {
        // Arrange
        var nonExistentId = ObjectId.NewObjectId();

        // Act
        var result = _logService!.GetById(nonExistentId);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region Update Tests

    [Test]
    public void Update_ManualEntry_UpdatesSuccessfully()
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
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Message, Is.EqualTo("Updated message"));
    }

    [Test]
    public void Update_AutomaticEntry_ThrowsInvalidOperationException()
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
        var ex = Assert.Throws<InvalidOperationException>(() => _logService.Update(created));
        Assert.That(ex?.Message, Does.Contain("automatically captured"));
    }

    [Test]
    public void Update_NonExistentEntry_ThrowsKeyNotFoundException()
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
        Assert.Throws<KeyNotFoundException>(() => _logService!.Update(entry));
    }

    [Test]
    public void Update_NullEntry_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _logService!.Update(null!));
    }

    #endregion

    #region Delete Tests

    [Test]
    public void Delete_ExistingEntry_ReturnsTrue()
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
        Assert.That(result, Is.True);
        Assert.That(_logService.GetById(created.Id), Is.Null);
    }

    [Test]
    public void Delete_NonExistentEntry_ReturnsFalse()
    {
        // Arrange
        var nonExistentId = ObjectId.NewObjectId();

        // Act
        var result = _logService!.Delete(nonExistentId);

        // Assert
        Assert.That(result, Is.False);
    }

    #endregion

    #region Search Tests

    [Test]
    public void Search_NoFilters_ReturnsAllEntries()
    {
        // Arrange
        _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Message 1", IsManualEntry = false });
        _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Message 2", IsManualEntry = false });

        // Act
        var results = _logService.Search().ToList();

        // Assert
        Assert.That(results.Count, Is.EqualTo(2));
    }

    [Test]
    public void Search_WithSearchTerm_FiltersByMessage()
    {
        // Arrange
        _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Error occurred", IsManualEntry = false });
        _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Success", IsManualEntry = false });

        // Act
        var results = _logService.Search(searchTerm: "Error").ToList();

        // Assert
        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results[0].Message, Does.Contain("Error"));
    }

    [Test]
    public void Search_WithLevelFilter_FiltersByLevel()
    {
        // Arrange
        _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Info message", IsManualEntry = false });
        _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Error message", IsManualEntry = false });

        // Act
        var results = _logService.Search(levelFilter: "Error").ToList();

        // Assert
        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results[0].Level, Is.EqualTo("Error"));
    }

    [Test]
    public void Search_WithPagination_ReturnsCorrectPage()
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
        Assert.That(results.Count, Is.EqualTo(3));
    }

    [Test]
    public void Search_WithOrderByOldestFirst_ReturnsOldestFirst()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        _logService!.Create(new LogEntry { Timestamp = now.AddSeconds(-2), Level = "Info", Message = "Second", IsManualEntry = false });
        _logService.Create(new LogEntry { Timestamp = now.AddSeconds(-1), Level = "Info", Message = "First", IsManualEntry = false });

        // Act
        var results = _logService.Search(orderByNewestFirst: false).ToList();

        // Assert
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results[0].Message, Is.EqualTo("Second")); // Oldest first
    }

    [Test]
    public void Search_WithSearchTermInProperties_FindsEntry()
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
        Assert.That(results.Count, Is.EqualTo(1));
    }

    [Test]
    public void Search_InvalidSkip_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _logService!.Search(skip: -1));
    }

    [Test]
    public void Search_InvalidTake_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _logService!.Search(take: 0));
        Assert.Throws<ArgumentException>(() => _logService!.Search(take: 1001));
    }

    #endregion

    #region GetCount Tests

    [Test]
    public void GetCount_NoFilters_ReturnsTotalCount()
    {
        // Arrange
        _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Message 1", IsManualEntry = false });
        _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Message 2", IsManualEntry = false });

        // Act
        var count = _logService.GetCount();

        // Assert
        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public void GetCount_WithSearchTerm_ReturnsFilteredCount()
    {
        // Arrange
        _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Error occurred", IsManualEntry = false });
        _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Success", IsManualEntry = false });

        // Act
        var count = _logService.GetCount(searchTerm: "Error");

        // Assert
        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void GetCount_WithLevelFilter_ReturnsFilteredCount()
    {
        // Arrange
        _logService!.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Info", Message = "Info message", IsManualEntry = false });
        _logService.Create(new LogEntry { Timestamp = DateTimeOffset.UtcNow, Level = "Error", Message = "Error message", IsManualEntry = false });

        // Act
        var count = _logService.GetCount(levelFilter: "Error");

        // Assert
        Assert.That(count, Is.EqualTo(1));
    }

    #endregion
}
