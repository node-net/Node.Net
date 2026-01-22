#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MELogLevel = Microsoft.Extensions.Logging.LogLevel;
using Node.Net.Diagnostic;
using Node.Net.Service.Logging;

namespace Node.Net.Test.Service.Logging;

/// <summary>
/// Integration tests for LogServiceLoggerProvider with Microsoft.Extensions.Logging.
/// </summary>
internal class LogServiceIntegrationTests : TestHarness
{
    public LogServiceIntegrationTests() : base("LogServiceIntegration")
    {
    }

    private LogService? _logService;
    private LogServiceLoggerProvider? _loggerProvider;
    private string? _testDatabasePath;

    private void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _logService = new LogService(_testDatabasePath);
        _loggerProvider = new LogServiceLoggerProvider(_logService);
    }

    private void TearDown()
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
    public async Task Log_InfoMessage_CapturesLogEntry()
    {
        SetUp();
        try
        {
            // Arrange
            var logger = _loggerProvider!.CreateLogger("TestCategory");

            // Act
            logger.LogInformation("Test information message");

            // Assert
            var entries = _logService!.Search().ToList();
            await Assert.That(entries.Count).IsEqualTo(1);
            await Assert.That(entries[0].Level).IsEqualTo("Info");
            await Assert.That(entries[0].Message.Contains("Test information message")).IsTrue();
            await Assert.That(entries[0].IsManualEntry).IsFalse();
            await Assert.That(entries[0].SourceContext).IsEqualTo("TestCategory");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Log_ErrorMessage_CapturesLogEntry()
    {
        SetUp();
        try
        {
            // Arrange
            var logger = _loggerProvider!.CreateLogger("TestCategory");

            // Act
            logger.LogError("Test error message");

            // Assert
            var entries = _logService!.Search().ToList();
            await Assert.That(entries.Count).IsEqualTo(1);
            await Assert.That(entries[0].Level).IsEqualTo("Error");
            await Assert.That(entries[0].Message.Contains("Test error message")).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Log_WithException_CapturesException()
    {
        SetUp();
        try
        {
            // Arrange
            var logger = _loggerProvider!.CreateLogger("TestCategory");
            var exception = new InvalidOperationException("Test exception");

            // Act
            logger.LogError(exception, "Error occurred");

            // Assert
            var entries = _logService!.Search().ToList();
            await Assert.That(entries.Count).IsEqualTo(1);
            await Assert.That(entries[0].Exception).IsNotNull();
            await Assert.That(entries[0].Exception!.Contains("InvalidOperationException")).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Log_WithStructuredProperties_CapturesProperties()
    {
        SetUp();
        try
        {
            // Arrange
            var logger = _loggerProvider!.CreateLogger("TestCategory");

            // Act
            logger.LogInformation("User {UserId} performed action {Action}", 12345, "Login");

            // Assert
            var entries = _logService!.Search().ToList();
            await Assert.That(entries.Count).IsEqualTo(1);
            await Assert.That(entries[0].Properties).IsNotNull();
            await Assert.That(entries[0].Properties!.ContainsKey("UserId")).IsTrue();
            await Assert.That(entries[0].Properties!["UserId"]).IsEqualTo(12345);
            await Assert.That(entries[0].Properties!.ContainsKey("Action")).IsTrue();
            await Assert.That(entries[0].Properties!["Action"]).IsEqualTo("Login");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Log_WithMessageTemplate_CapturesTemplate()
    {
        SetUp();
        try
        {
            // Arrange
            var logger = _loggerProvider!.CreateLogger("TestCategory");

            // Act
            logger.LogInformation("User {UserId} performed action", 12345);

            // Assert
            var entries = _logService!.Search().ToList();
            await Assert.That(entries.Count).IsEqualTo(1);
            await Assert.That(entries[0].MessageTemplate).IsEqualTo("User {UserId} performed action");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Log_AllLevels_CapturesCorrectly()
    {
        SetUp();
        try
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
            await Assert.That(entries.Count).IsEqualTo(6);
            await Assert.That(entries.Any(e => e.Level == "Debug" && e.Message.Contains("Trace"))).IsTrue();
            await Assert.That(entries.Any(e => e.Level == "Debug" && e.Message.Contains("Debug"))).IsTrue();
            await Assert.That(entries.Any(e => e.Level == "Info" && e.Message.Contains("Info"))).IsTrue();
            await Assert.That(entries.Any(e => e.Level == "Warn" && e.Message.Contains("Warning"))).IsTrue();
            await Assert.That(entries.Any(e => e.Level == "Error" && e.Message.Contains("Error"))).IsTrue();
            await Assert.That(entries.Any(e => e.Level == "Fatal" && e.Message.Contains("Critical"))).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Log_MultipleCategories_CreatesSeparateLoggers()
    {
        SetUp();
        try
        {
            // Arrange
            var logger1 = _loggerProvider!.CreateLogger("Category1");
            var logger2 = _loggerProvider!.CreateLogger("Category2");

            // Act
            logger1.LogInformation("Message from category 1");
            logger2.LogInformation("Message from category 2");

            // Assert
            var entries = _logService!.Search().ToList();
            await Assert.That(entries.Count).IsEqualTo(2);
            await Assert.That(entries.Any(e => e.SourceContext == "Category1")).IsTrue();
            await Assert.That(entries.Any(e => e.SourceContext == "Category2")).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Log_IsEnabled_ReturnsTrue()
    {
        SetUp();
        try
        {
            // Arrange
            var logger = _loggerProvider!.CreateLogger("TestCategory");

            // Act & Assert
            await Assert.That(logger.IsEnabled(MELogLevel.Trace)).IsTrue();
            await Assert.That(logger.IsEnabled(MELogLevel.Debug)).IsTrue();
            await Assert.That(logger.IsEnabled(MELogLevel.Information)).IsTrue();
            await Assert.That(logger.IsEnabled(MELogLevel.Warning)).IsTrue();
            await Assert.That(logger.IsEnabled(MELogLevel.Error)).IsTrue();
            await Assert.That(logger.IsEnabled(MELogLevel.Critical)).IsTrue();
        }
        finally
        {
            TearDown();
        }
    }
}
