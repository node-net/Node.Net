using System.Threading;
using System.Threading.Tasks;

namespace Node.Net.Security;

/// <summary>
/// Provides secure per-user per-machine secret storage.
/// Secrets are stable, high-entropy (384 bits), and persisted securely using OS-protected storage.
/// </summary>
public interface IUserSecretProvider
{
    /// <summary>
    /// Returns a stable, high-entropy secret unique per user per machine.
    /// The secret is generated once and persisted securely.
    /// If a secret already exists for the given key, it is returned.
    /// </summary>
    /// <param name="key">The key identifying the secret.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="UserSecret"/> containing the secret and metadata.</returns>
    /// <exception cref="ArgumentException">Thrown when key is invalid (null, empty, or exceeds 256 characters).</exception>
    /// <exception cref="InvalidOperationException">Thrown when secret storage is unavailable or corrupted.</exception>
    /// <exception cref="System.Security.SecurityException">Thrown when access to secret storage is denied.</exception>
    /// <exception cref="OperationCanceledException">Thrown when operation is cancelled after secret generation but before persistence.</exception>
    ValueTask<UserSecret> GetOrCreateAsync(
        UserSecretKey key,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the stored secret for the given key with a newly generated one.
    /// The operation is atomic - either the old secret is replaced or an exception is thrown.
    /// </summary>
    /// <param name="key">The key identifying the secret to rotate.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="UserSecret"/> containing the new secret and metadata.</returns>
    /// <exception cref="ArgumentException">Thrown when key is invalid (null, empty, or exceeds 256 characters).</exception>
    /// <exception cref="InvalidOperationException">Thrown when secret storage is unavailable or corrupted.</exception>
    /// <exception cref="System.Security.SecurityException">Thrown when access to secret storage is denied.</exception>
    /// <exception cref="OperationCanceledException">Thrown when operation is cancelled after secret generation but before persistence.</exception>
    ValueTask<UserSecret> RotateAsync(
        UserSecretKey key,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the stored secret for the given key.
    /// The operation is idempotent - no error is thrown if the secret does not exist.
    /// </summary>
    /// <param name="key">The key identifying the secret to delete.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <exception cref="ArgumentException">Thrown when key is invalid (null, empty, or exceeds 256 characters).</exception>
    /// <exception cref="InvalidOperationException">Thrown when secret storage is unavailable.</exception>
    /// <exception cref="System.Security.SecurityException">Thrown when access to secret storage is denied.</exception>
    ValueTask DeleteAsync(
        UserSecretKey key,
        CancellationToken cancellationToken = default);
}
