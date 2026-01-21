#nullable enable
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using MELogLevel = Microsoft.Extensions.Logging.LogLevel;
using Node.Net.Diagnostic;
using Node.Net.Service.Logging;

namespace Node.Net.Test.Service.Logging;

/// <summary>
/// Integration tests for LogServiceLoggerProvider with Microsoft.Extensions.Logging.
/// </summary>
[TestFixture]
internal class LogServiceIntegrationTests : TestHarness
{
    public LogServiceIntegrationTests() : base("LogServiceIntegration")
    {
    }

    private LogService? _logService;
    private LogServiceLoggerProvider? _loggerProvider;
    private string? _testDatabasePath;

    [SetUp]
    public void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _logService = new LogService(_testDatabasePath);
        _loggerProvider = new LogServiceLoggerProvider(_logService);
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose resources
        _loggerProvider?.Dispose();
        _logService?.Dispose();
        _logService = null;
        _loggerProvider = null;

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

    [Test]
    public void Log_InfoMessage_CapturesLogEntry()
    {
        // Arrange
        var logger = _loggerProvider!.CreateLogger("TestCategory");

        // Act
        logger.LogInformation("Test information message");

        // Assert
        var entries = _logService!.Search().ToList();
        Assert.That(entries.Count, Is.EqualTo(1));
        Assert.That(entries[0].Level, Is.EqualTo("Info"));
        Assert.That(entries[0].Message, Does.Contain("Test information message"));
        Assert.That(entries[0].IsManualEntry, Is.False);
        Assert.That(entries[0].SourceContext, Is.EqualTo("TestCategory"));
    }

    [Test]
    public void Log_ErrorMessage_CapturesLogEntry()
    {
        // Arrange
        var logger = _loggerProvider!.CreateLogger("TestCategory");

        // Act
        logger.LogError("Test error message");

        // Assert
        var entries = _logService!.Search().ToList();
        Assert.That(entries.Count, Is.EqualTo(1));
        Assert.That(entries[0].Level, Is.EqualTo("Error"));
        Assert.That(entries[0].Message, Does.Contain("Test error message"));
    }

    [Test]
    public void Log_WithException_CapturesException()
    {
        // Arrange
        var logger = _loggerProvider!.CreateLogger("TestCategory");
        var exception = new InvalidOperationException("Test exception");

        // Act
        logger.LogError(exception, "Error occurred");

        // Assert
        var entries = _logService!.Search().ToList();
        Assert.That(entries.Count, Is.EqualTo(1));
        Assert.That(entries[0].Exception, Is.Not.Null);
        Assert.That(entries[0].Exception, Does.Contain("InvalidOperationException"));
    }

    [Test]
    public void Log_WithStructuredProperties_CapturesProperties()
    {
        // Arrange
        var logger = _loggerProvider!.CreateLogger("TestCategory");

        // Act
        logger.LogInformation("User {UserId} performed action {Action}", 12345, "Login");

        // Assert
        var entries = _logService!.Search().ToList();
        Assert.That(entries.Count, Is.EqualTo(1));
        Assert.That(entries[0].Properties, Is.Not.Null);
        Assert.That(entries[0].Properties!.ContainsKey("UserId"), Is.True);
        Assert.That(entries[0].Properties["UserId"], Is.EqualTo(12345));
        Assert.That(entries[0].Properties.ContainsKey("Action"), Is.True);
        Assert.That(entries[0].Properties["Action"], Is.EqualTo("Login"));
    }

    [Test]
    public void Log_WithMessageTemplate_CapturesTemplate()
    {
        // Arrange
        var logger = _loggerProvider!.CreateLogger("TestCategory");

        // Act
        logger.LogInformation("User {UserId} performed action", 12345);

        // Assert
        var entries = _logService!.Search().ToList();
        Assert.That(entries.Count, Is.EqualTo(1));
        Assert.That(entries[0].MessageTemplate, Is.EqualTo("User {UserId} performed action"));
    }

    [Test]
    public void Log_AllLevels_CapturesCorrectly()
    {
        // Arrange
        var logger = _loggerProvider!.CreateLogger("TestCategory");

        // Act
        logger.LogTrace("Trace message");
        logger.LogDebug("Debug message");
        logger.LogInformation("Info message");
        logger.LogWarning("Warning message");
        logger.LogError("Error message");
        logger.LogCritical("Critical message");

        // Assert
        var entries = _logService!.Search().ToList();
        Assert.That(entries.Count, Is.EqualTo(6));
        Assert.That(entries.Any(e => e.Level == "Debug" && e.Message.Contains("Trace")), Is.True);
        Assert.That(entries.Any(e => e.Level == "Debug" && e.Message.Contains("Debug")), Is.True);
        Assert.That(entries.Any(e => e.Level == "Info" && e.Message.Contains("Info")), Is.True);
        Assert.That(entries.Any(e => e.Level == "Warn" && e.Message.Contains("Warning")), Is.True);
        Assert.That(entries.Any(e => e.Level == "Error" && e.Message.Contains("Error")), Is.True);
        Assert.That(entries.Any(e => e.Level == "Fatal" && e.Message.Contains("Critical")), Is.True);
    }

    [Test]
    public void Log_MultipleCategories_CreatesSeparateLoggers()
    {
        // Arrange
        var logger1 = _loggerProvider!.CreateLogger("Category1");
        var logger2 = _loggerProvider!.CreateLogger("Category2");

        // Act
        logger1.LogInformation("Message from category 1");
        logger2.LogInformation("Message from category 2");

        // Assert
        var entries = _logService!.Search().ToList();
        Assert.That(entries.Count, Is.EqualTo(2));
        Assert.That(entries.Any(e => e.SourceContext == "Category1"), Is.True);
        Assert.That(entries.Any(e => e.SourceContext == "Category2"), Is.True);
    }

    [Test]
    public void Log_IsEnabled_ReturnsTrue()
    {
        // Arrange
        var logger = _loggerProvider!.CreateLogger("TestCategory");

        // Act & Assert
        Assert.That(logger.IsEnabled(MELogLevel.Trace), Is.True);
        Assert.That(logger.IsEnabled(MELogLevel.Debug), Is.True);
        Assert.That(logger.IsEnabled(MELogLevel.Information), Is.True);
        Assert.That(logger.IsEnabled(MELogLevel.Warning), Is.True);
        Assert.That(logger.IsEnabled(MELogLevel.Error), Is.True);
        Assert.That(logger.IsEnabled(MELogLevel.Critical), Is.True);
    }
}
