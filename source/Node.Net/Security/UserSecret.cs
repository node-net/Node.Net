using System;

namespace Node.Net.Security;

/// <summary>
/// Represents a stored secret with metadata.
/// </summary>
/// <param name="Base64">Base64-encoded secret (384 bits / 48 bytes).</param>
/// <param name="CreatedUtc">Creation timestamp in UTC.</param>
/// <param name="ByteLength">Length of secret in bytes (always 48).</param>
public sealed record UserSecret(
    string Base64,
    DateTimeOffset CreatedUtc,
    int ByteLength)
{
    /// <summary>
    /// Converts the Base64-encoded secret to a byte array.
    /// </summary>
    /// <returns>The secret as a byte array (48 bytes).</returns>
    /// <exception cref="FormatException">Thrown when Base64 string is invalid.</exception>
    public byte[] GetBytes() => Convert.FromBase64String(Base64);

    /// <summary>
    /// Returns a string representation that does not expose secret material.
    /// </summary>
    /// <returns>A safe string representation showing only metadata.</returns>
    public override string ToString() => $"UserSecret(CreatedUtc={CreatedUtc:O}, ByteLength={ByteLength})";
}
