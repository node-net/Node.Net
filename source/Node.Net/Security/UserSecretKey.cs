using System;

namespace Node.Net.Security;

/// <summary>
/// A typed, namespaced identifier for a stored secret.
/// </summary>
/// <param name="Value">The key value (e.g., "litedb::node.net.desktop").</param>
public readonly record struct UserSecretKey(string Value)
{
    /// <summary>
    /// Creates a key for LiteDB database encryption.
    /// </summary>
    /// <param name="appId">The application identifier.</param>
    /// <returns>A <see cref="UserSecretKey"/> for LiteDB secrets.</returns>
    /// <exception cref="ArgumentException">Thrown when appId is null, empty, or would result in a key exceeding 256 characters.</exception>
    public static UserSecretKey LiteDb(string appId)
    {
        if (string.IsNullOrEmpty(appId))
        {
            throw new ArgumentException("Application ID cannot be null or empty.", nameof(appId));
        }

        var key = new UserSecretKey($"litedb::{appId}");
        ValidateKey(key);
        return key;
    }

    /// <summary>
    /// Creates a key for token cache encryption.
    /// </summary>
    /// <param name="appId">The application identifier.</param>
    /// <returns>A <see cref="UserSecretKey"/> for token cache secrets.</returns>
    /// <exception cref="ArgumentException">Thrown when appId is null, empty, or would result in a key exceeding 256 characters.</exception>
    public static UserSecretKey TokenCache(string appId)
    {
        if (string.IsNullOrEmpty(appId))
        {
            throw new ArgumentException("Application ID cannot be null or empty.", nameof(appId));
        }

        var key = new UserSecretKey($"token-cache::{appId}");
        ValidateKey(key);
        return key;
    }

    /// <summary>
    /// Returns the string representation of the key.
    /// </summary>
    /// <returns>The key value.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Validates the key value according to FR-018 requirements.
    /// </summary>
    /// <param name="key">The key to validate.</param>
    /// <exception cref="ArgumentException">Thrown when key is invalid (null, empty, or exceeds 256 characters).</exception>
    private static void ValidateKey(UserSecretKey key)
    {
        if (key.Value == null)
        {
            throw new ArgumentException("Key value cannot be null.", nameof(Value));
        }

        if (string.IsNullOrEmpty(key.Value))
        {
            throw new ArgumentException("Key value cannot be empty.", nameof(Value));
        }

        if (key.Value.Length > 256)
        {
            throw new ArgumentException($"Key value cannot exceed 256 characters. Current length: {key.Value.Length}", nameof(Value));
        }
    }
}
