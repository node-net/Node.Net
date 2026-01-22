using System;
using System.Threading.Tasks;
using Node.Net.Service.Logging;

namespace Node.Net.Service.Logging;

internal class LogEntryTests
{
    [Test]
    public async Task Validate_WithValidEntry_DoesNotThrow()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert - TUnit doesn't have Assert.DoesNotThrow, just call the method
        entry.Validate();
        await Task.CompletedTask;
    }

    [Test]
    public async Task Validate_WithFutureTimestamp_ThrowsArgumentException()
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
        var ex = await Assert.That(() => entry.Validate()).Throws<ArgumentException>();
        await Assert.That(ex.ParamName).IsEqualTo("Timestamp");
    }

    [Test]
    public async Task Validate_WithNullLevel_ThrowsArgumentException()
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
        var ex = await Assert.That(() => entry.Validate()).Throws<ArgumentException>();
        await Assert.That(ex.ParamName).IsEqualTo("Level");
    }

    [Test]
    public async Task Validate_WithEmptyLevel_ThrowsArgumentException()
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
        var ex = await Assert.That(() => entry.Validate()).Throws<ArgumentException>();
        await Assert.That(ex.ParamName).IsEqualTo("Level");
    }

    [Test]
    public async Task Validate_WithInvalidLevel_ThrowsArgumentException()
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
        var ex = await Assert.That(() => entry.Validate()).Throws<ArgumentException>();
        await Assert.That(ex.ParamName).IsEqualTo("Level");
    }

    [Test]
    [Arguments("Debug")]
    [Arguments("Info")]
    [Arguments("Warn")]
    [Arguments("Error")]
    [Arguments("Fatal")]
    [Arguments("debug")] // Case-insensitive
    [Arguments("INFO")] // Case-insensitive
    public async Task Validate_WithValidLevel_DoesNotThrow(string level)
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = level,
            Message = "Test message",
            IsManualEntry = false
        };

        // Act & Assert - TUnit doesn't have Assert.DoesNotThrow, just call the method
        entry.Validate();
        await Task.CompletedTask;
    }

    [Test]
    public async Task Validate_WithNullMessage_ThrowsArgumentException()
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
        var ex = await Assert.That(() => entry.Validate()).Throws<ArgumentException>();
        await Assert.That(ex.ParamName).IsEqualTo("Message");
    }

    [Test]
    public async Task Validate_WithEmptyMessage_ThrowsArgumentException()
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
        var ex = await Assert.That(() => entry.Validate()).Throws<ArgumentException>();
        await Assert.That(ex.ParamName).IsEqualTo("Message");
    }

    [Test]
    public async Task Properties_CanStoreStructuredData()
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
        await Assert.That(entry.Properties).IsNotNull();
        await Assert.That(entry.Properties.Count).IsEqualTo(3);
        await Assert.That(entry.Properties["UserId"]).IsEqualTo(12345);
    }
}
