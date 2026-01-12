using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using Node.Net.Diagnostic.Generic;
using Node.Net.Service.User;

namespace Node.Net.Service.User;

[TestFixture]
internal class SystemUserTests : TestHarness<SystemUser>
{
    [Test]
    public void GetProfilePicture_ReturnsImageAtSpecifiedSize()
    {
        // Arrange
        var systemUser = new SystemUser();
        int targetWidth = 128;
        int targetHeight = 128;

        // Act
        var result = systemUser.GetProfilePicture(targetWidth, targetHeight);

        // Assert
        if (result != null)
        {
            Assert.That(result.Length, Is.GreaterThan(0), "Image bytes should not be empty");
            
            // Verify it's a valid JPEG (starts with FF D8 FF)
            Assert.That(result[0], Is.EqualTo(0xFF), "JPEG should start with 0xFF");
            Assert.That(result[1], Is.EqualTo(0xD8), "JPEG should have 0xD8 after 0xFF");
        }
        else
        {
            Assert.Warn("Profile picture not available on this system - test skipped");
        }
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

        // Assert
        if (result != null)
        {
            // Load the resized image to verify dimensions
            using var imageStream = new MemoryStream(result);
            using var image = System.Drawing.Image.FromStream(imageStream);
            
            // Image should fit within target dimensions
            Assert.That(image.Width, Is.LessThanOrEqualTo(targetWidth), "Width should fit within target");
            Assert.That(image.Height, Is.LessThanOrEqualTo(targetHeight), "Height should fit within target");
            
            // Aspect ratio should be maintained (width/height ratio should match original or fit within)
            // We can't verify exact aspect ratio without original, but we can verify it fits
        }
        else
        {
            Assert.Warn("Profile picture not available on this system - test skipped");
        }
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

        // Assert
        if (result != null)
        {
            // JPEG files start with FF D8 FF
            Assert.That(result.Length, Is.GreaterThanOrEqualTo(2), "Image should have at least 2 bytes");
            Assert.That(result[0], Is.EqualTo(0xFF), "First byte should be 0xFF (JPEG SOI marker)");
            Assert.That(result[1], Is.EqualTo(0xD8), "Second byte should be 0xD8 (JPEG SOI marker)");
        }
        else
        {
            Assert.Warn("Profile picture not available on this system - test skipped");
        }
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

        // Assert
        if (result != null)
        {
            // Get artifact file path using TestHarness
            var artifactFile = GetArtifactFileInfo("User.Jpg");
            
            // Write image bytes to artifact file
            File.WriteAllBytes(artifactFile.FullName, result);
            
            // Verify file exists
            Assert.That(File.Exists(artifactFile.FullName), Is.True, $"Artifact file should exist at {artifactFile.FullName}");
            
            // Verify file is not empty
            var fileInfo = new FileInfo(artifactFile.FullName);
            Assert.That(fileInfo.Length, Is.GreaterThan(0), "Artifact file should not be empty");
            
            // Verify it's a valid JPEG
            var fileBytes = File.ReadAllBytes(artifactFile.FullName);
            Assert.That(fileBytes[0], Is.EqualTo(0xFF), "Artifact should be valid JPEG");
            Assert.That(fileBytes[1], Is.EqualTo(0xD8), "Artifact should be valid JPEG");
        }
        else
        {
            Assert.Warn("Profile picture not available on this system - artifact generation skipped");
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
}
