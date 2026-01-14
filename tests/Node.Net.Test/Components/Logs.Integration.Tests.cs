#if !IS_FRAMEWORK
extern alias NodeNet;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.IO;
using NodeNet::Node.Net.Components;
using NodeNet::Node.Net.Diagnostic;
using NodeNet::Node.Net.Service.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Test.Components;

/// <summary>
/// Integration tests for Logs component with example applications.
/// </summary>
[TestFixture]
[Ignore("JSInterop setup for Fluent UI components needs refinement. Skipping for now.")]
internal class LogsIntegrationTests : TestHarness
{
    public LogsIntegrationTests() : base("LogsIntegration")
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

    [Test]
    public void Component_WithLogService_DisplaysLogs()
    {
        // Arrange - Create some log entries
        _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Integration test message",
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService);

        // Act
        var cut = ctx.RenderComponent<Logs>();

        // Assert
        Assert.That(cut, Is.Not.Null);
        Assert.That(cut.Markup, Does.Contain("Integration test message"));
    }

    [Test]
    public void Component_WithMultipleEntries_DisplaysAll()
    {
        // Arrange - Create multiple log entries
        for (int i = 0; i < 5; i++)
        {
            _logService!.Create(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow.AddSeconds(-i),
                Level = i % 2 == 0 ? "Info" : "Error",
                Message = $"Message {i}",
                IsManualEntry = false
            });
        }

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService);
        FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

        // Act
        var cut = ctx.RenderComponent<Logs>();

        // Assert
        Assert.That(cut, Is.Not.Null);
        for (int i = 0; i < 5; i++)
        {
            Assert.That(cut.Markup, Does.Contain($"Message {i}"));
        }
    }
}
#endif
