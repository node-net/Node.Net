using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Node.Net.Service.FileSystem.FileSystem;

namespace Node.Net.Service.FileSystem;

/// <summary>
/// Thread-safe implementation of IFileSystemRegistry for managing multiple named IFileSystem instances.
/// </summary>
/// <remarks>
/// This implementation uses a ReaderWriterLockSlim for thread-safe operations.
/// All methods are thread-safe and can be called from multiple threads concurrently.
/// </remarks>
public class FileSystemRegistry : IFileSystemRegistry, IDisposable
{
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
    private readonly Dictionary<string, IFileSystem> _fileSystems = new Dictionary<string, IFileSystem>();
    private string? _defaultName;

    /// <summary>
    /// Gets the number of registered file systems.
    /// </summary>
    public int Count
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _fileSystems.Count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// Gets or sets the default file system name.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when setting to null or empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when setting to a name that is not registered.</exception>
    public string? DefaultName
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _defaultName;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        set
        {
            if (value == null)
            {
                _lock.EnterWriteLock();
                try
                {
                    _defaultName = null;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Default name cannot be empty or whitespace.", nameof(value));
            }

            _lock.EnterWriteLock();
            try
            {
                if (!_fileSystems.ContainsKey(value))
                {
                    throw new KeyNotFoundException($"No file system is registered with the name '{value}'.");
                }
                _defaultName = value;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }

    /// <summary>
    /// Registers an IFileSystem instance with the specified name.
    /// </summary>
    /// <param name="name">The name to register the file system under. Must not be null or empty.</param>
    /// <param name="fileSystem">The IFileSystem instance to register. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="fileSystem"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace, or when a file system with the same name is already registered.</exception>
    public void Register(string name, IFileSystem fileSystem)
    {
        ValidateName(name);
        if (fileSystem == null)
        {
            throw new ArgumentNullException(nameof(fileSystem));
        }

        _lock.EnterWriteLock();
        try
        {
            if (_fileSystems.ContainsKey(name))
            {
                throw new ArgumentException($"A file system with the name '{name}' is already registered.", nameof(name));
            }
            _fileSystems[name] = fileSystem;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Registers or replaces an IFileSystem instance with the specified name.
    /// </summary>
    /// <param name="name">The name to register the file system under. Must not be null or empty.</param>
    /// <param name="fileSystem">The IFileSystem instance to register. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="fileSystem"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    public void RegisterOrReplace(string name, IFileSystem fileSystem)
    {
        ValidateName(name);
        if (fileSystem == null)
        {
            throw new ArgumentNullException(nameof(fileSystem));
        }

        _lock.EnterWriteLock();
        try
        {
            _fileSystems[name] = fileSystem;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Retrieves an IFileSystem instance by name.
    /// </summary>
    /// <param name="name">The name of the file system to retrieve. Must not be null or empty.</param>
    /// <returns>The IFileSystem instance associated with the specified name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no file system is registered with the specified name.</exception>
    public IFileSystem Get(string name)
    {
        ValidateName(name);

        _lock.EnterReadLock();
        try
        {
            if (!_fileSystems.TryGetValue(name, out var fileSystem))
            {
                throw new KeyNotFoundException($"No file system is registered with the name '{name}'.");
            }
            return fileSystem;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Attempts to retrieve an IFileSystem instance by name.
    /// </summary>
    /// <param name="name">The name of the file system to retrieve. Must not be null or empty.</param>
    /// <param name="fileSystem">When this method returns, contains the IFileSystem instance if found; otherwise, null.</param>
    /// <returns>true if a file system with the specified name was found; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    public bool TryGet(string name, out IFileSystem? fileSystem)
    {
        ValidateName(name);

        _lock.EnterReadLock();
        try
        {
            return _fileSystems.TryGetValue(name, out fileSystem);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Removes a file system registration by name.
    /// </summary>
    /// <param name="name">The name of the file system to unregister. Must not be null or empty.</param>
    /// <returns>true if the file system was successfully removed; false if no file system was registered with the specified name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    public bool Unregister(string name)
    {
        ValidateName(name);

        _lock.EnterWriteLock();
        try
        {
            var removed = _fileSystems.Remove(name);
            
            // If the removed file system was the default, clear the default name
            if (removed && _defaultName == name)
            {
                _defaultName = null;
            }
            
            return removed;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Determines whether a file system is registered with the specified name.
    /// </summary>
    /// <param name="name">The name to check. Must not be null or empty.</param>
    /// <returns>true if a file system is registered with the specified name; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    public bool IsRegistered(string name)
    {
        ValidateName(name);

        _lock.EnterReadLock();
        try
        {
            return _fileSystems.ContainsKey(name);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets a collection of all registered file system names.
    /// </summary>
    /// <returns>An enumerable collection of registered file system names.</returns>
    public IEnumerable<string> GetRegisteredNames()
    {
        _lock.EnterReadLock();
        try
        {
            // Return a copy to avoid issues with concurrent modifications
            return _fileSystems.Keys.ToList();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets the default file system instance.
    /// </summary>
    /// <returns>The default IFileSystem instance, or null if no default is set.</returns>
    public IFileSystem? GetDefault()
    {
        _lock.EnterReadLock();
        try
        {
            if (_defaultName == null)
            {
                return null;
            }

            return _fileSystems.TryGetValue(_defaultName, out var fileSystem) ? fileSystem : null;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Clears all registered file systems.
    /// </summary>
    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _fileSystems.Clear();
            _defaultName = null;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Validates that a name is not null or empty.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    private static void ValidateName(string? name)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty or whitespace.", nameof(name));
        }
    }

    /// <summary>
    /// Disposes the registry and releases any resources.
    /// </summary>
    /// <remarks>
    /// This method disposes the internal lock but does not dispose registered file systems.
    /// Callers are responsible for disposing file systems if needed.
    /// </remarks>
    public void Dispose()
    {
        _lock?.Dispose();
    }
}
