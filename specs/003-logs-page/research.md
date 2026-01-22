# Research: Logs Page

**Feature**: 003-logs-page  
**Date**: 2025-01-12  
**Purpose**: Resolve technical unknowns and document implementation decisions

## Research Topics

### 1. Serilog Structured Logging Format Compatibility

**Question**: What fields and structure does LogEntry need to be compatible with Serilog structured logging?

**Research Findings**:
- Serilog log entries typically include:
  - Timestamp (DateTimeOffset)
  - Level (LogLevel enum: Verbose, Debug, Information, Warning, Error, Fatal)
  - Message template and rendered message
  - Properties (dictionary of structured data)
  - Exception (optional)
  - Source context (type/namespace)
- Microsoft.Extensions.Logging uses similar structure but with different property names
- Need to map between Microsoft.Extensions.Logging.LogLevel and Node.Net.LogLevel

**Decision**: LogEntry will include:
- `Id` (ObjectId for LiteDB primary key)
- `Timestamp` (DateTimeOffset)
- `Level` (string or enum compatible with both systems)
- `Message` (string - rendered message)
- `MessageTemplate` (string - template if available)
- `Properties` (dictionary<string, object> for structured data)
- `Exception` (string - serialized exception if present)
- `SourceContext` (string - type/namespace)
- `IsManualEntry` (bool - flag to distinguish manually created vs automatically captured entries)

**Rationale**: This structure captures all essential log information while maintaining compatibility with both Serilog format and Microsoft.Extensions.Logging.

**Alternatives Considered**:
- Using Serilog's LogEvent directly: Rejected - would create dependency on Serilog package
- Minimal structure with only message and level: Rejected - loses structured data capability required by spec

---

### 2. Microsoft.Extensions.Logging Integration

**Question**: How to integrate with Microsoft.Extensions.Logging to automatically capture application logs?

**Research Findings**:
- Microsoft.Extensions.Logging uses ILoggerProvider pattern
- Need to implement ILoggerProvider and ILogger interfaces
- ILoggerProvider.CreateLogger() is called for each category (typically type name)
- ILogger.Log() method receives LogLevel, EventId, state (TState), exception, and formatter
- Can extract structured properties from state if it implements IReadOnlyList<KeyValuePair<string, object>>
- Need to register provider in service collection: `services.AddLogging(builder => builder.AddProvider(new LogServiceProvider(...)))`

**Decision**: Create `LogServiceLoggerProvider` class implementing `ILoggerProvider` and `ILogger`:
- Provider creates logger instances that write to ILogService
- Logger extracts message, level, properties, and exception from log state
- Provider registered via extension method: `services.AddNodeNetLogging(logService)`

**Rationale**: Standard Microsoft.Extensions.Logging pattern, well-documented and widely used.

**Alternatives Considered**:
- Custom logging sink: Rejected - doesn't integrate with standard Microsoft.Extensions.Logging pipeline
- Wrapper around existing Log.cs: Rejected - doesn't support structured logging required by spec

---

### 3. LiteDB Schema and Query Patterns

**Question**: How to structure LiteDB database schema and implement efficient queries for search and pagination?

**Research Findings**:
- LiteDB uses BSON documents (similar to MongoDB)
- Collections created automatically on first insert
- Indexes improve query performance: `collection.EnsureIndex("fieldName")`
- Queries use LINQ: `collection.Query().Where(...).OrderBy(...).Skip(...).Take(...)`
- Full-text search requires indexing text fields or using LINQ Contains/StartsWith
- ObjectId type for primary keys (auto-generated)

**Decision**: 
- Collection name: "logentries"
- Indexes:
  - `Timestamp` (descending for newest-first default)
  - `Level` (for filtering)
  - `Message` (for search)
  - `IsManualEntry` (for editability checks)
- Query pattern: Use LINQ with Where, OrderBy, Skip, Take for pagination
- Search: Use LINQ Contains on Message and Properties values

**Rationale**: LiteDB's LINQ support provides flexible querying while maintaining good performance with proper indexing.

**Alternatives Considered**:
- SQLite: Rejected - LiteDB already in project, lighter weight, better for embedded scenarios
- In-memory storage: Rejected - doesn't meet persistence requirement (FR-009)

---

### 4. Blazor Component Structure and Fluent UI Integration

**Question**: How to structure the Blazor component for reusability and integrate Fluent UI components?

**Research Findings**:
- Fluent UI Blazor components available: FluentDataGrid, FluentSearch, FluentSelect, FluentButton
- FluentDataGrid supports pagination, sorting, and filtering
- Components should accept ILogService via dependency injection or parameter
- Use @code blocks for component logic
- Fluent UI components require net8.0+ (already excluded from net48)

**Decision**: 
- Component structure:
  - Parameters: `ILogService?` (optional, uses DI if not provided)
  - State: Current page, page size, search term, log level filter, sort order
  - Methods: LoadLogs(), Search(), FilterByLevel(), ChangePage(), ToggleSortOrder()
- Use FluentDataGrid for log entry display with pagination
- Use FluentSearch for search input
- Use FluentSelect for log level filter
- Use FluentButton for create/edit/delete actions
- Truncate long messages with FluentExpander or custom expand/collapse

**Rationale**: Fluent UI components provide consistent styling and built-in functionality (pagination, search) reducing custom code.

**Alternatives Considered**:
- Custom HTML/CSS: Rejected - Fluent UI already in project, provides better UX and consistency
- Server-side only component: Rejected - spec requires both ASP.NET Host and WebAssembly support

---

### 5. Editability and Read-Only Enforcement

**Question**: How to enforce read-only status for automatically captured logs vs manually created ones?

**Research Findings**:
- `IsManualEntry` flag in LogEntry distinguishes entry types
- UI can check flag before enabling edit/delete buttons
- Service layer should also enforce: `Update()` and `Delete()` methods check flag
- Exception thrown if attempting to edit/delete read-only entry

**Decision**:
- LogEntry includes `IsManualEntry` boolean property
- Automatically captured entries: `IsManualEntry = false`
- Manually created entries: `IsManualEntry = true`
- ILogService.Update() and Delete() methods validate flag before allowing operation
- Component UI disables edit/delete buttons for read-only entries
- Service throws `InvalidOperationException` if attempting to modify read-only entry

**Rationale**: Flag-based approach is simple, explicit, and easy to validate at both UI and service layers.

**Alternatives Considered**:
- Separate collections: Rejected - complicates queries and search across all entries
- Inheritance hierarchy: Rejected - over-engineered for simple boolean distinction

---

### 6. WebAssembly Compatibility

**Question**: How to ensure LiteDB and service layer work in WebAssembly environment?

**Research Findings**:
- LiteDB 5.0.17 supports WebAssembly (uses IndexedDB under the hood in browser)
- File system access limited in WebAssembly - LiteDB handles this abstraction
- Service layer is platform-agnostic (no file system direct access)
- Component uses standard Blazor patterns (works in both Server and WebAssembly)

**Decision**: 
- No special WebAssembly-specific code required
- LiteDB automatically uses IndexedDB in browser environment
- Service layer remains unchanged
- Component works identically in both hosting models

**Rationale**: LiteDB's cross-platform support and Blazor's component model ensure compatibility without additional work.

**Alternatives Considered**:
- Separate WebAssembly implementation: Rejected - unnecessary, LiteDB and Blazor handle platform differences
- Browser storage API: Rejected - LiteDB provides better abstraction and consistency

---

## Summary of Decisions

1. **LogEntry Structure**: Comprehensive structure with timestamp, level, message, properties, exception, source context, and manual entry flag
2. **Microsoft.Extensions.Logging Integration**: ILoggerProvider pattern with extension method for registration
3. **LiteDB Schema**: Single collection with indexes on key fields for performance
4. **Blazor Component**: Fluent UI components with dependency injection for ILogService
5. **Editability**: Boolean flag-based approach with validation at service and UI layers
6. **WebAssembly**: No special handling required - LiteDB and Blazor handle platform differences

All technical unknowns resolved. Ready for Phase 1 design.
