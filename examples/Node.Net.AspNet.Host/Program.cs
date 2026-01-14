using Node.Net.AspNet.Host.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Node.Net.Service.Application;
using Node.Net.Service.Logging;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddSingleton<IApplication, Application>();

// Configure Node.Net Logging
// Create LogService instance and register it
var tempApp = new Application();
var appInfo = tempApp.GetApplicationInfo();
var logDbPath = string.IsNullOrEmpty(appInfo.DataDirectory) 
    ? null 
    : Path.Combine(appInfo.DataDirectory, "log.db");
var logService = new LogService(logDbPath);
builder.Services.AddSingleton<ILogService>(logService);
builder.Services.AddNodeNetLogging(logService);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
