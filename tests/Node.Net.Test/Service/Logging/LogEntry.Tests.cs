using NUnit.Framework;
using System;
using Node.Net.Service.Logging;

namespace Node.Net.Service.Logging;

[TestFixture]
internal class LogEntryTests
{
    [Test]
    public void Validate_WithValidEntry_DoesNotThrow()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert
        Assert.DoesNotThrow(() => entry.Validate());
    }

    [Test]
    public void Validate_WithFutureTimestamp_ThrowsArgumentException()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow.AddHours(1),
            Level = "Info",
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => entry.Validate());
        Assert.That(ex?.ParamName, Is.EqualTo("Timestamp"));
    }

    [Test]
    public void Validate_WithNullLevel_ThrowsArgumentException()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = null!,
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => entry.Validate());
        Assert.That(ex?.ParamName, Is.EqualTo("Level"));
    }

    [Test]
    public void Validate_WithEmptyLevel_ThrowsArgumentException()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = string.Empty,
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => entry.Validate());
        Assert.That(ex?.ParamName, Is.EqualTo("Level"));
    }

    [Test]
    public void Validate_WithInvalidLevel_ThrowsArgumentException()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Invalid",
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => entry.Validate());
        Assert.That(ex?.ParamName, Is.EqualTo("Level"));
    }

    [Test]
    [TestCase("Debug")]
    [TestCase("Info")]
    [TestCase("Warn")]
    [TestCase("Error")]
    [TestCase("Fatal")]
    [TestCase("debug")] // Case-insensitive
    [TestCase("INFO")] // Case-insensitive
    public void Validate_WithValidLevel_DoesNotThrow(string level)
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = level,
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert
        Assert.DoesNotThrow(() => entry.Validate());
    }

    [Test]
    public void Validate_WithNullMessage_ThrowsArgumentException()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = null!,
            IsManualEntry = false
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => entry.Validate());
        Assert.That(ex?.ParamName, Is.EqualTo("Message"));
    }

    [Test]
    public void Validate_WithEmptyMessage_ThrowsArgumentException()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = string.Empty,
            IsManualEntry = false
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => entry.Validate());
        Assert.That(ex?.ParamName, Is.EqualTo("Message"));
    }

    [Test]
    public void Properties_CanStoreStructuredData()
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
                { "RequestId", "abc-123" },
                { "Duration", 42.5 }
            },
            IsManualEntry = false
        };

        // Act & Assert
        Assert.That(entry.Properties, Is.Not.Null);
        Assert.That(entry.Properties.Count, Is.EqualTo(3));
        Assert.That(entry.Properties["UserId"], Is.EqualTo(12345));
    }
}
