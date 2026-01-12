using System;

namespace Node.Net.Service.User;

/// <summary>
/// Interface for accessing local system user information
/// </summary>
public interface ISystemUser
{
    /// <summary>
    /// Gets the system user's profile picture as a JPEG byte array at the specified size
    /// </summary>
    /// <param name="width">The target width in pixels</param>
    /// <param name="height">The target height in pixels</param>
    /// <returns>A byte array containing JPEG image data. If the profile picture cannot be retrieved, returns an anonymous profile picture. The image will be resized to fit within the specified dimensions while maintaining aspect ratio. Returns null only if width or height is invalid (less than or equal to 0).</returns>
    byte[]? GetProfilePicture(int width, int height);
}
