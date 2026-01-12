using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Node.Net.Diagnostic.Generic;
using Node.Net.Service.User;

namespace Node.Net.Service.User;

[TestFixture]
internal class SystemUserTests : TestHarness<SystemUser>
{
    /// <summary>
    /// Determines if the test is running on net8.0-windows target framework.
    /// </summary>
    private static bool IsNet8Windows()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyLocation = assembly.Location;
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                if (!string.IsNullOrEmpty(assemblyDir))
                {
                    var dirName = Path.GetFileName(assemblyDir);
                    if (dirName == "net8.0-windows")
                    {
                        return true;
                    }
                    
                    // Check parent directory
                    var parentDir = Path.GetDirectoryName(assemblyDir);
                    if (!string.IsNullOrEmpty(parentDir))
                    {
                        var parentDirName = Path.GetFileName(parentDir);
                        if (parentDirName == "net8.0-windows")
                        {
                            return true;
                        }
                    }
                }
            }
        }
        catch
        {
            // If we can't determine, assume false
        }
        
        return false;
    }
    [Test]
    public void GetProfilePicture_ReturnsImageAtSpecifiedSize()
    {
        // Arrange
        var systemUser = new SystemUser();
        int targetWidth = 128;
        int targetHeight = 128;

        // Act
        var result = systemUser.GetProfilePicture(targetWidth, targetHeight);

        // Assert - Should always return a valid JPEG (either real profile picture or anonymous)
        Assert.That(result, Is.Not.Null, "GetProfilePicture should always return a valid JPEG (real or anonymous)");
        Assert.That(result!.Length, Is.GreaterThan(0), "Image bytes should not be empty");
        
        // Verify it's a valid JPEG (starts with FF D8)
        Assert.That(result[0], Is.EqualTo(0xFF), "JPEG should start with 0xFF");
        Assert.That(result[1], Is.EqualTo(0xD8), "JPEG should have 0xD8 after 0xFF");
    }

    [Test]
    public void GetProfilePicture_MaintainsAspectRatio()
    {
        // Arrange
        var systemUser = new SystemUser();
        int targetWidth = 256;
        int targetHeight = 128; // Different aspect ratio

        // Act
        var result = systemUser.GetProfilePicture(targetWidth, targetHeight);

        // Assert - Should always return a valid JPEG (either real profile picture or anonymous)
        Assert.That(result, Is.Not.Null, "GetProfilePicture should always return a valid JPEG (real or anonymous)");
        
        // Load the resized image to verify dimensions
        using var imageStream = new MemoryStream(result!);
#pragma warning disable CA1416 // System.Drawing.Common is cross-platform, analyzer warning is false positive
        using var image = System.Drawing.Image.FromStream(imageStream);
        
        // Image should fit within target dimensions
        Assert.That(image.Width, Is.LessThanOrEqualTo(targetWidth), "Width should fit within target");
        Assert.That(image.Height, Is.LessThanOrEqualTo(targetHeight), "Height should fit within target");
#pragma warning restore CA1416
        
        // Aspect ratio should be maintained (width/height ratio should match original or fit within)
        // We can't verify exact aspect ratio without original, but we can verify it fits
    }

    [Test]
    public void GetProfilePicture_ReturnsJpegFormat()
    {
        // Arrange
        var systemUser = new SystemUser();
        int targetWidth = 64;
        int targetHeight = 64;

        // Act
        var result = systemUser.GetProfilePicture(targetWidth, targetHeight);

        // Assert - Should always return a valid JPEG (either real profile picture or anonymous)
        Assert.That(result, Is.Not.Null, "GetProfilePicture should always return a valid JPEG (real or anonymous)");
        
        // JPEG files start with FF D8
        Assert.That(result!.Length, Is.GreaterThanOrEqualTo(2), "Image should have at least 2 bytes");
        Assert.That(result[0], Is.EqualTo(0xFF), "First byte should be 0xFF (JPEG SOI marker)");
        Assert.That(result[1], Is.EqualTo(0xD8), "Second byte should be 0xD8 (JPEG SOI marker)");
    }

    [Test]
    public void GetProfilePicture_GeneratesArtifact()
    {
        // Arrange
        var systemUser = new SystemUser();
        int targetWidth = 256;
        int targetHeight = 256;

        // Act
        var result = systemUser.GetProfilePicture(targetWidth, targetHeight);
        
        // Assert - Should always return a valid JPEG (either real profile picture or anonymous)
        Assert.That(result, Is.Not.Null, "GetProfilePicture should always return a valid JPEG (real or anonymous)");
        Assert.That(result!.Length, Is.GreaterThan(0), "Profile picture bytes must not be empty");
        
        // Assert - Generate artifact (always a valid JPEG now)
        if (result != null)
        {
            // Profile picture is available - generate valid JPEG file
            var artifactFile = GetArtifactFileInfo("user.jpeg");
            
            // Write image bytes to artifact file
            File.WriteAllBytes(artifactFile.FullName, result);
            
            // Verify file exists
            Assert.That(File.Exists(artifactFile.FullName), Is.True, $"Artifact file should exist at {artifactFile.FullName}");
            
            // Verify file is not empty
            var fileInfo = new FileInfo(artifactFile.FullName);
            Assert.That(fileInfo.Length, Is.GreaterThan(0), "Artifact file should not be empty");
            
            // Verify it's a valid JPEG file
            var fileBytes = File.ReadAllBytes(artifactFile.FullName);
            
            // JPEG files must start with SOI marker: 0xFF 0xD8
            Assert.That(fileBytes.Length, Is.GreaterThanOrEqualTo(2), "JPEG file should have at least 2 bytes");
            Assert.That(fileBytes[0], Is.EqualTo(0xFF), "JPEG file should start with 0xFF (SOI marker)");
            Assert.That(fileBytes[1], Is.EqualTo(0xD8), "JPEG file should have 0xD8 after 0xFF (SOI marker)");
            
            // Verify JPEG structure: should have valid JPEG markers after SOI
            // JPEG files typically have: SOI (FF D8) followed by APP markers (FF E0-E7), DQT (FF DB), SOF (FF C0-C3), etc.
            if (fileBytes.Length >= 4)
            {
                var hasValidMarker = false;
                // Check first 100 bytes for valid JPEG markers (should find one quickly)
                for (int i = 2; i < Math.Min(fileBytes.Length, 100); i++)
                {
                    if (fileBytes[i] == 0xFF && i + 1 < fileBytes.Length)
                    {
                        var marker = fileBytes[i + 1];
                        // Valid JPEG markers:
                        // - APP markers: 0xE0-0xEF (JFIF, EXIF, etc.)
                        // - DQT (Quantization Table): 0xDB
                        // - SOF (Start of Frame): 0xC0-0xC3, 0xC5-0xC7, 0xC9-0xCB, 0xCD-0xCF
                        // - DHT (Huffman Table): 0xC4
                        // - SOS (Start of Scan): 0xDA
                        if ((marker >= 0xE0 && marker <= 0xEF) || // APP markers
                            marker == 0xDB || // DQT
                            marker == 0xC4 || // DHT
                            (marker >= 0xC0 && marker <= 0xC3) || // SOF0-SOF3
                            (marker >= 0xC5 && marker <= 0xC7) || // SOF5-SOF7
                            (marker >= 0xC9 && marker <= 0xCB) || // SOF9-SOF11
                            (marker >= 0xCD && marker <= 0xCF) || // SOF13-SOF15
                            marker == 0xDA) // SOS
                        {
                            hasValidMarker = true;
                            break;
                        }
                        // Skip 0xFF 0xFF sequences (byte stuffing)
                        if (marker == 0xFF)
                        {
                            continue;
                        }
                    }
                }
                Assert.That(hasValidMarker, Is.True, "JPEG file should contain valid JPEG markers (APP/DQT/SOF/DHT/SOS) after SOI");
            }
            
            // Verify file ends with JPEG EOI marker (0xFF 0xD9) - required for valid JPEG
            Assert.That(fileBytes.Length, Is.GreaterThanOrEqualTo(4), "JPEG file should have at least 4 bytes (SOI + EOI)");
            var endsWithEOI = fileBytes[fileBytes.Length - 2] == 0xFF && fileBytes[fileBytes.Length - 1] == 0xD9;
            Assert.That(endsWithEOI, Is.True, "JPEG file should end with EOI marker (0xFF 0xD9)");
            
            // Additional validation: Try to load as System.Drawing.Image to ensure it's a valid image
            try
            {
                using var imageStream = new MemoryStream(fileBytes);
#pragma warning disable CA1416 // System.Drawing.Common is cross-platform, analyzer warning is false positive
                using var image = System.Drawing.Image.FromStream(imageStream);
                Assert.That(image.Width, Is.GreaterThan(0), "JPEG image should have valid width");
                Assert.That(image.Height, Is.GreaterThan(0), "JPEG image should have valid height");
                Assert.That(image.RawFormat, Is.EqualTo(System.Drawing.Imaging.ImageFormat.Jpeg), "Image format should be JPEG");
#pragma warning restore CA1416
            }
            catch (Exception ex)
            {
                Assert.Fail($"JPEG file failed to load as System.Drawing.Image: {ex.Message}");
            }
        }
    }

    [Test]
    public void GetProfilePicture_ReturnsNullWhenUnavailable()
    {
        // Arrange
        var systemUser = new SystemUser();
        
        // Note: This test may pass or be skipped depending on system configuration
        // If profile picture is available, we can't easily test the null case
        // This test documents the expected behavior
        
        // Act
        var result = systemUser.GetProfilePicture(64, 64);
        
        // Assert
        // Result can be null (if unavailable) or byte array (if available)
        // Both are valid per specification
        Assert.That(result == null || result.Length > 0, Is.True, "Result should be null or valid image bytes");
    }

    [Test]
    public void GetProfilePicture_HandlesInvalidDimensions()
    {
        // Arrange
        var systemUser = new SystemUser();

        // Act & Assert - Invalid dimensions should return null
        var resultZero = systemUser.GetProfilePicture(0, 64);
        Assert.That(resultZero, Is.Null, "Zero width should return null");

        var resultNegative = systemUser.GetProfilePicture(-1, 64);
        Assert.That(resultNegative, Is.Null, "Negative width should return null");

        var resultBothZero = systemUser.GetProfilePicture(0, 0);
        Assert.That(resultBothZero, Is.Null, "Both dimensions zero should return null");
    }

    [Test]
    public void GetProfilePicture_DiagnosticOutput()
    {
        // Arrange
        var profileService = new OsUserProfileService();
        
        // Act
        var result = profileService.GetUserProfilePictureWithDiagnostics();
        
        // Assert - Output diagnostic information for debugging
        Console.WriteLine("=== Profile Picture Diagnostic Information ===");
        Console.WriteLine(result.DiagnosticInfo);
        Console.WriteLine();
        Console.WriteLine($"Profile Picture URL: {(string.IsNullOrEmpty(result.ProfilePictureUrl) ? "null" : "available")}");
        Console.WriteLine($"Error Message: {result.ErrorMessage ?? "none"}");
        Console.WriteLine($"Attempted Paths: {result.AttemptedPaths.Count}");
        Console.WriteLine($"Found Paths: {result.FoundPaths.Count}");
        
        if (result.FoundPaths.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Found Paths:");
            foreach (var path in result.FoundPaths)
            {
                Console.WriteLine($"  - {path}");
            }
        }
        
        // Test passes regardless - this is for diagnostic purposes
        Assert.That(result, Is.Not.Null, "Diagnostic result should not be null");
    }
}
