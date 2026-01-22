# Quickstart: Logs Page Integration

**Feature**: 003-logs-page  
**Date**: 2025-01-12

## Overview

This guide demonstrates how to integrate the Logs Page component into applications. The component provides a complete UI for viewing, searching, filtering, and managing application logs.

## Prerequisites

- .NET 8.0 or later (for Blazor components)
- Node.Net library reference
- Microsoft.Extensions.Logging configured in application

## Integration Steps

### 1. Register Services

In your application's `Program.cs` (or `Startup.cs`), register the logging services:

```csharp
using Node.Net.Service.Logging;
using Node.Net.Service.Application;

// Get application data directory
var app = new Application();
var appInfo = app.GetApplicationInfo();
var logDbPath = Path.Combine(appInfo.DataDirectory, "log.db");

// Create and register LogService
var logService = new LogService(logDbPath);
services.AddSingleton<ILogService>(logService);

// Configure Microsoft.Extensions.Logging to route to LogService
services.AddNodeNetLogging(logService);

// Register other services...
```

### 2. Add Component to Page (ASP.NET Host)

In `examples/Node.Net.AspNet.Host/Components/Pages/Logs.razor`:

```razor
@page "/logs"
@using Node.Net.Components
@using Node.Net.Service.Logging

<PageTitle>Logs</PageTitle>

<h1>Application Logs</h1>

<Logs />
```

### 3. Add Component to Page (WebAssembly)

In `examples/Node.Net.Wasm/Pages/Logs.razor`:

```razor
@page "/logs"
@using Node.Net.Components
@using Node.Net.Service.Logging

<PageTitle>Logs</PageTitle>

<h1>Application Logs</h1>

<Logs />
```

### 4. Add Navigation Link

Add navigation link to your application's menu:

**ASP.NET Host** (`Components/Layout/NavMenu.razor`):
```razor
<FluentNavLink Href="/logs" Match="NavLinkMatch.Prefix">
    <FluentIcon Value="@(new Icons.Regular.Size24.DocumentText())" />
    Logs
</FluentNavLink>
```

**WebAssembly** (`Layout/NavMenu.razor`):
```razor
<FluentNavLink Href="/logs" Match="NavLinkMatch.Prefix">
    <FluentIcon Value="@(new Icons.Regular.Size24.DocumentText())" />
    Logs
</FluentNavLink>
```

## Usage Scenarios

### Scenario 1: View Application Logs

**Given**: Application is running and generating logs  
**When**: User navigates to `/logs` page  
**Then**: 
- Logs page displays with newest entries first
- Each entry shows timestamp, level, and message
- Pagination controls visible (default: 25 entries per page)

**Test Steps**:
1. Start application
2. Generate some log entries (use application features)
3. Navigate to `/logs`
4. Verify log entries are displayed
5. Verify newest entries appear first

---

### Scenario 2: Search Log Entries

**Given**: Log entries exist in the system  
**When**: User enters search term in search box  
**Then**: 
- Only matching entries are displayed
- Search matches message text and structured data properties
- Search is case-insensitive
- Results update within 1 second

**Test Steps**:
1. Navigate to `/logs`
2. Enter search term (e.g., "error")
3. Verify only matching entries displayed
4. Clear search
5. Verify all entries displayed again

---

### Scenario 3: Filter by Log Level

**Given**: Multiple log entries with different levels  
**When**: User selects log level from filter dropdown  
**Then**: 
- Only entries with selected level are displayed
- Filter can be combined with search
- Filter can be cleared to show all levels

**Test Steps**:
1. Navigate to `/logs`
2. Select "Error" from level filter
3. Verify only Error level entries displayed
4. Clear filter
5. Verify all entries displayed

---

### Scenario 4: Create Manual Log Entry

**Given**: Logs page is displayed  
**When**: User clicks "Create" button and fills in log entry form  
**Then**: 
- New entry is created with `IsManualEntry = true`
- Entry appears in log list
- Entry is editable and deletable

**Test Steps**:
1. Navigate to `/logs`
2. Click "Create" button
3. Fill in log entry form (level, message, etc.)
4. Click "Save"
5. Verify new entry appears in list
6. Verify entry can be edited and deleted

---

### Scenario 5: Edit Manual Log Entry

**Given**: Manually created log entry exists  
**When**: User selects entry and clicks "Edit"  
**Then**: 
- Entry form opens with current data
- User can modify entry
- Changes are saved and reflected in display

**Test Steps**:
1. Navigate to `/logs`
2. Create a manual log entry
3. Select the entry
4. Click "Edit"
5. Modify message or level
6. Click "Save"
7. Verify changes reflected in list

---

### Scenario 6: Attempt to Edit Automatic Log Entry

**Given**: Automatically captured log entry exists  
**When**: User attempts to edit the entry  
**Then**: 
- Edit button is disabled (read-only)
- If edit attempted programmatically, exception is thrown
- Entry cannot be modified

**Test Steps**:
1. Navigate to `/logs`
2. Generate automatic log entries (use application)
3. Select an automatic entry
4. Verify "Edit" button is disabled
5. Verify entry is read-only

---

### Scenario 7: Delete Log Entry

**Given**: Log entry exists (any type)  
**When**: User selects entry and clicks "Delete"  
**Then**: 
- Confirmation dialog appears
- On confirmation, entry is deleted
- Entry no longer appears in list

**Test Steps**:
1. Navigate to `/logs`
2. Select a log entry
3. Click "Delete"
4. Confirm deletion
5. Verify entry removed from list

---

### Scenario 8: Pagination

**Given**: More than 25 log entries exist  
**When**: User navigates through pages  
**Then**: 
- Page size selector available (25, 50, 100)
- Previous/Next page buttons work
- Current page and total pages displayed
- Page load completes within 2 seconds

**Test Steps**:
1. Navigate to `/logs`
2. Verify pagination controls visible
3. Change page size to 50
4. Navigate to next page
5. Verify different entries displayed
6. Verify page information updated

---

### Scenario 9: Long Message Display

**Given**: Log entry with very long message exists  
**When**: User views the entry  
**Then**: 
- Message is truncated with ellipsis
- "Expand" button/link available
- Clicking expand shows full message
- Message can be collapsed again

**Test Steps**:
1. Create log entry with long message (200+ characters)
2. Navigate to `/logs`
3. Verify message is truncated
4. Click "Expand"
5. Verify full message displayed
6. Click "Collapse"
7. Verify message truncated again

---

### Scenario 10: Empty State Handling

**Given**: No log entries exist  
**When**: User navigates to `/logs`  
**Then**: 
- Empty state message displayed
- "Create" button available
- No pagination controls shown

**Test Steps**:
1. Clear all log entries (or use fresh database)
2. Navigate to `/logs`
3. Verify empty state message
4. Verify "Create" button available

---

### Scenario 11: Search with No Results

**Given**: Log entries exist  
**When**: User enters search term with no matches  
**Then**: 
- "No results" message displayed
- Search term remains visible
- Clear search shows all entries again

**Test Steps**:
1. Navigate to `/logs`
2. Enter search term that matches nothing (e.g., "xyz123nonexistent")
3. Verify "No results" message
4. Clear search
5. Verify all entries displayed

---

## Configuration Options

### Custom Page Size

The component uses default page size of 25. To customize:

```razor
<Logs PageSize="50" />
```

### Custom Log Service Instance

If you need to use a different ILogService instance:

```razor
<Logs LogService="@myCustomLogService" />
```

## Troubleshooting

### Issue: Logs not appearing

**Solution**: 
1. Verify `ILogService` is registered in service collection
2. Verify `AddNodeNetLogging()` is called
3. Check database file exists at expected path
4. Verify application is generating logs

### Issue: Cannot edit automatic logs

**Expected Behavior**: Automatic logs are read-only by design. Only manually created entries can be edited.

### Issue: Search not working

**Solution**:
1. Verify search term is not empty
2. Check that entries exist with matching content
3. Verify search includes both message and properties

### Issue: Performance issues with many entries

**Solution**:
1. Use pagination (reduce page size)
2. Use search/filter to narrow results
3. Consider implementing log rotation/cleanup

## Next Steps

- Review component API documentation
- Customize component styling (Fluent UI themes)
- Implement log rotation policies
- Add export functionality (if needed)
