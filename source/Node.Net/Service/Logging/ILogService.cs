using System;
using System.Collections.Generic;
using LiteDB;

namespace Node.Net.Service.Logging;

/// <summary>
/// Interface for CRUD operations on LogEntry and search functionality.
/// </summary>
/// <remarks>
/// This interface provides methods for creating, reading, updating, and deleting log entries,
/// as well as searching and filtering log entries with pagination support.
/// </remarks>
public interface ILogService
{
    /// <summary>
    /// Creates a new log entry.
    /// </summary>
    /// <param name="entry">Log entry to create. The Id property may be null and will be assigned by the service.</param>
    /// <returns>Created log entry with assigned Id.</returns>
    /// <exception cref="ArgumentException">Thrown when entry validation fails.</exception>
    LogEntry Create(LogEntry entry);

    /// <summary>
    /// Retrieves a log entry by Id.
    /// </summary>
    /// <param name="id">Log entry identifier.</param>
    /// <returns>Log entry if found, null otherwise.</returns>
    LogEntry? GetById(ObjectId id);

    /// <summary>
    /// Updates an existing log entry (only allowed for manually created entries).
    /// </summary>
    /// <param name="entry">Log entry with updated data. The Id property must be set.</param>
    /// <exception cref="InvalidOperationException">Thrown if entry is read-only (automatically captured).</exception>
    /// <exception cref="KeyNotFoundException">Thrown if entry does not exist.</exception>
    /// <exception cref="ArgumentException">Thrown when entry validation fails.</exception>
    void Update(LogEntry entry);

    /// <summary>
    /// Deletes a log entry by Id.
    /// </summary>
    /// <param name="id">Log entry identifier.</param>
    /// <returns>True if entry was deleted, false if not found.</returns>
    bool Delete(ObjectId id);

    /// <summary>
    /// Searches log entries by content (message text and structured data fields).
    /// </summary>
    /// <param name="searchTerm">Search term (case-insensitive). Searches both message text and structured data properties.</param>
    /// <param name="levelFilter">Optional log level filter (Debug, Info, Warn, Error, Fatal). Case-insensitive.</param>
    /// <param name="skip">Number of entries to skip (for pagination, default: 0).</param>
    /// <param name="take">Number of entries to return (for pagination, default: 25).</param>
    /// <param name="orderByNewestFirst">True for newest first, false for oldest first (default: true).</param>
    /// <returns>Collection of matching log entries.</returns>
    /// <exception cref="ArgumentException">Thrown when skip < 0 or take is not in valid range (1-1000).</exception>
    IEnumerable<LogEntry> Search(
        string? searchTerm = null,
        string? levelFilter = null,
        int skip = 0,
        int take = 25,
        bool orderByNewestFirst = true
    );

    /// <summary>
    /// Gets total count of log entries matching search criteria.
    /// </summary>
    /// <param name="searchTerm">Search term (case-insensitive). Same as Search method.</param>
    /// <param name="levelFilter">Optional log level filter. Same as Search method.</param>
    /// <returns>Total count of matching entries.</returns>
    int GetCount(string? searchTerm = null, string? levelFilter = null);
}
