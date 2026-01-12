#if !IS_FRAMEWORK
extern alias NodeNet;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using NodeNet::Node.Net.Components;
using NodeNet::Node.Net.Diagnostic;
using Microsoft.Playwright;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Test.Components;

[TestFixture]
internal class SystemInfoTests : TestHarness
{
    public SystemInfoTests() : base(typeof(SystemInfo))
    {
    }

    [Test]
    public async Task Render_GeneratesImage()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        
        // Add required services for Fluent UI components
        ctx.Services.AddFluentUIComponents();
        
        // Render the component
        var cut = ctx.RenderComponent<SystemInfo>();
        
        // Assert component rendered
        Assert.That(cut, Is.Not.Null);
        Assert.That(cut.Markup, Is.Not.Empty);
        
        // Generate image from rendered HTML
        var artifactFile = GetArtifactFileInfo("SystemInfo.png");
        await GenerateComponentImage(cut.Markup, artifactFile.FullName);
        
        // Verify artifact was created (either PNG image or TXT placeholder)
        var pngExists = File.Exists(artifactFile.FullName);
        var txtFile = GetArtifactFileInfo("SystemInfo.txt");
        var txtExists = File.Exists(txtFile.FullName);
        
        Assert.That(pngExists || txtExists, Is.True, $"Artifact should be created at {artifactFile.FullName} or {txtFile.FullName}");
        
        if (pngExists)
        {
            Assert.That(new FileInfo(artifactFile.FullName).Length, Is.GreaterThan(0), "Image file should not be empty");
        }
        else if (txtExists)
        {
            Assert.That(new FileInfo(txtFile.FullName).Length, Is.GreaterThan(0), "Text file should not be empty");
        }
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
            
            // Take screenshot
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = outputPath,
                FullPage = true
            });
        }
        catch (PlaywrightException ex) when (ex.Message.Contains("Executable doesn't exist"))
        {
            // Playwright browsers not installed - create a placeholder text file with instructions
            var placeholderText = $@"Playwright browsers not installed. To install, run:
pwsh -Command ""playwright install chromium""

Original HTML:
{html}";
            var txtPath = outputPath.Replace(".png", ".txt");
            await File.WriteAllTextAsync(txtPath, placeholderText);
            Assert.Warn("Playwright browsers not installed. Install with: playwright install chromium");
        }
    }
}
#endif
