#if !IS_FRAMEWORK
#nullable enable
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TUnit;
using System;
using System.IO;
using Node.Net.Components;
using Node.Net.Diagnostic;
using Node.Net.Service.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Components.Test;

/// <summary>
/// Integration tests for Logs component with example applications.
/// </summary>
// TODO: JSInterop setup for Fluent UI components needs refinement. Skipping for now.
internal class LogsIntegrationTests : TestHarness
{
    public LogsIntegrationTests() : base("LogsIntegration")
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

    [Test]
    public async Task Component_WithLogService_DisplaysLogs()
    {
        SetUp();
        try
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
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            // Act
            var cut = ctx.RenderComponent<Logs>();

            // Assert
            await Assert.That(cut).IsNotNull();
            await Assert.That(cut.Markup).Contains("Integration test message");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Component_WithMultipleEntries_DisplaysAll()
    {
        SetUp();
        try
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
            ctx.Services.AddSingleton<ILogService>(_logService!);
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            // Act
            var cut = ctx.RenderComponent<Logs>();

            // Assert
            await Assert.That(cut).IsNotNull();
            for (int i = 0; i < 5; i++)
            {
                await Assert.That(cut.Markup).Contains($"Message {i}");
            }
        }
        finally
        {
            TearDown();
        }
    }
}
#endif
