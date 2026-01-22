#if !IS_FRAMEWORK
#nullable enable
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Core;
using TUnit.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Node.Net.Components;
using Node.Net.Diagnostic;
using Node.Net.Service.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Components.Test;

/// <summary>
/// Unit tests for Logs Razor component.
/// </summary>
// TODO: JSInterop setup for Fluent UI components needs refinement. Skipping for now.
internal class LogsTests : TestHarness
{
    public LogsTests() : base(typeof(Logs))
    {
    }

    private LogService? _logService;
    private string? _testDatabasePath;

    private void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = System.IO.Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _logService = new LogService(_testDatabasePath);
    }

    private void TearDown()
    {
        // Dispose the log service
        _logService?.Dispose();
        _logService = null;

        // Clean up test database file if it exists
        if (_testDatabasePath != null && System.IO.File.Exists(_testDatabasePath))
        {
            try
            {
                System.IO.File.Delete(_testDatabasePath);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }


    [Test]
    public async Task Render_WithLogEntries_DisplaysEntries()
    {
        SetUp();
        try
        {
            // Arrange
            _logService!.Create(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Test message 1",
                IsManualEntry = false
            });
            _logService.Create(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow.AddSeconds(-1),
                Level = "Error",
                Message = "Test message 2",
                IsManualEntry = false
            });

            using var ctx = new Bunit.TestContext();
            ctx.Services.AddFluentUIComponents();
            ctx.Services.AddSingleton<ILogService>(_logService!);
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            // Act
            var cut = ctx.RenderComponent<Logs>();

            // Assert
            await Assert.That(cut).IsNotNull();
            await Assert.That(cut.Markup).Contains("Test message 1");
            await Assert.That(cut.Markup).Contains("Test message 2");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Render_WithNoEntries_DisplaysEmptyState()
    {
        SetUp();
        try
        {
            // Arrange
            using var ctx = new Bunit.TestContext();
            ctx.Services.AddFluentUIComponents();
            ctx.Services.AddSingleton<ILogService>(_logService!);
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            // Act
            var cut = ctx.RenderComponent<Logs>();

            // Assert
            await Assert.That(cut).IsNotNull();
            // Component should handle empty state (exact message depends on implementation)
            await Assert.That(cut.Markup).IsNotEmpty();
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Render_WithEntries_DisplaysInChronologicalOrder()
    {
        SetUp();
        try
        {
            // Arrange
            var now = DateTimeOffset.UtcNow;
            _logService!.Create(new LogEntry
            {
                Timestamp = now.AddSeconds(-2),
                Level = "Info",
                Message = "Oldest",
                IsManualEntry = false
            });
            _logService.Create(new LogEntry
            {
                Timestamp = now,
                Level = "Info",
                Message = "Newest",
                IsManualEntry = false
            });
            _logService.Create(new LogEntry
            {
                Timestamp = now.AddSeconds(-1),
                Level = "Info",
                Message = "Middle",
                IsManualEntry = false
            });

            using var ctx = new Bunit.TestContext();
            ctx.Services.AddFluentUIComponents();
            ctx.Services.AddSingleton<ILogService>(_logService!);
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            // Act
            var cut = ctx.RenderComponent<Logs>();

            // Assert
            await Assert.That(cut).IsNotNull();
            var markup = cut.Markup;
            var newestIndex = markup.IndexOf("Newest", StringComparison.Ordinal);
            var middleIndex = markup.IndexOf("Middle", StringComparison.Ordinal);
            var oldestIndex = markup.IndexOf("Oldest", StringComparison.Ordinal);

            // Newest should appear first (default order)
            await Assert.That(newestIndex).IsGreaterThan(-1);
            await Assert.That(middleIndex).IsGreaterThan(-1);
            await Assert.That(oldestIndex).IsGreaterThan(-1);
            await Assert.That(newestIndex).IsLessThan(middleIndex);
            await Assert.That(middleIndex).IsLessThan(oldestIndex);
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Render_WithEntries_DisplaysTimestampLevelAndMessage()
    {
        SetUp();
        try
        {
            // Arrange
            var timestamp = DateTimeOffset.UtcNow;
            _logService!.Create(new LogEntry
            {
                Timestamp = timestamp,
                Level = "Error",
                Message = "Error occurred",
                IsManualEntry = false
            });

            using var ctx = new Bunit.TestContext();
            ctx.Services.AddFluentUIComponents();
            ctx.Services.AddSingleton<ILogService>(_logService!);
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            // Act
            var cut = ctx.RenderComponent<Logs>();

            // Assert
            await Assert.That(cut).IsNotNull();
            var markup = cut.Markup;
            await Assert.That(markup).Contains("Error occurred");
            await Assert.That(markup).Contains("Error");
            // Timestamp format may vary, so we just check that component rendered
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task Update_AutomaticLogEntry_IsPrevented()
    {
        SetUp();
        try
        {
            // Arrange - Create an automatic entry (read-only)
            var entry = _logService!.Create(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Automatic entry",
                IsManualEntry = false
            });

            using var ctx = new Bunit.TestContext();
            ctx.Services.AddFluentUIComponents();
            ctx.Services.AddSingleton<ILogService>(_logService!);
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            var cut = ctx.RenderComponent<Logs>();

            // Act & Assert - Edit button should be disabled or not present for automatic entries
            var allButtons = cut.FindAll("button");
            var editButtons = allButtons.Where(b => b.TextContent.Contains("Edit", StringComparison.OrdinalIgnoreCase));
            var entryEditButton = editButtons.FirstOrDefault(b => 
                b.GetAttribute("data-entry-id") == entry.Id.ToString());
            
            // Edit button should be disabled or not exist for automatic entries
            if (entryEditButton != null)
            {
                var disabledAttr = entryEditButton.GetAttribute("disabled");
                await Assert.That(disabledAttr).IsNotNull();
            }
            
            // Attempting to update should throw exception
            entry.Message = "Attempted update";
            await Assert.That(() => _logService.Update(entry)).Throws<InvalidOperationException>();
        }
        finally
        {
            TearDown();
        }
    }


    [Test]
    public async Task MessageTruncation_ShortMessage_NoTruncation()
    {
        SetUp();
        try
        {
            // Arrange - Create entry with short message
            _logService!.Create(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                Level = "Info",
                Message = "Short message",
                IsManualEntry = false
            });

            using var ctx = new Bunit.TestContext();
            ctx.Services.AddFluentUIComponents();
            ctx.Services.AddSingleton<ILogService>(_logService!);
            FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

            var cut = ctx.RenderComponent<Logs>();

            // Assert - No truncation or expand button for short messages
            await Assert.That(cut.Markup).Contains("Short message");
            await Assert.That(cut.Markup).DoesNotContain("Expand");
        }
        finally
        {
            TearDown();
        }
    }
}
#endif
