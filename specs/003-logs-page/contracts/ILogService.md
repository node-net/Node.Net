# ILogService Interface Contract

**Feature**: 003-logs-page  
**Date**: 2025-01-12  
**Type**: Service Interface

## Interface Definition

```csharp
namespace Node.Net.Service.Logging;

/// <summary>
/// Interface for CRUD operations on LogEntry and search functionality
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Creates a new log entry
    /// </summary>
    /// <param name="entry">Log entry to create</param>
    /// <returns>Created log entry with assigned Id</returns>
    LogEntry Create(LogEntry entry);

    /// <summary>
    /// Retrieves a log entry by Id
    /// </summary>
    /// <param name="id">Log entry identifier</param>
    /// <returns>Log entry if found, null otherwise</returns>
    LogEntry? GetById(ObjectId id);

    /// <summary>
    /// Updates an existing log entry (only allowed for manually created entries)
    /// </summary>
    /// <param name="entry">Log entry with updated data</param>
    /// <exception cref="InvalidOperationException">Thrown if entry is read-only (automatically captured)</exception>
    /// <exception cref="KeyNotFoundException">Thrown if entry does not exist</exception>
    void Update(LogEntry entry);

    /// <summary>
    /// Deletes a log entry by Id
    /// </summary>
    /// <param name="id">Log entry identifier</param>
    /// <returns>True if entry was deleted, false if not found</returns>
    bool Delete(ObjectId id);

    /// <summary>
    /// Searches log entries by content (message text and structured data fields)
    /// </summary>
    /// <param name="searchTerm">Search term (case-insensitive)</param>
    /// <param name="levelFilter">Optional log level filter</param>
    /// <param name="skip">Number of entries to skip (for pagination)</param>
    /// <param name="take">Number of entries to return (for pagination)</param>
    /// <param name="orderByNewestFirst">True for newest first, false for oldest first</param>
    /// <returns>Collection of matching log entries</returns>
    IEnumerable<LogEntry> Search(
        string? searchTerm = null,
        string? levelFilter = null,
        int skip = 0,
        int take = 25,
        bool orderByNewestFirst = true
    );

    /// <summary>
    /// Gets total count of log entries matching search criteria
    /// </summary>
    /// <param name="searchTerm">Search term (case-insensitive)</param>
    /// <param name="levelFilter">Optional log level filter</param>
    /// <returns>Total count of matching entries</returns>
    int GetCount(string? searchTerm = null, string? levelFilter = null);
}
```

## Method Specifications

### Create

**Purpose**: Create a new log entry (manual or automatic)

**Parameters**:
- `entry`: LogEntry with all required fields (Id may be null, will be assigned)

**Returns**: LogEntry with assigned Id

**Side Effects**: 
- Adds entry to LiteDB collection
- Assigns ObjectId to entry.Id

**Preconditions**:
- `entry.Timestamp` is not in the future
- `entry.Level` is valid log level
- `entry.Message` is not null or empty

**Postconditions**:
- Entry exists in database with unique Id
- Entry is queryable via GetById, Search

---

### GetById

**Purpose**: Retrieve a specific log entry by identifier

**Parameters**:
- `id`: ObjectId of the log entry

**Returns**: LogEntry if found, null if not found

**Side Effects**: None

**Preconditions**: None

**Postconditions**: None

---

### Update

**Purpose**: Update an existing log entry (only manually created entries)

**Parameters**:
- `entry`: LogEntry with updated data (Id must be set)

**Returns**: void

**Side Effects**: 
- Updates entry in LiteDB collection
- Throws exception if entry is read-only

**Preconditions**:
- `entry.Id` is set and entry exists
- `entry.IsManualEntry` is true (entry must be manually created)

**Postconditions**:
- Entry data is updated in database
- Entry remains queryable with updated data

**Exceptions**:
- `InvalidOperationException`: If entry.IsManualEntry is false (read-only entry)
- `KeyNotFoundException`: If entry with given Id does not exist

---

### Delete

**Purpose**: Delete a log entry (any entry can be deleted)

**Parameters**:
- `id`: ObjectId of the log entry to delete

**Returns**: True if entry was deleted, false if not found

**Side Effects**: 
- Removes entry from LiteDB collection

**Preconditions**: None

**Postconditions**:
- Entry no longer exists in database
- Entry is no longer queryable

---

### Search

**Purpose**: Search and filter log entries with pagination support

**Parameters**:
- `searchTerm`: Optional search term (searches message and properties, case-insensitive)
- `levelFilter`: Optional log level filter (Debug, Info, Warn, Error, Fatal)
- `skip`: Number of entries to skip (for pagination, default: 0)
- `take`: Number of entries to return (for pagination, default: 25)
- `orderByNewestFirst`: Sort order (true = newest first, false = oldest first, default: true)

**Returns**: IEnumerable<LogEntry> of matching entries

**Side Effects**: None

**Preconditions**:
- `skip >= 0`
- `take > 0` and `take <= 1000` (reasonable limit)

**Postconditions**:
- Returns entries matching search criteria
- Results are paginated according to skip/take
- Results are sorted according to orderByNewestFirst

**Search Behavior**:
- If `searchTerm` provided: Searches `Message` field (case-insensitive contains) and `Properties` values (case-insensitive contains)
- If `levelFilter` provided: Filters by exact `Level` match (case-insensitive)
- If both provided: Applies both filters (AND logic)
- If neither provided: Returns all entries (subject to pagination)

---

### GetCount

**Purpose**: Get total count of entries matching search criteria (for pagination UI)

**Parameters**:
- `searchTerm`: Optional search term (same as Search method)
- `levelFilter`: Optional log level filter (same as Search method)

**Returns**: int count of matching entries

**Side Effects**: None

**Preconditions**: None

**Postconditions**: Returns accurate count for current search/filter criteria

## Implementation Requirements

- All methods must be thread-safe (LiteDB handles this)
- Search must be performant (uses indexes)
- Update must validate IsManualEntry flag
- All exceptions must be meaningful and include context

## Testing Requirements

- Unit tests for each method
- Integration tests with LiteDB
- Tests for read-only entry update prevention
- Tests for search with various criteria
- Tests for pagination edge cases
