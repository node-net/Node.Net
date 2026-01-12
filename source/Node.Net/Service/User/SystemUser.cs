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
    /// <returns>A byte array containing JPEG image data. If the profile picture cannot be retrieved, returns an anonymous profile picture. The image will be resized to fit within the specified dimensions while maintaining aspect ratio.</returns>
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
            if (!string.IsNullOrEmpty(dataUrl))
            {
                // Extract image bytes from data URL
                var imageBytes = ExtractBytesFromDataUrl(dataUrl!);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    // Resize image to specified dimensions
                    return ResizeImage(imageBytes, width, height);
                }
            }

            // If profile picture is not available, return anonymous profile picture
            return GenerateAnonymousProfilePicture(width, height);
        }
        catch
        {
            // On failure, return anonymous profile picture as fallback
            return GenerateAnonymousProfilePicture(width, height);
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

    /// <summary>
    /// Generates an anonymous profile picture (generic user icon)
    /// </summary>
    /// <param name="width">The target width in pixels</param>
    /// <param name="height">The target height in pixels</param>
    /// <returns>A byte array containing JPEG image data of an anonymous profile picture</returns>
    private byte[] GenerateAnonymousProfilePicture(int width, int height)
    {
        try
        {
#pragma warning disable CA1416 // System.Drawing.Common is cross-platform, analyzer warning is false positive
            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);

            // Use high-quality settings
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            // Fill with light gray background
            graphics.Clear(Color.FromArgb(240, 240, 240));

            // Calculate circle dimensions (slightly smaller than image to add padding)
            int size = Math.Min(width, height);
            int padding = size / 10; // 10% padding
            int circleSize = size - (padding * 2);
            int x = (width - circleSize) / 2;
            int y = (height - circleSize) / 2;

            // Draw circular background (medium gray)
            using (var brush = new SolidBrush(Color.FromArgb(180, 180, 180)))
            {
                graphics.FillEllipse(brush, x, y, circleSize, circleSize);
            }

            // Draw person silhouette icon (white)
            using (var pen = new Pen(Color.White, circleSize / 8))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;

                // Head (circle)
                int headSize = circleSize / 3;
                int headX = x + (circleSize - headSize) / 2;
                int headY = y + circleSize / 4;
                graphics.DrawEllipse(pen, headX, headY, headSize, headSize);

                // Body (rounded rectangle/trapezoid shape)
                int bodyWidth = circleSize / 2;
                int bodyHeight = circleSize / 2;
                int bodyX = x + (circleSize - bodyWidth) / 2;
                int bodyY = headY + headSize + circleSize / 20;
                
                // Draw body as a rounded rectangle
                var bodyRect = new Rectangle(bodyX, bodyY, bodyWidth, bodyHeight);
                int cornerRadius = bodyWidth / 4;
                FillRoundedRectangle(graphics, bodyRect, cornerRadius);
            }

            // Save as JPEG to memory stream
            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream.ToArray();
#pragma warning restore CA1416
        }
        catch
        {
            // If generation fails, return a minimal valid JPEG (1x1 pixel)
            return GenerateMinimalJpeg(width, height);
        }
    }

    /// <summary>
    /// Helper method to draw a rounded rectangle
    /// </summary>
    private void FillRoundedRectangle(Graphics graphics, Rectangle rect, int radius)
    {
#pragma warning disable CA1416 // System.Drawing.Common is cross-platform, analyzer warning is false positive
        using var path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
        path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
        path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
        path.CloseFigure();
        using var brush = new SolidBrush(Color.White);
        graphics.FillPath(brush, path);
#pragma warning restore CA1416
    }

    /// <summary>
    /// Generates a minimal valid JPEG as a last resort fallback
    /// </summary>
    private byte[] GenerateMinimalJpeg(int width, int height)
    {
        try
        {
#pragma warning disable CA1416 // System.Drawing.Common is cross-platform, analyzer warning is false positive
            using var bitmap = new Bitmap(Math.Max(1, width), Math.Max(1, height));
            bitmap.SetPixel(0, 0, Color.LightGray);
            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream.ToArray();
#pragma warning restore CA1416
        }
        catch
        {
            // Ultimate fallback: return a minimal JPEG header (this should never happen)
            return new byte[] { 0xFF, 0xD8, 0xFF, 0xD9 };
        }
    }
}
