#if !IS_FRAMEWORK
extern alias NodeNet;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using NUnit.Framework;
using NodeNet::Node.Net.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Test.Components;

/// <summary>
/// Integration tests for Maps component in example applications.
/// These tests verify that the Maps component can be used in both Blazor Server and WebAssembly hosting models.
/// </summary>
[TestFixture]
internal class MapsIntegrationTests
{
    private Bunit.TestContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = new Bunit.TestContext();
        _ctx.Services.AddFluentUIComponents();
        ConfigureJSInteropForLeaflet(_ctx);
    }

    [TearDown]
    public void TearDown()
    {
        _ctx?.Dispose();
    }

    [Test]
    public void MapsComponent_CanBeRenderedInBlazorServerContext()
    {
        // Arrange & Act
        var cut = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 45.5)
            .Add(p => p.Longitude, -120.3));

        // Assert
        Assert.That(cut, Is.Not.Null);
        Assert.That(cut.Markup, Is.Not.Empty);
        Assert.That(cut.Markup, Does.Contain("map-"), "Component should contain map element");
    }

    [Test]
    public void MapsComponent_CanBeRenderedInWebAssemblyContext()
    {
        // Arrange & Act - Same component works in both contexts
        var cut = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 37.7749)
            .Add(p => p.Longitude, -122.4194)
            .Add(p => p.ZoomLevel, 13)
            .Add(p => p.MapType, "roadmap"));

        // Assert
        Assert.That(cut, Is.Not.Null);
        Assert.That(cut.Markup, Is.Not.Empty);
        Assert.That(cut.Instance.Latitude, Is.EqualTo(37.7749));
        Assert.That(cut.Instance.Longitude, Is.EqualTo(-122.4194));
        Assert.That(cut.Instance.ZoomLevel, Is.EqualTo(13));
        Assert.That(cut.Instance.MapType, Is.EqualTo("roadmap"));
    }

    [Test]
    public void MapsComponent_HandlesMultipleInstances()
    {
        // Arrange & Act - Render multiple map instances (simulating multiple maps on a page)
        var cut1 = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 40.7128)
            .Add(p => p.Longitude, -74.0060));

        var cut2 = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 51.5074)
            .Add(p => p.Longitude, -0.1278));

        // Assert - Both should render independently
        Assert.That(cut1, Is.Not.Null);
        Assert.That(cut2, Is.Not.Null);
        Assert.That(cut1.Markup, Is.Not.EqualTo(cut2.Markup), "Each map instance should have unique markup");
    }

    [Test]
    public void MapsComponent_WorksWithDefaultParameters()
    {
        // Arrange & Act - Use only required parameters, defaults should apply
        var cut = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 0.0)
            .Add(p => p.Longitude, 0.0));

        // Assert
        Assert.That(cut.Instance.ZoomLevel, Is.EqualTo(13), "Default zoom should be 13");
        Assert.That(cut.Instance.MapType, Is.EqualTo("satellite"), "Default map type should be satellite");
    }

    private static void ConfigureJSInteropForLeaflet(Bunit.TestContext ctx)
    {
        // Configure JSInterop for Leaflet map operations
        // The Maps component imports a JavaScript module, so we need to set that up first
        var modulePath = "./_content/Node.Net/Components/Maps.razor.js";
        
        // Setup the module import - SetupModule handles the "import" call
        var moduleInterop = ctx.JSInterop.SetupModule(modulePath);
        
        // Setup the module methods that will be called
        // Note: initializeMap returns an IJSObjectReference, but bUnit module interop
        // doesn't support Setup<IJSObjectReference> directly. Use SetupModule's return value instead.
        // For now, we'll use Loose mode to allow all calls through
        moduleInterop.SetupVoid("updateMapCenter", _ => true);
        moduleInterop.SetupVoid("updateZoomLevel", _ => true);
        moduleInterop.SetupVoid("updateMapType", _ => true);
        
        // Set JSInterop to Loose mode for any other calls (including initializeMap returning IJSObjectReference)
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
    }
}
#endif
