#tool nuget:?package=NuGet.CommandLine&version=6.5.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "./Node.Net.sln";
var mainProject = "./source/Node.Net/Node.Net.csproj";
var testProject = "./tests/Node.Net.Test/Node.Net.Test.csproj";
var componentsTestProject = "./tests/Node.Net.Components.Test/Node.Net.Components.Test.csproj";
var artifactsDir = "./artifacts";
var nupkgDir = $"{artifactsDir}/nupkg";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    Information("Building Node.Net");
    Information("Configuration: {0}", configuration);
    Information("Target: {0}", target);
    
    // Create artifacts directory
    EnsureDirectoryExists(artifactsDir);
    EnsureDirectoryExists(nupkgDir);
});

Teardown(ctx =>
{
    Information("Build completed.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    Information("Cleaning artifacts directory...");
    CleanDirectory(artifactsDir);
    EnsureDirectoryExists(nupkgDir);
    
    Information("Cleaning build outputs...");
    var cleanSettings = new DotNetCleanSettings
    {
        Configuration = configuration,
        Verbosity = DotNetVerbosity.Minimal
    };
    
    DotNetClean(solution, cleanSettings);
});

Task("Restore")
    .Does(() =>
{
    Information("Restoring NuGet packages...");
    DotNetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    Information("Building solution...");
    var buildSettings = new DotNetBuildSettings
    {
        Configuration = configuration,
        NoRestore = true,
        Verbosity = DotNetVerbosity.Minimal
    };
    
    DotNetBuild(solution, buildSettings);
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Creating NuGet package...");
    var packSettings = new DotNetPackSettings
    {
        Configuration = configuration,
        OutputDirectory = nupkgDir,
        NoBuild = true,
        Verbosity = DotNetVerbosity.Minimal
    };
    
    DotNetPack(mainProject, packSettings);
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Running tests...");
    
    var testSettings = new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
        Verbosity = DotNetVerbosity.Normal,
        Loggers = new[] { "console;verbosity=normal" }
    };
    
    // Run Node.Net.Test
    Information("Running Node.Net.Test...");
    DotNetTest(testProject, testSettings);
    
    // Run Node.Net.Components.Test (if it exists)
    if (FileExists(componentsTestProject))
    {
        Information("Running Node.Net.Components.Test...");
        DotNetTest(componentsTestProject, testSettings);
    }
    else
    {
        Information("Node.Net.Components.Test project not found, skipping...");
    }
});

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("CI")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
