#nullable enable
extern alias NodeNet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NodeNet::Node.Net.Diagnostic;

namespace Node.Net.Test.Components;

/// <summary>
/// Build verification tests for .NET Standard 2.0 target framework support.
/// </summary>
[TestFixture]
internal class BuildVerificationTests : TestHarness
{
    public BuildVerificationTests() : base("BuildVerification")
    {
    }

    /// <summary>
    /// Executes a build command and returns the result with improved error handling.
    /// </summary>
    private (int ExitCode, string Output, string Error) ExecuteBuild(string projectPath, string? framework = null, int timeoutMs = 60000)
    {
        var arguments = framework != null 
            ? $"build \"{projectPath}\" --framework {framework}"
            : $"build \"{projectPath}\"";
            
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
    public void Build_NetStandard2_0_Succeeds()
    {
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        Assert.That(File.Exists(projectPath), Is.True, 
            $"Project file not found at: {projectPath}. Repo root: {repoRoot.FullName}");

        // Act
        var (exitCode, output, error) = ExecuteBuild(projectPath, "netstandard2.0");

        // Assert
        if (exitCode != 0)
        {
            var filteredErrors = FilterErrors(output, error);
            Assert.Fail(
                $"Build failed for netstandard2.0. Exit code: {exitCode}. " +
                $"Filtered errors:\n{filteredErrors}");
        }
        
        Assert.That(exitCode, Is.EqualTo(0), 
            $"Build succeeded for netstandard2.0");
    }

    [Test]
    public void Build_AllTargetFrameworks_Succeeds()
    {
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        Assert.That(File.Exists(projectPath), Is.True, 
            $"Project file not found at: {projectPath}. Repo root: {repoRoot.FullName}");

        // Act
        var (exitCode, output, error) = ExecuteBuild(projectPath);

        // Assert
        // Verify netstandard2.0 is mentioned in output
        Assert.That(output, Does.Contain("netstandard2.0"), 
            "Build output should mention netstandard2.0 target framework");
        
        if (exitCode != 0)
        {
            var filteredErrors = FilterErrors(output, error);
            Assert.Fail(
                $"Build failed for all target frameworks. Exit code: {exitCode}. " +
                $"Filtered errors:\n{filteredErrors}");
        }
        
        Assert.That(exitCode, Is.EqualTo(0), 
            $"Build succeeded for all target frameworks");
    }

    [Test]
    public void Build_ExistingTargetFrameworks_StillSucceeds()
    {
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        Assert.That(File.Exists(projectPath), Is.True, 
            $"Project file not found at: {projectPath}. Repo root: {repoRoot.FullName}");

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
            var (exitCode, output, error) = ExecuteBuild(projectPath, framework);

            // Assert
            if (exitCode != 0)
            {
                var filteredErrors = FilterErrors(output, error);
                Assert.Fail(
                    $"Build failed for {framework}. Exit code: {exitCode}. " +
                    $"Filtered errors:\n{filteredErrors}");
            }
            
            Assert.That(exitCode, Is.EqualTo(0), 
                $"Build succeeded for {framework}");
        }
    }

    [Test]
    public void PackageReference_NetStandard2_0_CanReferenceLibrary()
    {
        // This test verifies that a project targeting .NET Standard 2.0 can reference Node.Net
        // Note: This is a placeholder test - actual package reference test should be done manually
        // or via a separate test project as it requires NuGet package generation
        
        // Arrange
        // GetProjectDirectoryInfo() returns the repo root directory
        var repoRoot = GetProjectDirectoryInfo();
        var projectPath = Path.GetFullPath(Path.Combine(
            repoRoot.FullName, "source", "Node.Net", "Node.Net.csproj"));
        
        Assert.That(File.Exists(projectPath), Is.True, 
            $"Project file not found at: {projectPath}. Repo root: {repoRoot.FullName}");

        // Act & Assert
        // Verify the project file includes netstandard2.0 in TargetFrameworks
        var projectContent = File.ReadAllText(projectPath);
        Assert.That(projectContent, Does.Contain("netstandard2.0"), 
            "Project file should include netstandard2.0 in TargetFrameworks");
        
        // Verify System.Drawing.Common 7.0.0 is referenced for netstandard2.0
        Assert.That(projectContent, Does.Contain("System.Drawing.Common\" Version=\"7.0.0\""), 
            "Project file should reference System.Drawing.Common 7.0.0 for netstandard2.0");
    }
}
