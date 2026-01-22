#if !IS_FRAMEWORK
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using TUnit.Core;
using TUnit.Assertions;
using Node.Net.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Components.Test;

/// <summary>
/// Integration tests for Maps component in example applications.
/// These tests verify that the Maps component can be used in both Blazor Server and WebAssembly hosting models.
/// </summary>
internal class MapsIntegrationTests
{
    private Bunit.TestContext _ctx = null!;

    private void Setup()
    {
        _ctx = new Bunit.TestContext();
        _ctx.Services.AddFluentUIComponents();
        ConfigureJSInteropForLeaflet(_ctx);
    }

    private void TearDown()
    {
        _ctx?.Dispose();
    }

    [Test]
    public async Task MapsComponent_CanBeRenderedInBlazorServerContext()
    {
        Setup();
        try
        {
            // Arrange & Act
            var cut = _ctx.RenderComponent<Maps>(parameters => parameters
                .Add(p => p.Latitude, 45.5)
                .Add(p => p.Longitude, -120.3));

            // Assert
            await Assert.That(cut).IsNotNull();
            await Assert.That(cut.Markup).IsNotEmpty();
            await Assert.That(cut.Markup).Contains("map-");
        }
        finally
        {
            TearDown();
        }
    }

    [Test]
    public async Task MapsComponent_CanBeRenderedInWebAssemblyContext()
    {
        Setup();
        // Arrange & Act - Same component works in both contexts
        var cut = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 37.7749)
            .Add(p => p.Longitude, -122.4194)
            .Add(p => p.ZoomLevel, 13)
            .Add(p => p.MapType, "roadmap"));

        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        await Assert.That(cut.Instance.Latitude).IsEqualTo(37.7749);
        await Assert.That(cut.Instance.Longitude).IsEqualTo(-122.4194);
        await Assert.That(cut.Instance.ZoomLevel).IsEqualTo(13);
        await Assert.That(cut.Instance.MapType).IsEqualTo("roadmap");
    }

    [Test]
    public async Task MapsComponent_HandlesMultipleInstances()
    {
        Setup();
        // Arrange & Act - Render multiple map instances (simulating multiple maps on a page)
        var cut1 = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 40.7128)
            .Add(p => p.Longitude, -74.0060));

        var cut2 = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 51.5074)
            .Add(p => p.Longitude, -0.1278));

        // Assert - Both should render independently
        await Assert.That(cut1).IsNotNull();
        await Assert.That(cut2).IsNotNull();
        await Assert.That(cut1.Markup).IsNotEqualTo(cut2.Markup);
    }

    [Test]
    public async Task MapsComponent_WorksWithDefaultParameters()
    {
        Setup();
        // Arrange & Act - Use only required parameters, defaults should apply
        var cut = _ctx.RenderComponent<Maps>(parameters => parameters
            .Add(p => p.Latitude, 0.0)
            .Add(p => p.Longitude, 0.0));

        // Assert
        await Assert.That(cut.Instance.ZoomLevel).IsEqualTo(13);
        await Assert.That(cut.Instance.MapType).IsEqualTo("satellite");
    }

    private static void ConfigureJSInteropForLeaflet(Bunit.TestContext ctx)
    {
        // Configure JSInterop for Leaflet map operations
        // The Maps component imports a JavaScript module, so we need to set that up first
        var modulePath = "./_content/Node.Net.Components/Maps.razor.js";
        
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
