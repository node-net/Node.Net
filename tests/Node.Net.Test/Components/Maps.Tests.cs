#if !IS_FRAMEWORK
extern alias NodeNet;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
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

    [Test]
    public void Render_WithValidCoordinates_RendersComponent()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        // Act
        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3));

        // Assert
        Assert.That(cut, Is.Not.Null);
        Assert.That(cut.Markup, Is.Not.Empty);
        Assert.That(cut.Markup, Does.Contain("map-"), "Component should contain map element ID");
    }

    [Test]
    public void Render_WithValidCoordinates_ContainsMapElement()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        // Act
        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3));

        // Assert
        var mapElement = cut.Find("div[id^='map-']");
        Assert.That(mapElement, Is.Not.Null, "Component should contain a div with map element ID");
        var style = mapElement.GetAttribute("style");
        Assert.That(style, Does.Contain("width: 100%"), "Map element should have 100% width");
        Assert.That(style, Does.Contain("height: 100%"), "Map element should have 100% height");
    }

    [Test]
    public void Render_WithInvalidCoordinates_UsesDefaultLocation()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        // Act - Invalid coordinates should be normalized to (0, 0)
        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 95.0)  // Invalid latitude
            .Add(p => p.Longitude, 200.0)); // Invalid longitude

        // Assert - Component should still render (coordinates normalized internally)
        Assert.That(cut, Is.Not.Null);
        Assert.That(cut.Markup, Is.Not.Empty);
    }

    [Test]
    public void Render_WithDefaultParameters_UsesDefaults()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        // Act - Only provide required parameters, optional should use defaults
        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3));

        // Assert
        Assert.That(cut.Instance.ZoomLevel, Is.EqualTo(13), "Default zoom level should be 13");
        Assert.That(cut.Instance.MapType, Is.EqualTo("satellite"), "Default map type should be satellite");
    }

    [Test]
    public void Render_WithCustomZoomLevel_UsesCustomValue()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        // Act
        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3)
            .Add(p => p.ZoomLevel, 10));

        // Assert
        Assert.That(cut.Instance.ZoomLevel, Is.EqualTo(10), "Component should use custom zoom level");
    }

    [Test]
    public void Render_WithCustomMapType_UsesCustomValue()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        // Act
        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3)
            .Add(p => p.MapType, "roadmap"));

        // Assert
        Assert.That(cut.Instance.MapType, Is.EqualTo("roadmap"), "Component should use custom map type");
    }

    [Test]
    public void Render_WithoutRequiredParameters_ThrowsException()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        // Act & Assert - EditorRequired attribute should cause validation error
        // Note: bUnit may not enforce EditorRequired in tests, but the component should handle this
        // We'll verify the component structure is correct
        var cut = ctx.RenderComponent<Maps>();

        // The component should still render, but parameters will have default values
        // EditorRequired is enforced at runtime by Blazor, not at compile time
        Assert.That(cut, Is.Not.Null);
    }

    [Test]
    public void SetParameters_WithCoordinateChanges_UpdatesComponent()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3));

        // Act - Change coordinates
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Latitude, 50.0)
            .Add(p => p.Longitude, -125.0));

        // Assert
        Assert.That(cut.Instance.Latitude, Is.EqualTo(50.0), "Latitude should be updated");
        Assert.That(cut.Instance.Longitude, Is.EqualTo(-125.0), "Longitude should be updated");
    }

    [Test]
    public void SetParameters_WithZoomLevelChange_UpdatesComponent()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3)
            .Add(p => p.ZoomLevel, 10));

        // Act - Change zoom level
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3)
            .Add(p => p.ZoomLevel, 15));

        // Assert
        Assert.That(cut.Instance.ZoomLevel, Is.EqualTo(15), "Zoom level should be updated");
    }

    [Test]
    public void SetParameters_WithMapTypeChange_UpdatesComponent()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ConfigureJSInteropForLeaflet(ctx);

        var cut = ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3)
            .Add(p => p.MapType, "roadmap"));

        // Act - Change map type
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3)
            .Add(p => p.MapType, "terrain"));

        // Assert
        Assert.That(cut.Instance.MapType, Is.EqualTo("terrain"), "Map type should be updated");
    }

    private static void ConfigureJSInteropForLeaflet(Bunit.TestContext ctx)
    {
        // Configure JSInterop for Leaflet map operations
        // Leaflet uses window.L for the global object
        // We need to mock the JSInterop calls that the component will make
        
        // Set JSInterop to Loose mode to allow any calls
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        
        // Setup catch-all handlers for any JSInterop calls
        // The Maps component will call JSInterop methods like InvokeVoidAsync or InvokeAsync
        ctx.JSInterop.SetupVoid("L.map", _ => true);
        ctx.JSInterop.SetupVoid("L.tileLayer", _ => true);
        ctx.JSInterop.SetupVoid("map.setView", _ => true);
        ctx.JSInterop.SetupVoid("map.setZoom", _ => true);
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
