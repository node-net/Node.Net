#if !IS_FRAMEWORK
using System;
using System.Threading.Tasks;
using Bunit;
using Microsoft.JSInterop;

namespace Node.Net.Components.Test;

/// <summary>
/// Helper class for configuring JSInterop for Fluent UI components in bUnit tests.
/// This eliminates the need to manually configure JSInterop in each test.
/// </summary>
internal static class FluentUIJSInteropHelper
{
    /// <summary>
    /// Configures JSInterop for Fluent UI components in a bUnit test context.
    /// This sets up all the necessary module imports and method calls that Fluent UI components require.
    /// </summary>
    /// <param name="ctx">The bUnit test context to configure.</param>
    public static void ConfigureJSInterop(Bunit.BunitContext ctx)
    {
        // Fluent UI components require JSInterop for module imports and method calls
        // When components call InvokeAsync<IJSObjectReference>("import", modulePath),
        // bUnit REQUIRES using SetupModule (cannot use Setup<IJSObjectReference>)
        
        // Set up modules for FluentUI paths - must match EXACTLY including query strings
        // Components typically call the versioned path, so set that up first
        var modulePaths = new[]
        {
            "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Label/FluentInputLabel.razor.js?v=4.13.2.25331",
            "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Label/FluentInputLabel.razor.js",
        };
        
        foreach (var modulePath in modulePaths)
        {
            // SetupModule handles the "import" call and returns a module interop
            var moduleInterop = ctx.JSInterop.SetupModule(modulePath);
            
            // Setup catch-all handlers for method calls on the module
            moduleInterop.SetupVoid(_ => true);
            moduleInterop.Setup<string>(_ => true);
            moduleInterop.Setup<object>(_ => true);
        }
        
        // Set JSInterop to Loose mode for any other calls
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
    }
}
#endif
