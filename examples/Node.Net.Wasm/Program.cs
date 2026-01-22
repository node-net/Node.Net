using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Node.Net.Wasm;
using Node.Net.Service.Application;
using Node.Net.Service.Logging;
using System.IO;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluentUIComponents();
builder.Services.AddSingleton<IApplication, Application>();

// Configure Node.Net Logging
// Note: WebAssembly has limited file system access, so we use in-memory database
// In a real scenario, you might want to use browser storage (IndexedDB) via a different implementation
var logService = new LogService(); // Uses in-memory database for WASM
builder.Services.AddSingleton<ILogService>(logService);
builder.Services.AddNodeNetLogging(logService);

await builder.Build().RunAsync();
