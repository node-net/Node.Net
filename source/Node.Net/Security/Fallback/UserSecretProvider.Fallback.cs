using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Node.Net.Security;

namespace Node.Net.Security.Fallback;

/// <summary>
/// Fallback implementation of IUserSecretProvider using encrypted file storage.
/// Uses AES-256-GCM on .NET Core/8.0, AES-256-CBC with HMAC-SHA256 on .NET Framework 4.8.
/// Used when OS security services are permanently unavailable.
/// </summary>
public class UserSecretProvider : Node.Net.Security.UserSecretProvider
{
    private const int KeyDerivationIterations = 100000;

    /// <summary>
    /// Attempts to retrieve an existing secret for the given key from encrypted file storage.
    /// </summary>
    protected override ValueTask<UserSecret?> TryRetrieveSecretAsync(
        UserSecretKey key,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var filePath = GetStorageFilePath(key);
        if (!File.Exists(filePath))
        {
            return new ValueTask<UserSecret?>((UserSecret?)null);
        }

        try
        {
            var encryptedData = File.ReadAllBytes(filePath);
            if (encryptedData.Length < 12) // Nonce (12 bytes) minimum
            {
                throw new InvalidOperationException("Stored secret data is corrupted.",
                    new FormatException("Invalid data format in stored secret."));
            }

#if IS_FRAMEWORK
            // .NET Framework 4.8: Use AES-CBC with HMAC
            return new ValueTask<UserSecret?>(DecryptAesCbc(encryptedData, key));
#else
            // .NET Core/8.0: Use AES-GCM
            // Extract nonce (12 bytes), ciphertext, and tag (16 bytes)
            var nonce = new byte[12];
            Array.Copy(encryptedData, 0, nonce, 0, 12);
            
            var tag = new byte[16];
            Array.Copy(encryptedData, encryptedData.Length - 16, tag, 0, 16);
            
            var ciphertext = new byte[encryptedData.Length - 12 - 16];
            Array.Copy(encryptedData, 12, ciphertext, 0, ciphertext.Length);

            // Derive key from user credentials
            var keyBytes = DeriveKey(key);

            // Decrypt
            using var aes = new AesGcm(keyBytes, 16); // Explicitly specify 16-byte tag size
            var decryptedData = new byte[ciphertext.Length];
            aes.Decrypt(nonce, ciphertext, tag, decryptedData, null);

            // Parse the stored data: format is "Base64|CreatedUtcTicks"
            var dataString = Encoding.UTF8.GetString(decryptedData);
            var parts = dataString.Split('|');
            if (parts.Length != 2)
            {
                throw new InvalidOperationException("Stored secret data is corrupted.",
                    new FormatException("Invalid data format in stored secret."));
            }

            var base64 = parts[0];
            var createdTicks = long.Parse(parts[1]);
            var createdUtc = new DateTimeOffset(createdTicks, TimeSpan.Zero);

            return new ValueTask<UserSecret?>(new UserSecret(base64, createdUtc, 48));
#endif
        }
        catch (CryptographicException ex)
        {
            throw new InvalidOperationException("Failed to retrieve secret from storage.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new System.Security.SecurityException("Access to secret storage is denied.", ex);
        }
    }

    /// <summary>
    /// Stores a secret for the given key in encrypted file storage.
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
            var dataBytes = Encoding.UTF8.GetBytes(dataString);

            // Derive key from user credentials
            var keyBytes = DeriveKey(key);

#if IS_FRAMEWORK
            // .NET Framework 4.8: Use AES-CBC with HMAC
            var encryptedData = EncryptAesCbc(dataBytes, keyBytes);
#else
            // .NET Core/8.0: Use AES-GCM
            // Generate nonce
            var nonce = new byte[12];
            RandomNumberGenerator.Fill(nonce);

            // Encrypt
            using var aes = new AesGcm(keyBytes, 16); // Explicitly specify 16-byte tag size
            var ciphertext = new byte[dataBytes.Length];
            var tag = new byte[16];
            aes.Encrypt(nonce, dataBytes, ciphertext, tag, null);

            // Combine nonce + ciphertext + tag
            var encryptedData = new byte[12 + ciphertext.Length + 16];
            Array.Copy(nonce, 0, encryptedData, 0, 12);
            Array.Copy(ciphertext, 0, encryptedData, 12, ciphertext.Length);
            Array.Copy(tag, 0, encryptedData, 12 + ciphertext.Length, 16);
#endif

            // Write to file
            var filePath = GetStorageFilePath(key);
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllBytes(filePath, encryptedData);

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
        // For file storage, replacement is the same as storage (atomic write)
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
            var filePath = GetStorageFilePath(key);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return default(ValueTask);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new System.Security.SecurityException("Access to secret storage is denied.", ex);
        }
    }

#if IS_FRAMEWORK
    /// <summary>
    /// Encrypts data using AES-256-CBC with HMAC-SHA256 (for .NET Framework 4.8 compatibility).
    /// </summary>
    private byte[] EncryptAesCbc(byte[] data, byte[] key)
    {
        // Split key: first 32 bytes for AES, last 32 bytes for HMAC
        var aesKey = new byte[32];
        var hmacKey = new byte[32];
        Array.Copy(key, 0, aesKey, 0, 32);
        Array.Copy(key, 32, hmacKey, 0, 32);

        // Generate IV
        var iv = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(iv);
        }

        // Encrypt with AES-CBC
        byte[] ciphertext;
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Key = aesKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var encryptor = aes.CreateEncryptor())
            {
                ciphertext = encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        // Compute HMAC
        byte[] hmac;
        using (var hmacAlg = new HMACSHA256(hmacKey))
        {
            // HMAC over IV + ciphertext
            var toHash = new byte[iv.Length + ciphertext.Length];
            Array.Copy(iv, 0, toHash, 0, iv.Length);
            Array.Copy(ciphertext, 0, toHash, iv.Length, ciphertext.Length);
            hmac = hmacAlg.ComputeHash(toHash);
        }

        // Combine: IV (16) + ciphertext + HMAC (32)
        var result = new byte[16 + ciphertext.Length + 32];
        Array.Copy(iv, 0, result, 0, 16);
        Array.Copy(ciphertext, 0, result, 16, ciphertext.Length);
        Array.Copy(hmac, 0, result, 16 + ciphertext.Length, 32);

        return result;
    }

    /// <summary>
    /// Decrypts data using AES-256-CBC with HMAC-SHA256 (for .NET Framework 4.8 compatibility).
    /// </summary>
    private UserSecret DecryptAesCbc(byte[] encryptedData, UserSecretKey key)
    {
        if (encryptedData.Length < 48) // IV (16) + HMAC (32) minimum
        {
            throw new InvalidOperationException("Stored secret data is corrupted.",
                new FormatException("Invalid data format in stored secret."));
        }

        // Extract IV, ciphertext, and HMAC
        var iv = new byte[16];
        Array.Copy(encryptedData, 0, iv, 0, 16);

        var hmac = new byte[32];
        Array.Copy(encryptedData, encryptedData.Length - 32, hmac, 0, 32);

        var ciphertext = new byte[encryptedData.Length - 16 - 32];
        Array.Copy(encryptedData, 16, ciphertext, 0, ciphertext.Length);

        // Derive key
        var keyBytes = DeriveKey(key);
        var aesKey = new byte[32];
        var hmacKey = new byte[32];
        Array.Copy(keyBytes, 0, aesKey, 0, 32);
        Array.Copy(keyBytes, 32, hmacKey, 0, 32);

        // Verify HMAC
        using (var hmacAlg = new HMACSHA256(hmacKey))
        {
            var toHash = new byte[iv.Length + ciphertext.Length];
            Array.Copy(iv, 0, toHash, 0, iv.Length);
            Array.Copy(ciphertext, 0, toHash, iv.Length, ciphertext.Length);
            var computedHmac = hmacAlg.ComputeHash(toHash);

            if (!ByteArraysEqual(hmac, computedHmac))
            {
                throw new InvalidOperationException("Stored secret data is corrupted.",
                    new CryptographicException("HMAC verification failed."));
            }
        }

        // Decrypt
        byte[] decryptedData;
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Key = aesKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var decryptor = aes.CreateDecryptor())
            {
                decryptedData = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            }
        }

        // Parse the stored data: format is "Base64|CreatedUtcTicks"
        var dataString = Encoding.UTF8.GetString(decryptedData);
        var parts = dataString.Split('|');
        if (parts.Length != 2)
        {
            throw new InvalidOperationException("Stored secret data is corrupted.",
                new FormatException("Invalid data format in stored secret."));
        }

        var base64 = parts[0];
        var createdTicks = long.Parse(parts[1]);
        var createdUtc = new DateTimeOffset(createdTicks, TimeSpan.Zero);

        return new UserSecret(base64, createdUtc, 48);
    }

    /// <summary>
    /// Constant-time comparison of byte arrays to prevent timing attacks.
    /// </summary>
    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        var result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }

        return result == 0;
    }
#endif

    /// <summary>
    /// Derives an encryption key from user credentials using PBKDF2.
    /// </summary>
    private static byte[] DeriveKey(UserSecretKey key)
    {
        // Use user context (not machine identifiers per FR-008)
        var salt = Encoding.UTF8.GetBytes($"Node.Net.UserSecretProvider:{key.Value}:{Environment.UserName}@{Environment.UserDomainName}");
        var password = $"{Environment.UserName}@{Environment.UserDomainName}:{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}";

        using var pbkdf2 = new Rfc2898DeriveBytes(
            Encoding.UTF8.GetBytes(password),
            salt,
            KeyDerivationIterations,
            HashAlgorithmName.SHA256);

#if IS_FRAMEWORK
        // For .NET Framework, derive 64 bytes (32 for AES, 32 for HMAC)
        return pbkdf2.GetBytes(64);
#else
        // For .NET Core/8.0, derive 32 bytes (AES-256-GCM uses single key)
        return pbkdf2.GetBytes(32);
#endif
    }

    /// <summary>
    /// Gets the file path for storing encrypted data for the given key.
    /// </summary>
    private static string GetStorageFilePath(UserSecretKey key)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var safeKey = SanitizeFileName(key.Value);
        return Path.Combine(appData, "Node.Net", "Secrets", "Fallback", $"{safeKey}.dat");
    }

    /// <summary>
    /// Sanitizes a key value for use as a filename.
    /// </summary>
    private static string SanitizeFileName(string key)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        foreach (var c in invalidChars)
        {
            key = key.Replace(c, '_');
        }
        return key;
    }
}
