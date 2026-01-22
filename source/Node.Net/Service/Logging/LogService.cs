using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;

namespace Node.Net.Service.Logging;

/// <summary>
/// Concrete implementation of ILogService backed by LiteDB database.
/// </summary>
/// <remarks>
/// This implementation stores log entries in a LiteDB database file. The database is created
/// automatically if it doesn't exist. Indexes are created on key fields for performance.
/// Thread-safe operations are handled by LiteDB.
/// </remarks>
public class LogService : ILogService, IDisposable
{
    private readonly string _databasePath;
    private readonly Lazy<ILiteDatabase> _database;
    private const string LogEntriesCollectionName = "logentries";

    /// <summary>
    /// Initializes a new instance of the LogService class with the specified database path.
    /// </summary>
    /// <param name="databasePath">The path to the LiteDB database file. If null or empty, uses an in-memory database.</param>
    /// <exception cref="ArgumentException">Thrown when the database path contains invalid characters.</exception>
    public LogService(string? databasePath = null)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
        {
            _databasePath = ":memory:";
        }
        else
        {
            if (databasePath!.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException("Database path contains invalid characters.", nameof(databasePath));
            }

            _databasePath = databasePath;
        }

        _database = new Lazy<ILiteDatabase>(() => CreateDatabase(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
    }

    /// <summary>
    /// Creates and initializes the LiteDB database instance with required indexes.
    /// </summary>
    private ILiteDatabase CreateDatabase()
    {
        ILiteDatabase db;
        if (_databasePath == ":memory:")
        {
            db = new LiteDatabase(new MemoryStream());
        }
        else
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_databasePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            db = new LiteDatabase(_databasePath);
        }

        // Create indexes for performance
        var collection = db.GetCollection<LogEntry>(LogEntriesCollectionName);
        collection.EnsureIndex(x => x.Timestamp);
        collection.EnsureIndex(x => x.Level);
        collection.EnsureIndex(x => x.Message);
        collection.EnsureIndex(x => x.IsManualEntry);

        return db;
    }

    /// <summary>
    /// Gets the LiteDB database instance.
    /// </summary>
    private ILiteDatabase Database => _database.Value;

    /// <summary>
    /// Gets the log entries collection from the database.
    /// </summary>
    private ILiteCollection<LogEntry> LogEntries => Database.GetCollection<LogEntry>(LogEntriesCollectionName);

    /// <inheritdoc/>
    public LogEntry Create(LogEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        entry.Validate();

        // Set default IsManualEntry if not set (defaults to false for automatic, but Create is typically called for manual entries)
        // The caller should set IsManualEntry appropriately

        // Insert into database (LiteDB will auto-generate Id if ObjectId.Empty)
        if (entry.Id == ObjectId.Empty)
        {
            entry.Id = ObjectId.NewObjectId();
        }

        LogEntries.Insert(entry);
        return entry;
    }

    /// <inheritdoc/>
    public LogEntry? GetById(ObjectId id)
    {
        return LogEntries.FindById(id);
    }

    /// <inheritdoc/>
    public void Update(LogEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        if (entry.Id == ObjectId.Empty)
        {
            throw new ArgumentException("Entry Id must be set for update operations.", nameof(entry));
        }

        entry.Validate();

        // Check if entry exists
        var existing = LogEntries.FindById(entry.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Log entry with Id {entry.Id} does not exist.");
        }

        // Validate that entry is manually created (read-only check)
        if (!existing.IsManualEntry)
        {
            throw new InvalidOperationException("Cannot update automatically captured log entries. Only manually created entries can be updated.");
        }

        // Update entry
        LogEntries.Update(entry);
    }

    /// <inheritdoc/>
    public bool Delete(ObjectId id)
    {
        return LogEntries.Delete(id);
    }

    /// <inheritdoc/>
    public IEnumerable<LogEntry> Search(
        string? searchTerm = null,
        string? levelFilter = null,
        int skip = 0,
        int take = 25,
        bool orderByNewestFirst = true)
    {
        if (skip < 0)
        {
            throw new ArgumentException("Skip must be >= 0.", nameof(skip));
        }

        if (take <= 0 || take > 1000)
        {
            throw new ArgumentException("Take must be between 1 and 1000.", nameof(take));
        }

        var query = LogEntries.Query();

        // Apply level filter first (can be done in LiteDB)
        if (!string.IsNullOrWhiteSpace(levelFilter))
        {
            query = query.Where(x => x.Level != null && x.Level.Equals(levelFilter, StringComparison.OrdinalIgnoreCase));
        }

        // Apply ordering (must be done before search for proper pagination)
        if (orderByNewestFirst)
        {
            query = query.OrderByDescending(x => x.Timestamp);
        }
        else
        {
            query = query.OrderBy(x => x.Timestamp);
        }

        // Convert to enumerable for search term filtering (LiteDB LINQ limitations with complex property searches)
        var entries = query.ToEnumerable();

        // Apply search term filter (searches message and properties) in memory
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm!.ToLowerInvariant();
            entries = entries.Where(x =>
            {
                var messageMatch = x.Message != null && x.Message.ToLowerInvariant().Contains(searchLower);
                if (messageMatch) return true;
                
                if (x.Properties != null)
                {
                    foreach (var value in x.Properties.Values)
                    {
                        if (value != null)
                        {
                            var valueStr = value.ToString();
                            if (valueStr != null && valueStr.ToLowerInvariant().Contains(searchLower))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            });
        }

        // Apply pagination
        return entries.Skip(skip).Take(take);
    }

    /// <inheritdoc/>
    public int GetCount(string? searchTerm = null, string? levelFilter = null)
    {
        var query = LogEntries.Query();

        // Apply level filter first (can be done in LiteDB)
        if (!string.IsNullOrWhiteSpace(levelFilter))
        {
            query = query.Where(x => x.Level != null && x.Level.Equals(levelFilter, StringComparison.OrdinalIgnoreCase));
        }

        // Convert to enumerable for search term filtering (LiteDB LINQ limitations with complex property searches)
        var entries = query.ToEnumerable();

        // Apply search term filter (same logic as Search method) in memory
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm!.ToLowerInvariant();
            entries = entries.Where(x =>
            {
                var messageMatch = x.Message != null && x.Message.ToLowerInvariant().Contains(searchLower);
                if (messageMatch) return true;
                
                if (x.Properties != null)
                {
                    foreach (var value in x.Properties.Values)
                    {
                        if (value != null)
                        {
                            var valueStr = value.ToString();
                            if (valueStr != null && valueStr.ToLowerInvariant().Contains(searchLower))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            });
        }

        return entries.Count();
    }

    /// <summary>
    /// Disposes the database resources.
    /// </summary>
    public void Dispose()
    {
        if (_database.IsValueCreated)
        {
            _database.Value?.Dispose();
        }
    }
}
