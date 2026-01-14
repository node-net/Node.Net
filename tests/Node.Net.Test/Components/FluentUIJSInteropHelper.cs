#if !IS_FRAMEWORK
using System;
using System.Threading.Tasks;
using Bunit;
using Microsoft.JSInterop;

namespace Node.Net.Test.Components;

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
    public static void ConfigureJSInterop(Bunit.TestContext ctx)
    {
        // Fluent UI components require JSInterop for module imports and method calls
        // We need to set up the import call on the main JSInterop, then configure the module
        
        // First, set up the import calls on the main JSInterop
        // Fluent UI components call: InvokeAsync<IJSObjectReference>("import", modulePath)
        var modulePaths = new[]
        {
            "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Label/FluentInputLabel.razor.js?v=4.13.2.25331",
            "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Label/FluentInputLabel.razor.js"
        };
        
        foreach (var modulePath in modulePaths)
        {
            // Setup the import call - this returns an IJSObjectReference
            ctx.JSInterop.Setup<IJSObjectReference>("import", modulePath);
            
            // Now set up the module itself for method calls
            var moduleInterop = ctx.JSInterop.SetupModule(modulePath);
            
            // Setup catch-all handlers for all possible method calls on the module
            // This handles methods like setInputAriaLabel, focus, blur, scrollIntoView, etc.
            moduleInterop.SetupVoid(_ => true);
            moduleInterop.Setup<string>(_ => true);
            moduleInterop.Setup<object>(_ => true);
            moduleInterop.Setup<IJSObjectReference>(_ => true);
        }
        
        // Set the main JSInterop to Loose mode for any other calls
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
    }
}
#endif
