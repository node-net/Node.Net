using System.Runtime.InteropServices;

namespace Node.Net.Security;

/// <summary>
/// Factory for creating platform-specific implementations of IUserSecretProvider.
/// </summary>
public static class UserSecretProviderFactory
{
    /// <summary>
    /// Creates an appropriate IUserSecretProvider implementation based on the current platform.
    /// </summary>
    /// <returns>An IUserSecretProvider implementation for the current platform.</returns>
    public static IUserSecretProvider Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
#if IS_WINDOWS
            return new Windows.UserSecretProvider();
#else
            // Fallback for Windows when not compiled with IS_WINDOWS
            return new Fallback.UserSecretProvider();
#endif
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            // macOS Keychain integration not yet implemented - use fallback provider
            // TODO: Switch to MacOS.UserSecretProvider() once Keychain integration is complete
            return new Fallback.UserSecretProvider();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Check if Android (Linux-based)
            if (IsAndroid())
            {
                return new Android.UserSecretProvider();
            }
            return new Linux.UserSecretProvider();
        }
        else
        {
            // Unknown platform - use fallback
            return new Fallback.UserSecretProvider();
        }
    }

    /// <summary>
    /// Determines if the current platform is Android.
    /// </summary>
    /// <returns>True if running on Android, false otherwise.</returns>
    private static bool IsAndroid()
    {
        // Android detection: Check for Android-specific environment variables or system properties
        // This is a simplified check - in production, you might want more robust detection
        var osDescription = RuntimeInformation.OSDescription;
#if IS_FRAMEWORK
        return osDescription.IndexOf("Android", System.StringComparison.OrdinalIgnoreCase) >= 0;
#else
        return osDescription.Contains("Android", System.StringComparison.OrdinalIgnoreCase);
#endif
    }
}
