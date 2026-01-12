using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Node.Net.Service.User;

/// <summary>
/// Implementation of ISystemUser for accessing local system user profile picture
/// </summary>
public class SystemUser : ISystemUser
{
    private readonly OsUserProfileService _profileService;

    /// <summary>
    /// Initializes a new instance of the SystemUser class
    /// </summary>
    public SystemUser()
    {
        _profileService = new OsUserProfileService();
    }

    /// <summary>
    /// Gets the system user's profile picture as a JPEG byte array at the specified size
    /// </summary>
    /// <param name="width">The target width in pixels</param>
    /// <param name="height">The target height in pixels</param>
    /// <returns>A byte array containing JPEG image data, or null if the profile picture cannot be retrieved. The image will be resized to fit within the specified dimensions while maintaining aspect ratio.</returns>
    public byte[]? GetProfilePicture(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            return null;
        }

        try
        {
            // Get profile picture data URL from OsUserProfileService
            var dataUrl = _profileService.GetUserProfilePicture();
            if (string.IsNullOrEmpty(dataUrl))
            {
                return null;
            }

            // Extract image bytes from data URL
            var imageBytes = ExtractBytesFromDataUrl(dataUrl!);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            // Resize image to specified dimensions
            return ResizeImage(imageBytes, width, height);
        }
        catch
        {
            // Return null on any failure (per spec requirement)
            return null;
        }
    }

    /// <summary>
    /// Extracts image bytes from a data URL string
    /// </summary>
    /// <param name="dataUrl">The data URL string (format: data:image/...;base64,{base64data})</param>
    /// <returns>The decoded byte array, or null if parsing fails</returns>
    private byte[]? ExtractBytesFromDataUrl(string dataUrl)
    {
        if (string.IsNullOrEmpty(dataUrl))
        {
            return null;
        }

        const string base64Prefix = "base64,";
        int base64Index = dataUrl.IndexOf(base64Prefix, StringComparison.Ordinal);
        if (base64Index < 0)
        {
            return null;
        }

        try
        {
            string base64Data = dataUrl.Substring(base64Index + base64Prefix.Length);
            return Convert.FromBase64String(base64Data);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Calculates new dimensions that fit within target dimensions while maintaining aspect ratio
    /// </summary>
    /// <param name="originalWidth">The original image width</param>
    /// <param name="originalHeight">The original image height</param>
    /// <param name="targetWidth">The target width</param>
    /// <param name="targetHeight">The target height</param>
    /// <returns>A tuple containing the new width and height</returns>
    private (int newWidth, int newHeight) CalculateDimensions(
        int originalWidth, int originalHeight,
        int targetWidth, int targetHeight)
    {
        if (originalWidth <= 0 || originalHeight <= 0)
        {
            return (targetWidth, targetHeight);
        }

        double scaleX = (double)targetWidth / originalWidth;
        double scaleY = (double)targetHeight / originalHeight;
        double scale = Math.Min(scaleX, scaleY); // Fit within, maintain aspect ratio

        return (
            (int)(originalWidth * scale),
            (int)(originalHeight * scale)
        );
    }

    /// <summary>
    /// Resizes an image to the specified dimensions while maintaining aspect ratio and converts to JPEG format
    /// </summary>
    /// <param name="imageBytes">The original image bytes</param>
    /// <param name="width">The target width</param>
    /// <param name="height">The target height</param>
    /// <returns>The resized image as JPEG byte array, or null if resizing fails</returns>
    private byte[]? ResizeImage(byte[] imageBytes, int width, int height)
    {
        if (imageBytes == null || imageBytes.Length == 0)
        {
            return null;
        }

        try
        {
#pragma warning disable CA1416 // System.Drawing.Common is cross-platform, analyzer warning is false positive
            using var originalImageStream = new MemoryStream(imageBytes);
            using var originalImage = Image.FromStream(originalImageStream);

            var (newWidth, newHeight) = CalculateDimensions(
                originalImage.Width, originalImage.Height, width, height);

            using var resizedBitmap = new Bitmap(newWidth, newHeight);
            using var graphics = Graphics.FromImage(resizedBitmap);

            // Use high-quality settings for better image quality
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            // Draw resized image
            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);

            // Save as JPEG to memory stream
            using var memoryStream = new MemoryStream();
            resizedBitmap.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream.ToArray();
#pragma warning restore CA1416
        }
        catch
        {
            return null;
        }
    }
}
