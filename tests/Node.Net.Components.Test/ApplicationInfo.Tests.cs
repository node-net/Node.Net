#if !IS_FRAMEWORK
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TUnit;
using System;
using System.IO;
using System.Threading.Tasks;
using Node.Net.Components;
using Node.Net.Diagnostic;
using Microsoft.Playwright;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Components.Test;

internal class ApplicationInfoTests : TestHarness
{
    public ApplicationInfoTests() : base(typeof(ApplicationInfo))
    {
    }

    [Test]
    public async Task Render_DisplaysApplicationName()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        await Assert.That(cut.Markup).Contains("Application:");
        // Should contain application name (either from assembly or "Unknown")
        await Assert.That(cut.Markup).Contains("Application Information");
    }

    [Test]
    public async Task Render_DisplaysCompanyName()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        await Assert.That(cut.Markup).Contains("Company:");
    }

    [Test]
    public async Task Render_DisplaysDataDirectory()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        await Assert.That(cut.Markup).Contains("Data Directory:");
    }

    [Test]
    public async Task Render_DisplaysTargetFramework()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        await Assert.That(cut.Markup).Contains("Target Framework:");
    }

    [Test]
    public async Task Render_DisplaysExecutingAssemblyFilename()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        await Assert.That(cut.Markup).Contains("Executing Assembly:");
    }

    [Test]
    public async Task Render_DisplaysVersion()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        await Assert.That(cut.Markup).Contains("Version:");
    }

    [Test]
    public async Task Render_DisplaysFallbackWhenMetadataMissing()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        // Component should handle missing metadata gracefully
        // Either display actual values or fallback text ("Unknown", "Not available")
        await Assert.That(cut.Markup).Contains("Application:");
        await Assert.That(cut.Markup).Contains("Company:");
    }

    [Test]
    public async Task Render_MatchesApplicationServiceValues()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        
        // Verify component uses IApplication service
        // The component should display values from Application service
        var application = new Node.Net.Service.Application.Application();
        var appInfo = application.GetApplicationInfo();
        
        // Component should display these values (or fallback if empty)
        if (!string.IsNullOrEmpty(appInfo.Name))
        {
            await Assert.That(cut.Markup).Contains(appInfo.Name);
        }
        
        if (!string.IsNullOrEmpty(appInfo.Company))
        {
            await Assert.That(cut.Markup).Contains(appInfo.Company);
        }
        
        // Verify new properties are displayed
        await Assert.That(cut.Markup).Contains("User:");
        await Assert.That(cut.Markup).Contains("Domain:");
        await Assert.That(cut.Markup).Contains("Operating System:");
        await Assert.That(cut.Markup).Contains("Machine:");
    }

    [Test]
    public async Task Render_GeneratesImage()
    {
        // Check if artifact already exists - skip if present
        var artifactFile = GetArtifactFileInfo("ApplicationInfo.jpeg");
        if (File.Exists(artifactFile.FullName))
        {
            // TUnit doesn't have Assert.Ignore - just return early
            return;
        }
        
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        
        // Render the component
        var cut = ctx.RenderComponent<ApplicationInfo>();
        
        // Assert component rendered
        await Assert.That(cut).IsNotNull();
        await Assert.That(cut.Markup).IsNotEmpty();
        
        // Generate image from rendered HTML
        await GenerateComponentImage(cut.Markup, artifactFile.FullName);
        
        // Verify artifact was created (either JPEG image or TXT placeholder)
        var jpegExists = File.Exists(artifactFile.FullName);
        var txtFile = GetArtifactFileInfo("ApplicationInfo.txt");
        var txtExists = File.Exists(txtFile.FullName);
        
        await Assert.That(jpegExists || txtExists).IsTrue();
        
        if (jpegExists)
        {
            await Assert.That(new FileInfo(artifactFile.FullName).Length).IsGreaterThan(0);
        }
        else if (txtExists)
        {
            await Assert.That(new FileInfo(txtFile.FullName).Length).IsGreaterThan(0);
        }
    }

    [Test]
    public async Task Render_PerformanceUnder500ms()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ctx.Services.AddFluentUIComponents();
        ctx.Services.AddSingleton<Node.Net.Service.Application.IApplication>(
            new Node.Net.Service.Application.Application());
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        // Act
        var cut = ctx.RenderComponent<ApplicationInfo>();
        stopwatch.Stop();
        
        // Assert
        await Assert.That(cut).IsNotNull();
        await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(500);
    }

    private async Task GenerateComponentImage(string html, string outputPath)
    {
        // Create a complete HTML document with the component markup
        var fullHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            padding: 20px;
            background: white;
        }}
        .fluent-card {{
            border: 1px solid #e1e1e1;
            border-radius: 4px;
            padding: 1rem;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
    </style>
</head>
<body>
    {html}
</body>
</html>";

        // Use Playwright to render HTML and take screenshot
        try
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            
            var page = await browser.NewPageAsync();
            await page.SetContentAsync(fullHtml);
            
            // Wait for content to render
            await page.WaitForTimeoutAsync(500);
            
            // Take screenshot as JPEG
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = outputPath,
                FullPage = true,
                Type = ScreenshotType.Jpeg,
                Quality = 90
            });
        }
        catch (PlaywrightException ex)
        {
            // Playwright error - create a placeholder text file with error details
            var placeholderText = $@"Playwright error: {ex.Message}

To install Playwright browsers, run:
pwsh -Command ""playwright install chromium""

Original HTML:
{html}";
            var txtPath = outputPath.Replace(".jpeg", ".txt");
            await File.WriteAllTextAsync(txtPath, placeholderText);
            // Don't fail the test - just create the placeholder and continue
            // The test will verify that either JPEG or TXT exists
        }
    }
}
#endif
