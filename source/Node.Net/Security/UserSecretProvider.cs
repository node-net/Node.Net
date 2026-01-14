using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Node.Net.Security;

/// <summary>
/// Abstract base class for platform-specific implementations of IUserSecretProvider.
/// Provides common secret generation logic and thread safety.
/// </summary>
public abstract class UserSecretProvider : IUserSecretProvider
{
    private const int SecretByteLength = 48; // 384 bits
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _keySemaphores = new();

    /// <summary>
    /// Returns a stable, high-entropy secret unique per user per machine.
    /// The secret is generated once and persisted securely.
    /// If a secret already exists for the given key, it is returned.
    /// </summary>
    public async ValueTask<UserSecret> GetOrCreateAsync(
        UserSecretKey key,
        CancellationToken cancellationToken = default)
    {
        // Validate key (validation happens in UserSecretKey constructor, but double-check)
        if (string.IsNullOrEmpty(key.Value))
        {
            throw new ArgumentException("Key value cannot be null or empty.", nameof(key));
        }

        if (key.Value.Length > 256)
        {
            throw new ArgumentException($"Key value cannot exceed 256 characters. Current length: {key.Value.Length}", nameof(key));
        }

        // Get or create semaphore for this key (thread safety)
        var semaphore = _keySemaphores.GetOrAdd(key.Value, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Try to retrieve existing secret
            var existingSecret = await TryRetrieveSecretAsync(key, cancellationToken).ConfigureAwait(false);
            if (existingSecret != null)
            {
                return existingSecret;
            }

            // Generate new secret
            var secretBytes = GenerateSecretBytes();
            var base64Secret = Convert.ToBase64String(secretBytes);
            var createdUtc = DateTimeOffset.UtcNow;

            cancellationToken.ThrowIfCancellationRequested();

            // Persist secret (platform-specific) - pass createdUtc so it's stored correctly
            await StoreSecretAsync(key, base64Secret, createdUtc, cancellationToken).ConfigureAwait(false);

            return new UserSecret(base64Secret, createdUtc, SecretByteLength);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <summary>
    /// Replaces the stored secret for the given key with a newly generated one.
    /// </summary>
    public async ValueTask<UserSecret> RotateAsync(
        UserSecretKey key,
        CancellationToken cancellationToken = default)
    {
        // Validate key
        if (string.IsNullOrEmpty(key.Value))
        {
            throw new ArgumentException("Key value cannot be null or empty.", nameof(key));
        }

        if (key.Value.Length > 256)
        {
            throw new ArgumentException($"Key value cannot exceed 256 characters. Current length: {key.Value.Length}", nameof(key));
        }

        // Get or create semaphore for this key
        var semaphore = _keySemaphores.GetOrAdd(key.Value, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Generate new secret
            var secretBytes = GenerateSecretBytes();
            var base64Secret = Convert.ToBase64String(secretBytes);
            var createdUtc = DateTimeOffset.UtcNow;

            cancellationToken.ThrowIfCancellationRequested();

            // Atomically replace secret (platform-specific) - pass createdUtc so it's stored correctly
            await ReplaceSecretAsync(key, base64Secret, createdUtc, cancellationToken).ConfigureAwait(false);

            return new UserSecret(base64Secret, createdUtc, SecretByteLength);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <summary>
    /// Deletes the stored secret for the given key.
    /// </summary>
    public async ValueTask DeleteAsync(
        UserSecretKey key,
        CancellationToken cancellationToken = default)
    {
        // Validate key
        if (string.IsNullOrEmpty(key.Value))
        {
            throw new ArgumentException("Key value cannot be null or empty.", nameof(key));
        }

        if (key.Value.Length > 256)
        {
            throw new ArgumentException($"Key value cannot exceed 256 characters. Current length: {key.Value.Length}", nameof(key));
        }

        // Get or create semaphore for this key
        var semaphore = _keySemaphores.GetOrAdd(key.Value, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Delete secret (platform-specific, idempotent)
            await DeleteSecretAsync(key, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <summary>
    /// Generates a cryptographically secure random secret of 48 bytes (384 bits).
    /// </summary>
    /// <returns>A byte array containing 48 random bytes.</returns>
    protected virtual byte[] GenerateSecretBytes()
    {
        var bytes = new byte[SecretByteLength];
#if IS_FRAMEWORK
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
#else
        RandomNumberGenerator.Fill(bytes);
#endif
        return bytes;
    }

    /// <summary>
    /// Attempts to retrieve an existing secret for the given key.
    /// </summary>
    /// <param name="key">The key identifying the secret.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The existing secret, or null if not found.</returns>
    protected abstract ValueTask<UserSecret?> TryRetrieveSecretAsync(
        UserSecretKey key,
        CancellationToken cancellationToken);

    /// <summary>
    /// Stores a secret for the given key.
    /// </summary>
    /// <param name="key">The key identifying the secret.</param>
    /// <param name="base64Secret">The Base64-encoded secret to store.</param>
    /// <param name="createdUtc">The creation timestamp to store with the secret.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected abstract ValueTask StoreSecretAsync(
        UserSecretKey key,
        string base64Secret,
        DateTimeOffset createdUtc,
        CancellationToken cancellationToken);

    /// <summary>
    /// Atomically replaces an existing secret with a new one.
    /// </summary>
    /// <param name="key">The key identifying the secret.</param>
    /// <param name="base64Secret">The new Base64-encoded secret.</param>
    /// <param name="createdUtc">The creation timestamp to store with the secret.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected abstract ValueTask ReplaceSecretAsync(
        UserSecretKey key,
        string base64Secret,
        DateTimeOffset createdUtc,
        CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a secret for the given key (idempotent operation).
    /// </summary>
    /// <param name="key">The key identifying the secret.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected abstract ValueTask DeleteSecretAsync(
        UserSecretKey key,
        CancellationToken cancellationToken);
}
