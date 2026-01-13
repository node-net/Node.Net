using System;
using System.Threading;
using System.Threading.Tasks;
using Node.Net.Security;

namespace Node.Net.Security.Android;

/// <summary>
/// Android implementation of IUserSecretProvider using Android Keystore.
/// </summary>
/// <remarks>
/// This is a placeholder implementation. Full Android Keystore integration requires
/// Xamarin/MAUI bindings or Java interop.
/// </remarks>
public class UserSecretProvider : Node.Net.Security.UserSecretProvider
{
    /// <summary>
    /// Attempts to retrieve an existing secret for the given key from Android Keystore.
    /// </summary>
    protected override ValueTask<UserSecret?> TryRetrieveSecretAsync(
        UserSecretKey key,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Implement Android Keystore integration
        throw new NotImplementedException("Android Keystore integration not yet implemented. Use Fallback provider.");
    }

    /// <summary>
    /// Stores a secret for the given key in Android Keystore.
    /// </summary>
    protected override ValueTask StoreSecretAsync(
        UserSecretKey key,
        string base64Secret,
        DateTimeOffset createdUtc,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Implement Android Keystore integration
        throw new NotImplementedException("Android Keystore integration not yet implemented. Use Fallback provider.");
    }

    /// <summary>
    /// Atomically replaces an existing secret with a new one in Android Keystore.
    /// </summary>
    protected override ValueTask ReplaceSecretAsync(
        UserSecretKey key,
        string base64Secret,
        DateTimeOffset createdUtc,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Implement Android Keystore integration
        throw new NotImplementedException("Android Keystore integration not yet implemented. Use Fallback provider.");
    }

    /// <summary>
    /// Deletes a secret for the given key from Android Keystore (idempotent operation).
    /// </summary>
    protected override ValueTask DeleteSecretAsync(
        UserSecretKey key,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Implement Android Keystore integration
        throw new NotImplementedException("Android Keystore integration not yet implemented. Use Fallback provider.");
    }
}
