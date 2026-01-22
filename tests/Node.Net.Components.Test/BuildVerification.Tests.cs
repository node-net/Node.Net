#nullable enable
using TUnit.Core;
using TUnit.Assertions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Node.Net.Diagnostic;

namespace Node.Net.Components.Test;

/// <summary>
/// Build verification tests for .NET Standard 2.0 target framework support.
/// </summary>
internal class BuildVerificationTests : TestHarness
{
    public BuildVerificationTests() : base("BuildVerification")
    {
    }

    /// <summary>
    /// Executes a build command and returns the result with improved error handling.
    /// </summary>
    private (int ExitCode, string Output, string Error) ExecuteBuild(string projectPath, string? framework = null, string? configuration = null, int timeoutMs = 60000)
    {
        var arguments = $"build \"{projectPath}\"";
        if (configuration != null)
        {
            arguments += $" --configuration {configuration}";
        }
        if (framework != null)
        {
            arguments += $" --framework {framework}";
        }
            
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processStartInfo);
        if (process == null)
        {
            throw new InvalidOperationException("Failed to start dotnet build process");
        }

        var outputTask = Task.Run(() => process.StandardOutput.ReadToEnd());
        var errorTask = Task.Run(() => process.StandardError.ReadToEnd());
        
        if (!process.WaitForExit(timeoutMs))
        {
            process.Kill();
            throw new TimeoutException($"Build process exceeded timeout of {timeoutMs}ms");
        }

        Task.WaitAll(outputTask, errorTask);
        return (process.ExitCode, outputTask.Result, errorTask.Result);
    }

    /// <summary>
    /// Checks if build output contains known compatibility issues that indicate the framework
    /// has fundamental incompatibilities that need to be addressed in the codebase.
    /// Note: This method is kept for potential future use but compatibility issues have been fixed.
    /// </summary>
    private bool HasKnownCompatibilityIssues(string output, string error, string framework)
    {
        // All known compatibility issues have been fixed with conditional compilation
        // This method is kept for potential future use
        return false;
    }

    /// <summary>
    /// Filters out warnings and focuses on actual errors for better error messages.
    /// </summary>
    private string FilterErrors(string output, string error)
    {
        var lines = (output + "\n" + error).Split('\n');
        var errorLines = lines.Where(line => 
            line.Contains("error CS") || 
            line.Contains("error MSB") ||
            line.Contains("error NETSDK") ||
            line.Contains("Build FAILED")).ToList();
        
        return errorLines.Count > 0 
            ? string.Join("\n", errorLines.Take(20)) // Limit to first 20 errors
            : output + "\n" + error;
    }

    [Test]
    public async Task Build_NetStandard2_0_Succeeds()
    {
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        await Assert.That(File.Exists(projectPath)).IsTrue();

        // Act
        var (exitCode, output, error) = ExecuteBuild(projectPath, "netstandard2.0", "Release");

        // Assert
        if (exitCode != 0)
        {
            var filteredErrors = FilterErrors(output, error);
            throw new Exception(
                $"Build failed for netstandard2.0. Exit code: {exitCode}. " +
                $"Filtered errors:\n{filteredErrors}");
        }
        
        await Assert.That(exitCode).IsEqualTo(0);
    }

    [Test]
    public async Task Build_AllTargetFrameworks_Succeeds()
    {
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        await Assert.That(File.Exists(projectPath)).IsTrue();

        // Act
        var (exitCode, output, error) = ExecuteBuild(projectPath, configuration: "Release");

        // Assert
        if (exitCode != 0)
        {
            var filteredErrors = FilterErrors(output, error);
            throw new Exception(
                $"Build failed for all target frameworks. Exit code: {exitCode}. " +
                $"Filtered errors:\n{filteredErrors}");
        }
        
        await Assert.That(exitCode).IsEqualTo(0);
        
        // Verify netstandard2.0 is mentioned in output (only check if build succeeded)
        await Assert.That(output + error).Contains("netstandard2.0");
    }

    [Test]
    public async Task Build_ExistingTargetFrameworks_StillSucceeds()
    {
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        await Assert.That(File.Exists(projectPath)).IsTrue();

        // net48 requires Windows/.NET Framework SDK, so skip on non-Windows platforms
        var frameworks = new List<string> { "net8.0" };
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            frameworks.Add("net48");
            frameworks.Add("net8.0-windows");
        }
        else
        {
            // On non-Windows, only test net8.0 (net8.0-windows also requires Windows)
            // Skip net48 and net8.0-windows as they require Windows SDK
        }

        foreach (var framework in frameworks)
        {
            // Act
            var (exitCode, output, error) = ExecuteBuild(projectPath, framework, "Release");

            // Assert
            if (exitCode != 0)
            {
                var filteredErrors = FilterErrors(output, error);
                throw new Exception(
                    $"Build failed for {framework}. Exit code: {exitCode}. " +
                    $"Filtered errors:\n{filteredErrors}");
            }
            
            await Assert.That(exitCode).IsEqualTo(0);
        }
    }

    [Test]
    public async Task PackageReference_NetStandard2_0_CanReferenceLibrary()
    {
        // This test verifies that a project targeting .NET Standard 2.0 can reference Node.Net
        // Note: This is a placeholder test - actual package reference test should be done manually
        // or via a separate test project as it requires NuGet package generation
        
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        await Assert.That(File.Exists(projectPath)).IsTrue();

        // Act & Assert
        // Verify the project file includes netstandard2.0 in TargetFrameworks
        var projectContent = File.ReadAllText(projectPath);
        await Assert.That(projectContent).Contains("netstandard2.0");
        
        // Verify System.Drawing.Common 7.0.0 is referenced for netstandard2.0
        await Assert.That(projectContent).Contains("System.Drawing.Common\" Version=\"7.0.0\"");
    }
}
