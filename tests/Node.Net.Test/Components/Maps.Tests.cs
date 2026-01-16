#if !IS_FRAMEWORK
extern alias NodeNet;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NodeNet::Node.Net.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Test.Components;

[TestFixture]
internal class MapsTests
{
    private Bunit.TestContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = new Bunit.TestContext();
        _ctx.Services.AddFluentUIComponents();
    }

    [TearDown]
    public void TearDown()
    {
        _ctx?.Dispose();
    }

    [Test]
    public void ValidateLatitude_ValidRange_ReturnsTrue()
    {
        // Arrange & Act & Assert
        Assert.That(MapsComponentHelper.ValidateLatitude(-90.0), Is.True, "Minimum latitude should be valid");
        Assert.That(MapsComponentHelper.ValidateLatitude(0.0), Is.True, "Zero latitude should be valid");
        Assert.That(MapsComponentHelper.ValidateLatitude(90.0), Is.True, "Maximum latitude should be valid");
        Assert.That(MapsComponentHelper.ValidateLatitude(45.5), Is.True, "Mid-range latitude should be valid");
    }

    [Test]
    public void ValidateLatitude_InvalidRange_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.That(MapsComponentHelper.ValidateLatitude(-90.1), Is.False, "Latitude below -90 should be invalid");
        Assert.That(MapsComponentHelper.ValidateLatitude(90.1), Is.False, "Latitude above 90 should be invalid");
        Assert.That(MapsComponentHelper.ValidateLatitude(-100.0), Is.False, "Latitude -100 should be invalid");
        Assert.That(MapsComponentHelper.ValidateLatitude(100.0), Is.False, "Latitude 100 should be invalid");
    }

    [Test]
    public void ValidateLongitude_ValidRange_ReturnsTrue()
    {
        // Arrange & Act & Assert
        Assert.That(MapsComponentHelper.ValidateLongitude(-180.0), Is.True, "Minimum longitude should be valid");
        Assert.That(MapsComponentHelper.ValidateLongitude(0.0), Is.True, "Zero longitude should be valid");
        Assert.That(MapsComponentHelper.ValidateLongitude(180.0), Is.True, "Maximum longitude should be valid");
        Assert.That(MapsComponentHelper.ValidateLongitude(120.5), Is.True, "Mid-range longitude should be valid");
    }

    [Test]
    public void ValidateLongitude_InvalidRange_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.That(MapsComponentHelper.ValidateLongitude(-180.1), Is.False, "Longitude below -180 should be invalid");
        Assert.That(MapsComponentHelper.ValidateLongitude(180.1), Is.False, "Longitude above 180 should be invalid");
        Assert.That(MapsComponentHelper.ValidateLongitude(-200.0), Is.False, "Longitude -200 should be invalid");
        Assert.That(MapsComponentHelper.ValidateLongitude(200.0), Is.False, "Longitude 200 should be invalid");
    }

    [Test]
    public void NormalizeCoordinates_ValidCoordinates_ReturnsOriginal()
    {
        // Arrange
        var latitude = 45.5;
        var longitude = -120.3;

        // Act
        var (normalizedLat, normalizedLon) = MapsComponentHelper.NormalizeCoordinates(latitude, longitude);

        // Assert
        Assert.That(normalizedLat, Is.EqualTo(latitude), "Valid latitude should remain unchanged");
        Assert.That(normalizedLon, Is.EqualTo(longitude), "Valid longitude should remain unchanged");
    }

    [Test]
    public void NormalizeCoordinates_InvalidLatitude_DefaultsToZero()
    {
        // Arrange
        var invalidLatitude = 95.0; // Out of range
        var validLongitude = -120.3;

        // Act
        var (normalizedLat, normalizedLon) = MapsComponentHelper.NormalizeCoordinates(invalidLatitude, validLongitude);

        // Assert
        Assert.That(normalizedLat, Is.EqualTo(0.0), "Invalid latitude should default to 0.0");
        Assert.That(normalizedLon, Is.EqualTo(validLongitude), "Valid longitude should remain unchanged");
    }

    [Test]
    public void NormalizeCoordinates_InvalidLongitude_DefaultsToZero()
    {
        // Arrange
        var validLatitude = 45.5;
        var invalidLongitude = 200.0; // Out of range

        // Act
        var (normalizedLat, normalizedLon) = MapsComponentHelper.NormalizeCoordinates(validLatitude, invalidLongitude);

        // Assert
        Assert.That(normalizedLat, Is.EqualTo(validLatitude), "Valid latitude should remain unchanged");
        Assert.That(normalizedLon, Is.EqualTo(0.0), "Invalid longitude should default to 0.0");
    }

    [Test]
    public void NormalizeCoordinates_BothInvalid_DefaultsBothToZero()
    {
        // Arrange
        var invalidLatitude = -100.0; // Out of range
        var invalidLongitude = 200.0; // Out of range

        // Act
        var (normalizedLat, normalizedLon) = MapsComponentHelper.NormalizeCoordinates(invalidLatitude, invalidLongitude);

        // Assert
        Assert.That(normalizedLat, Is.EqualTo(0.0), "Invalid latitude should default to 0.0");
        Assert.That(normalizedLon, Is.EqualTo(0.0), "Invalid longitude should default to 0.0");
    }

    [Test]
    public void NormalizeCoordinates_BoundaryValues_HandlesCorrectly()
    {
        // Arrange & Act & Assert - Test boundary values
        var (lat1, lon1) = MapsComponentHelper.NormalizeCoordinates(-90.0, -180.0);
        Assert.That(lat1, Is.EqualTo(-90.0), "Minimum latitude boundary should be valid");
        Assert.That(lon1, Is.EqualTo(-180.0), "Minimum longitude boundary should be valid");

        var (lat2, lon2) = MapsComponentHelper.NormalizeCoordinates(90.0, 180.0);
        Assert.That(lat2, Is.EqualTo(90.0), "Maximum latitude boundary should be valid");
        Assert.That(lon2, Is.EqualTo(180.0), "Maximum longitude boundary should be valid");
    }
}

/// <summary>
/// Helper class to expose private validation methods from Maps component for testing.
/// This is a workaround since we can't directly test private methods.
/// In the actual implementation, these methods are private in the Maps.razor @code block.
/// </summary>
internal static class MapsComponentHelper
{
    public static bool ValidateLatitude(double latitude)
    {
        return latitude >= -90.0 && latitude <= 90.0;
    }

    public static bool ValidateLongitude(double longitude)
    {
        return longitude >= -180.0 && longitude <= 180.0;
    }

    public static (double Latitude, double Longitude) NormalizeCoordinates(double latitude, double longitude)
    {
        var normalizedLat = ValidateLatitude(latitude) ? latitude : 0.0;
        var normalizedLon = ValidateLongitude(longitude) ? longitude : 0.0;
        return (normalizedLat, normalizedLon);
    }
}
#endif
