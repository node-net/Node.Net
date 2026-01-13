#if IS_WINDOWS
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Node.Net.Security;

namespace Node.Net.Security.Windows;

/// <summary>
/// Windows implementation of IUserSecretProvider using DPAPI (Data Protection API).
/// </summary>
public class UserSecretProvider : Node.Net.Security.UserSecretProvider
{
    private const string EntropyPrefix = "Node.Net.UserSecretProvider:";

    /// <summary>
    /// Attempts to retrieve an existing secret for the given key using DPAPI.
    /// </summary>
    protected override ValueTask<UserSecret?> TryRetrieveSecretAsync(
        UserSecretKey key,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var encryptedData = ReadEncryptedData(key);
            if (encryptedData == null || encryptedData.Length == 0)
            {
                return new ValueTask<UserSecret?>((UserSecret?)null);
            }

            var entropy = GetEntropy(key);
            var decryptedData = ProtectedData.Unprotect(encryptedData, entropy, DataProtectionScope.CurrentUser);

            // Parse the stored data: format is "Base64|CreatedUtcTicks"
            var dataString = System.Text.Encoding.UTF8.GetString(decryptedData);
            var parts = dataString.Split('|');
            if (parts.Length != 2)
            {
                // Corrupted data
                throw new InvalidOperationException("Stored secret data is corrupted.", 
                    new FormatException("Invalid data format in stored secret."));
            }

            var base64 = parts[0];
            var createdTicks = long.Parse(parts[1]);
            var createdUtc = new DateTimeOffset(createdTicks, TimeSpan.Zero);

            return new ValueTask<UserSecret?>(new UserSecret(base64, createdUtc, 48));
        }
        catch (CryptographicException ex)
        {
            // Corrupted or inaccessible data
            throw new InvalidOperationException("Failed to retrieve secret from storage.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new System.Security.SecurityException("Access to secret storage is denied.", ex);
        }
    }

    /// <summary>
    /// Stores a secret for the given key using DPAPI.
    /// </summary>
    protected override ValueTask StoreSecretAsync(
        UserSecretKey key,
        string base64Secret,
        DateTimeOffset createdUtc,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var dataString = $"{base64Secret}|{createdUtc.UtcTicks}";
            var dataBytes = System.Text.Encoding.UTF8.GetBytes(dataString);
            var entropy = GetEntropy(key);

            var encryptedData = ProtectedData.Protect(dataBytes, entropy, DataProtectionScope.CurrentUser);
            WriteEncryptedData(key, encryptedData);

            return default(ValueTask);
        }
        catch (CryptographicException ex)
        {
            throw new InvalidOperationException("Failed to store secret in secure storage.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new System.Security.SecurityException("Access to secret storage is denied.", ex);
        }
    }

    /// <summary>
    /// Atomically replaces an existing secret with a new one.
    /// </summary>
    protected override ValueTask ReplaceSecretAsync(
        UserSecretKey key,
        string base64Secret,
        DateTimeOffset createdUtc,
        CancellationToken cancellationToken)
    {
        // For DPAPI, replacement is the same as storage (atomic write)
        return StoreSecretAsync(key, base64Secret, createdUtc, cancellationToken);
    }

    /// <summary>
    /// Deletes a secret for the given key (idempotent operation).
    /// </summary>
    protected override ValueTask DeleteSecretAsync(
        UserSecretKey key,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            DeleteEncryptedData(key);
            return default(ValueTask);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new System.Security.SecurityException("Access to secret storage is denied.", ex);
        }
    }

    /// <summary>
    /// Generates entropy for DPAPI encryption based on the key and user context.
    /// </summary>
    private static byte[] GetEntropy(UserSecretKey key)
    {
        // Use key value + user context for entropy (not machine identifiers per FR-008)
        var userContext = $"{Environment.UserName}@{Environment.UserDomainName}";
        var entropyString = $"{EntropyPrefix}{key.Value}:{userContext}";
        return System.Text.Encoding.UTF8.GetBytes(entropyString);
    }

    /// <summary>
    /// Reads encrypted data from storage for the given key.
    /// </summary>
    private byte[]? ReadEncryptedData(UserSecretKey key)
    {
        var filePath = GetStorageFilePath(key);
        if (!System.IO.File.Exists(filePath))
        {
            return null;
        }

        return System.IO.File.ReadAllBytes(filePath);
    }

    /// <summary>
    /// Writes encrypted data to storage for the given key.
    /// </summary>
    private void WriteEncryptedData(UserSecretKey key, byte[] encryptedData)
    {
        var filePath = GetStorageFilePath(key);
        var directory = System.IO.Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        System.IO.File.WriteAllBytes(filePath, encryptedData);
    }

    /// <summary>
    /// Deletes encrypted data from storage for the given key.
    /// </summary>
    private void DeleteEncryptedData(UserSecretKey key)
    {
        var filePath = GetStorageFilePath(key);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    /// <summary>
    /// Gets the file path for storing encrypted data for the given key.
    /// </summary>
    private static string GetStorageFilePath(UserSecretKey key)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var safeKey = SanitizeFileName(key.Value);
        return System.IO.Path.Combine(appData, "Node.Net", "Secrets", $"{safeKey}.dat");
    }

    /// <summary>
    /// Sanitizes a key value for use as a filename.
    /// </summary>
    private static string SanitizeFileName(string key)
    {
        var invalidChars = System.IO.Path.GetInvalidFileNameChars();
        foreach (var c in invalidChars)
        {
            key = key.Replace(c, '_');
        }
        return key;
    }
}
#endif
