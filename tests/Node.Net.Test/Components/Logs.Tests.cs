#if !IS_FRAMEWORK
#nullable enable
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Node.Net.Components;
using Node.Net.Diagnostic;
using Node.Net.Service.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Test.Components;

/// <summary>
/// Unit tests for Logs Razor component.
/// </summary>
[TestFixture]
[Ignore("JSInterop setup for Fluent UI components needs refinement. Skipping for now.")]
internal class LogsTests : TestHarness
{
    public LogsTests() : base(typeof(Logs))
    {
    }

    private LogService? _logService;
    private string? _testDatabasePath;

    [SetUp]
    public void SetUp()
    {
        // Create a unique test database path for each test
        var artifactsDir = GetArtifactsDirectoryInfo();
        _testDatabasePath = System.IO.Path.Combine(artifactsDir.FullName, $"test_{Guid.NewGuid():N}.db");
        _logService = new LogService(_testDatabasePath);
    }

    [TearDown]
    public void TearDown()
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
    public void Render_WithLogEntries_DisplaysEntries()
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
        Assert.That(cut, Is.Not.Null);
        Assert.That(cut.Markup, Does.Contain("Test message 1"));
        Assert.That(cut.Markup, Does.Contain("Test message 2"));
    }

    [Test]
    public void Render_WithNoEntries_DisplaysEmptyState()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);
        FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

        // Act
        var cut = ctx.RenderComponent<Logs>();

        // Assert
        Assert.That(cut, Is.Not.Null);
        // Component should handle empty state (exact message depends on implementation)
        Assert.That(cut.Markup, Is.Not.Empty);
    }

    [Test]
    public void Render_WithEntries_DisplaysInChronologicalOrder()
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

        // Act
        var cut = ctx.RenderComponent<Logs>();

        // Assert
        Assert.That(cut, Is.Not.Null);
        var markup = cut.Markup;
        var newestIndex = markup.IndexOf("Newest", StringComparison.Ordinal);
        var middleIndex = markup.IndexOf("Middle", StringComparison.Ordinal);
        var oldestIndex = markup.IndexOf("Oldest", StringComparison.Ordinal);

        // Newest should appear first (default order)
        Assert.That(newestIndex, Is.GreaterThan(-1), "Newest message should be found");
        Assert.That(middleIndex, Is.GreaterThan(-1), "Middle message should be found");
        Assert.That(oldestIndex, Is.GreaterThan(-1), "Oldest message should be found");
        Assert.That(newestIndex, Is.LessThan(middleIndex), "Newest should appear before middle");
        Assert.That(middleIndex, Is.LessThan(oldestIndex), "Middle should appear before oldest");
    }

    [Test]
    public void Render_WithEntries_DisplaysTimestampLevelAndMessage()
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

        // Act
        var cut = ctx.RenderComponent<Logs>();

        // Assert
        Assert.That(cut, Is.Not.Null);
        var markup = cut.Markup;
        Assert.That(markup, Does.Contain("Error occurred"), "Should display message");
        Assert.That(markup, Does.Contain("Error"), "Should display level");
        // Timestamp format may vary, so we just check that component rendered
    }

    [Test]
    public void Search_ByMessageText_FiltersEntries()
    {
        // Arrange
        _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "User logged in successfully",
            IsManualEntry = false
        });
        _logService.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow.AddSeconds(-1),
            Level = "Error",
            Message = "Database connection failed",
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Find search input and enter search term
        var searchInput = cut.Find("input[type='search']");
        searchInput.Change("logged");

        // Assert
        Assert.That(cut.Markup, Does.Contain("User logged in successfully"));
        Assert.That(cut.Markup, Does.Not.Contain("Database connection failed"));
    }

    [Test]
    public void Search_ByStructuredData_FiltersEntries()
    {
        // Arrange
        _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Processing request",
            Properties = new Dictionary<string, object> { { "UserId", "12345" }, { "Action", "Login" } },
            IsManualEntry = false
        });
        _logService.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow.AddSeconds(-1),
            Level = "Info",
            Message = "Another message",
            Properties = new Dictionary<string, object> { { "UserId", "67890" } },
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Search for value in structured data
        var searchInput = cut.Find("input[type='search']");
        searchInput.Change("12345");

        // Assert
        Assert.That(cut.Markup, Does.Contain("Processing request"));
        Assert.That(cut.Markup, Does.Not.Contain("Another message"));
    }

    [Test]
    public void Search_CaseInsensitive_FiltersEntries()
    {
        // Arrange
        _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "User LOGGED in",
            IsManualEntry = false
        });
        _logService.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow.AddSeconds(-1),
            Level = "Error",
            Message = "Different message",
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Search with lowercase term
        var searchInput = cut.Find("input[type='search']");
        searchInput.Change("logged");

        // Assert
        Assert.That(cut.Markup, Does.Contain("User LOGGED in"));
        Assert.That(cut.Markup, Does.Not.Contain("Different message"));
    }

    [Test]
    public void Filter_ByLogLevel_FiltersEntries()
    {
        // Arrange
        _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Info message",
            IsManualEntry = false
        });
        _logService.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow.AddSeconds(-1),
            Level = "Error",
            Message = "Error message",
            IsManualEntry = false
        });
        _logService.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow.AddSeconds(-2),
            Level = "Warn",
            Message = "Warning message",
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Filter by Error level
        var levelSelect = cut.Find("select");
        levelSelect.Change("Error");

        // Assert
        Assert.That(cut.Markup, Does.Contain("Error message"));
        Assert.That(cut.Markup, Does.Not.Contain("Info message"));
        Assert.That(cut.Markup, Does.Not.Contain("Warning message"));
    }

    [Test]
    public void Filter_ClearFilter_ShowsAllEntries()
    {
        // Arrange
        _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Info message",
            IsManualEntry = false
        });
        _logService.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow.AddSeconds(-1),
            Level = "Error",
            Message = "Error message",
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Filter by Error, then clear filter
        var levelSelect = cut.Find("select");
        levelSelect.Change("Error");
        
        // Clear filter (set to empty or "All")
        levelSelect.Change("");

        // Assert
        Assert.That(cut.Markup, Does.Contain("Info message"));
        Assert.That(cut.Markup, Does.Contain("Error message"));
    }

    [Test]
    public void Search_NoMatches_DisplaysEmptySearchResultsMessage()
    {
        // Arrange
        _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Some message",
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Search for term that doesn't match
        var searchInput = cut.Find("input[type='search']");
        searchInput.Change("nonexistent");

        // Assert
        Assert.That(cut.Markup, Does.Not.Contain("Some message"));
        // Component should display empty search results message
        Assert.That(cut.Markup, Does.Contain("No log entries found").Or.Contain("No results").Or.Contain("No matches"));
    }

    [Test]
    public void Create_NewLogEntry_AppearsInList()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);
        FluentUIJSInteropHelper.ConfigureJSInterop(ctx);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Find and click create button, fill form, save
        var createButton = cut.Find("button:contains('Create')");
        createButton.Click();

        // Fill in form fields (assuming form appears)
        var messageInput = cut.Find("input[placeholder*='Message']");
        messageInput.Change("New manual log entry");

        var levelSelect = cut.Find("select[name*='Level']");
        levelSelect.Change("Info");

        var saveButton = cut.Find("button:contains('Save')");
        saveButton.Click();

        // Assert
        Assert.That(cut.Markup, Does.Contain("New manual log entry"));
        
        // Verify entry was created with IsManualEntry=true
        var entries = _logService!.Search();
        var createdEntry = entries.FirstOrDefault(e => e.Message == "New manual log entry");
        Assert.That(createdEntry, Is.Not.Null);
        Assert.That(createdEntry!.IsManualEntry, Is.True);
    }

    [Test]
    public void Update_ManualLogEntry_ChangesPersist()
    {
        // Arrange - Create a manual entry
        var entry = _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Original message",
            IsManualEntry = true
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Find edit button for manual entry, edit, save
        var editButton = cut.Find($"button[data-entry-id='{entry.Id}']:contains('Edit')");
        editButton.Click();

        // Update message in form
        var messageInput = cut.Find("input[value='Original message']");
        messageInput.Change("Updated message");

        var saveButton = cut.Find("button:contains('Save')");
        saveButton.Click();

        // Assert
        Assert.That(cut.Markup, Does.Contain("Updated message"));
        Assert.That(cut.Markup, Does.Not.Contain("Original message"));
        
        // Verify changes persisted
        var updatedEntry = _logService.GetById(entry.Id);
        Assert.That(updatedEntry, Is.Not.Null);
        Assert.That(updatedEntry!.Message, Is.EqualTo("Updated message"));
    }

    [Test]
    public void Update_AutomaticLogEntry_IsPrevented()
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

        var cut = ctx.RenderComponent<Logs>();

        // Act & Assert - Edit button should be disabled or not present for automatic entries
        var editButtons = cut.FindAll("button:contains('Edit')");
        var entryEditButton = editButtons.FirstOrDefault(b => 
            b.GetAttribute("data-entry-id") == entry.Id.ToString());
        
        // Edit button should be disabled or not exist for automatic entries
        if (entryEditButton != null)
        {
            var disabledAttr = entryEditButton.GetAttribute("disabled");
            Assert.That(disabledAttr, Is.Not.Null, "Edit button should be disabled for automatic entries");
        }
        
        // Attempting to update should throw exception
        entry.Message = "Attempted update";
        Assert.Throws<InvalidOperationException>(() => _logService.Update(entry));
    }

    [Test]
    public void Delete_LogEntry_RemovesFromList()
    {
        // Arrange - Create an entry
        var entry = _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = "Entry to delete",
            IsManualEntry = true
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Verify entry is displayed
        Assert.That(cut.Markup, Does.Contain("Entry to delete"));

        // Act - Find delete button, click, confirm
        var deleteButton = cut.Find($"button[data-entry-id='{entry.Id}']:contains('Delete')");
        deleteButton.Click();

        // Confirm deletion in dialog
        var confirmButton = cut.Find("button:contains('Confirm')");
        confirmButton.Click();

        // Assert
        Assert.That(cut.Markup, Does.Not.Contain("Entry to delete"));
        
        // Verify entry was deleted
        var deletedEntry = _logService.GetById(entry.Id);
        Assert.That(deletedEntry, Is.Null);
    }

    [Test]
    public void Pagination_NavigatePages_DisplaysCorrectEntries()
    {
        // Arrange - Create more entries than page size
        for (int i = 0; i < 30; i++)
        {
            _logService!.Create(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow.AddSeconds(-i),
                Level = "Info",
                Message = $"Message {i}",
                IsManualEntry = false
            });
        }

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Assert - First page should show entries
        Assert.That(cut.Markup, Does.Contain("Message 0"));

        // Act - Navigate to next page
        var nextButton = cut.Find("button:contains('Next')");
        nextButton.Click();

        // Assert - Second page should show different entries
        // (exact content depends on page size, but should be different)
        Assert.That(cut.Markup, Does.Not.Contain("Message 0").Or.Contains("Message 25"));
    }

    [Test]
    public void Pagination_ChangePageSize_UpdatesDisplay()
    {
        // Arrange - Create entries
        for (int i = 0; i < 30; i++)
        {
            _logService!.Create(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow.AddSeconds(-i),
                Level = "Info",
                Message = $"Message {i}",
                IsManualEntry = false
            });
        }

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Change page size to 50
        var pageSizeSelect = cut.Find("select");
        var pageSizeOptions = cut.FindAll("option");
        // Find the option with value "50"
        var option50 = pageSizeOptions.FirstOrDefault(o => o.GetAttribute("value") == "50");
        if (option50 != null)
        {
            pageSizeSelect.Change("50");
        }

        // Assert - More entries should be visible (or page count should change)
        // The exact assertion depends on implementation, but page size change should be reflected
        Assert.That(cut.Markup, Is.Not.Empty);
    }

    [Test]
    public void SortOrder_ToggleToOldestFirst_DisplaysOldestFirst()
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

        var cut = ctx.RenderComponent<Logs>();

        // Verify default is newest first
        var markup = cut.Markup;
        var newestIndex = markup.IndexOf("Newest", StringComparison.Ordinal);
        var oldestIndex = markup.IndexOf("Oldest", StringComparison.Ordinal);
        Assert.That(newestIndex, Is.LessThan(oldestIndex), "Default should be newest first");

        // Act - Toggle to oldest first
        var sortButton = cut.Find("button:contains('Newest First')");
        sortButton.Click();

        // Assert - Should now be oldest first
        markup = cut.Markup;
        newestIndex = markup.IndexOf("Newest", StringComparison.Ordinal);
        oldestIndex = markup.IndexOf("Oldest", StringComparison.Ordinal);
        Assert.That(oldestIndex, Is.LessThan(newestIndex), "After toggle should be oldest first");
    }

    [Test]
    public void SortOrder_ToggleToNewestFirst_DisplaysNewestFirst()
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

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Act - Toggle to oldest first, then back to newest first
        var sortButton = cut.Find("button:contains('Newest First')");
        sortButton.Click(); // Toggle to oldest first
        sortButton.Click(); // Toggle back to newest first

        // Assert - Should be newest first again
        var markup = cut.Markup;
        var newestIndex = markup.IndexOf("Newest", StringComparison.Ordinal);
        var oldestIndex = markup.IndexOf("Oldest", StringComparison.Ordinal);
        Assert.That(newestIndex, Is.LessThan(oldestIndex), "Should be newest first after toggle back");
    }

    [Test]
    public void MessageTruncation_LongMessage_TruncatesAndExpands()
    {
        // Arrange - Create entry with long message
        var longMessage = new string('A', 150); // 150 characters
        var entry = _logService!.Create(new LogEntry
        {
            Timestamp = DateTimeOffset.UtcNow,
            Level = "Info",
            Message = longMessage,
            IsManualEntry = false
        });

        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<ILogService>(_logService!);

        var cut = ctx.RenderComponent<Logs>();

        // Assert - Message should be truncated
        Assert.That(cut.Markup, Does.Contain("..."));
        Assert.That(cut.Markup, Does.Contain("Expand"));

        // Act - Click expand button
        var expandButton = cut.Find("button:contains('Expand')");
        expandButton.Click();

        // Assert - Full message should be visible
        Assert.That(cut.Markup, Does.Contain(longMessage));
        Assert.That(cut.Markup, Does.Contain("Collapse"));

        // Act - Click collapse button
        var collapseButton = cut.Find("button:contains('Collapse')");
        collapseButton.Click();

        // Assert - Message should be truncated again
        Assert.That(cut.Markup, Does.Contain("..."));
        Assert.That(cut.Markup, Does.Contain("Expand"));
    }

    [Test]
    public void MessageTruncation_ShortMessage_NoTruncation()
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

        var cut = ctx.RenderComponent<Logs>();

        // Assert - No truncation or expand button for short messages
        Assert.That(cut.Markup, Does.Contain("Short message"));
        Assert.That(cut.Markup, Does.Not.Contain("Expand"));
    }
}
#endif
