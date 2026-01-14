using System.Collections.Generic;
using Node.Net.Service.FileSystem.FileSystem;

namespace Node.Net.Service.FileSystem;

/// <summary>
/// Interface for managing multiple named IFileSystem instances.
/// Provides a registry pattern for storing and retrieving file system implementations by name.
/// </summary>
/// <remarks>
/// This interface allows applications to manage multiple file system instances, each with a unique name.
/// This is useful for scenarios where different file systems are needed for different purposes
/// (e.g., "local", "cache", "temp", "user-data", "app-data", etc.).
/// </remarks>
public interface IFileSystemRegistry
{
    /// <summary>
    /// Registers an IFileSystem instance with the specified name.
    /// </summary>
    /// <param name="name">The name to register the file system under. Must not be null or empty.</param>
    /// <param name="fileSystem">The IFileSystem instance to register. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="fileSystem"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace, or when a file system with the same name is already registered.</exception>
    void Register(string name, IFileSystem fileSystem);

    /// <summary>
    /// Registers or replaces an IFileSystem instance with the specified name.
    /// If a file system with the given name already exists, it is replaced.
    /// </summary>
    /// <param name="name">The name to register the file system under. Must not be null or empty.</param>
    /// <param name="fileSystem">The IFileSystem instance to register. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="fileSystem"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    void RegisterOrReplace(string name, IFileSystem fileSystem);

    /// <summary>
    /// Retrieves an IFileSystem instance by name.
    /// </summary>
    /// <param name="name">The name of the file system to retrieve. Must not be null or empty.</param>
    /// <returns>The IFileSystem instance associated with the specified name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no file system is registered with the specified name.</exception>
    IFileSystem Get(string name);

    /// <summary>
    /// Attempts to retrieve an IFileSystem instance by name.
    /// </summary>
    /// <param name="name">The name of the file system to retrieve. Must not be null or empty.</param>
    /// <param name="fileSystem">When this method returns, contains the IFileSystem instance if found; otherwise, null.</param>
    /// <returns>true if a file system with the specified name was found; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    bool TryGet(string name, out IFileSystem? fileSystem);

    /// <summary>
    /// Removes a file system registration by name.
    /// </summary>
    /// <param name="name">The name of the file system to unregister. Must not be null or empty.</param>
    /// <returns>true if the file system was successfully removed; false if no file system was registered with the specified name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    bool Unregister(string name);

    /// <summary>
    /// Determines whether a file system is registered with the specified name.
    /// </summary>
    /// <param name="name">The name to check. Must not be null or empty.</param>
    /// <returns>true if a file system is registered with the specified name; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    bool IsRegistered(string name);

    /// <summary>
    /// Gets a collection of all registered file system names.
    /// </summary>
    /// <returns>An enumerable collection of registered file system names.</returns>
    IEnumerable<string> GetRegisteredNames();

    /// <summary>
    /// Gets the number of registered file systems.
    /// </summary>
    /// <returns>The count of registered file systems.</returns>
    int Count { get; }

    /// <summary>
    /// Gets or sets the default file system name.
    /// When set, this name is used when retrieving a file system without specifying a name.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when setting to null or empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when setting to a name that is not registered.</exception>
    string? DefaultName { get; set; }

    /// <summary>
    /// Gets the default file system instance.
    /// Returns the file system associated with <see cref="DefaultName"/>, or null if no default is set.
    /// </summary>
    /// <returns>The default IFileSystem instance, or null if no default is set.</returns>
    IFileSystem? GetDefault();

    /// <summary>
    /// Clears all registered file systems.
    /// </summary>
    /// <remarks>
    /// This method removes all registrations but does not dispose of the file system instances.
    /// Callers are responsible for disposing file systems if needed.
    /// </remarks>
    void Clear();
}
