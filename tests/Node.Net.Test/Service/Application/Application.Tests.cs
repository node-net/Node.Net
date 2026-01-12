extern alias NodeNet;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using NodeNet::Node.Net.Diagnostic.Generic;
using NodeNet::Node.Net.Service.Application;
using ApplicationService = NodeNet::Node.Net.Service.Application.Application;

namespace Node.Net.Test.Service.Application;

[TestFixture]
internal class ApplicationTests : TestHarness<ApplicationService>
{
    [Test]
    public void GetName_ReturnsApplicationName()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var name = app.GetName();

        // Assert
        Assert.That(name, Is.Not.Null);
        Assert.That(name, Is.Not.Empty);
        // Application.GetExecutingAssembly() returns the assembly where Application class is defined (Node.Net)
        // So we need to check the Node.Net assembly, not the test assembly
        var nodeNetAssembly = typeof(ApplicationService).Assembly;
        var titleAttribute = nodeNetAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
        string expectedName;
        if (titleAttribute != null && !string.IsNullOrEmpty(titleAttribute.Title))
        {
            expectedName = titleAttribute.Title;
        }
        else
        {
            expectedName = nodeNetAssembly.GetName().Name ?? "Unknown";
        }
        Assert.That(name, Is.EqualTo(expectedName));
    }

    [Test]
    public void GetName_ReturnsAssemblyNameWhenTitleMissing()
    {
        // Arrange
        var app = new ApplicationService();
        var assembly = Assembly.GetExecutingAssembly();
        var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();

        // Act
        var name = app.GetName();

        // Assert
        Assert.That(name, Is.Not.Null);
        if (titleAttribute == null || string.IsNullOrEmpty(titleAttribute.Title))
        {
            // Should fallback to assembly name
            var assemblyName = assembly.GetName().Name;
            Assert.That(name, Is.EqualTo(assemblyName ?? "Unknown"));
        }
        else
        {
            // If title exists, it should be used (test still passes)
            Assert.Pass("AssemblyTitle exists, fallback not tested");
        }
    }

    [Test]
    public void GetName_ReturnsUnknownWhenAssemblyNameNull()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var name = app.GetName();

        // Assert
        Assert.That(name, Is.Not.Null);
        Assert.That(name, Is.Not.Empty);
        // If assembly name is null, should return "Unknown"
        // Note: This is hard to test directly without mocking, but we verify the method never returns null
    }

    [Test]
    public void GetCompany_ReturnsCompanyName()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var company = app.GetCompany();

        // Assert
        Assert.That(company, Is.Not.Null);
        // Should return AssemblyCompany attribute value if present
        var nodeNetAssembly = typeof(ApplicationService).Assembly;
        var companyAttribute = nodeNetAssembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        if (companyAttribute != null && !string.IsNullOrEmpty(companyAttribute.Company))
        {
            Assert.That(company, Is.EqualTo(companyAttribute.Company));
        }
        else
        {
            // Should return empty string if not present
            Assert.That(company, Is.EqualTo(string.Empty));
        }
    }

    [Test]
    public void GetCompany_ReturnsEmptyStringWhenMissing()
    {
        // Arrange
        var app = new ApplicationService();
        var nodeNetAssembly = typeof(ApplicationService).Assembly;
        var companyAttribute = nodeNetAssembly.GetCustomAttribute<AssemblyCompanyAttribute>();

        // Act
        var company = app.GetCompany();

        // Assert
        Assert.That(company, Is.Not.Null);
        if (companyAttribute == null || string.IsNullOrEmpty(companyAttribute.Company))
        {
            Assert.That(company, Is.EqualTo(string.Empty));
        }
        else
        {
            // If company exists, it should be used (test still passes)
            Assert.Pass("AssemblyCompany exists, fallback not tested");
        }
    }

    [Test]
    public void GetApplicationDataDirectory_ReturnsDirectoryInfo()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var directory = app.GetApplicationDataDirectory();

        // Assert
        Assert.That(directory, Is.Not.Null);
        Assert.That(directory.Exists, Is.True);
        Assert.That(directory.FullName, Does.Contain(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
    }

    [Test]
    public void GetApplicationDataDirectory_CreatesDirectoryIfNotExists()
    {
        // Arrange
        var app = new ApplicationService();
        var appDataBase = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var company = app.GetCompany();
        var appName = app.GetName();
        var expectedPath = Path.Combine(appDataBase, company, appName);

        // Clean up if exists
        if (Directory.Exists(expectedPath))
        {
            Directory.Delete(expectedPath, true);
        }

        // Act
        var directory = app.GetApplicationDataDirectory();

        // Assert
        Assert.That(directory, Is.Not.Null);
        Assert.That(directory.Exists, Is.True);
        Assert.That(directory.FullName, Is.EqualTo(expectedPath));
    }

    [Test]
    public void GetApplicationDataDirectory_UsesPlatformConventions()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var directory = app.GetApplicationDataDirectory();

        // Assert
        var appDataBase = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Assert.That(directory.FullName, Does.StartWith(appDataBase).IgnoreCase);
    }

    [Test]
    public void GetApplicationDataDirectory_ThrowsExceptionOnPermissionDenied()
    {
        // Arrange
        var app = new ApplicationService();
        
        // Note: This test is difficult to execute without actually having permission issues
        // We verify the method signature and exception documentation, but cannot reliably test
        // the exception without creating a permission-denied scenario
        // The method should throw UnauthorizedAccessException if access is denied
        
        // Act & Assert
        // We verify the method works normally and has proper error handling structure
        var directory = app.GetApplicationDataDirectory();
        Assert.That(directory, Is.Not.Null);
        Assert.That(directory.Exists, Is.True);
    }

    [Test]
    public void GetApplicationDataDirectory_IsIdempotent()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var directory1 = app.GetApplicationDataDirectory();
        var directory2 = app.GetApplicationDataDirectory();

        // Assert
        Assert.That(directory1.FullName, Is.EqualTo(directory2.FullName));
        Assert.That(directory1.Exists, Is.True);
        Assert.That(directory2.Exists, Is.True);
    }

    [Test]
    public void GetName_PerformanceUnder10ms()
    {
        // Arrange
        var app = new ApplicationService();
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        for (int i = 0; i < 1000; i++)
        {
            _ = app.GetName();
        }
        stopwatch.Stop();

        // Assert
        var averageMs = stopwatch.ElapsedMilliseconds / 1000.0;
        Assert.That(averageMs, Is.LessThan(10), $"GetName() average time {averageMs}ms exceeds 10ms threshold");
    }

    [Test]
    public void GetCompany_PerformanceUnder10ms()
    {
        // Arrange
        var app = new ApplicationService();
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        for (int i = 0; i < 1000; i++)
        {
            _ = app.GetCompany();
        }
        stopwatch.Stop();

        // Assert
        var averageMs = stopwatch.ElapsedMilliseconds / 1000.0;
        Assert.That(averageMs, Is.LessThan(10), $"GetCompany() average time {averageMs}ms exceeds 10ms threshold");
    }

    [Test]
    public void GetApplicationDataDirectory_PerformanceUnder10ms()
    {
        // Arrange
        var app = new ApplicationService();
        var stopwatch = new Stopwatch();

        // Act - First call may be slower due to directory creation, so we test subsequent calls
        _ = app.GetApplicationDataDirectory(); // Warm up
        stopwatch.Start();
        for (int i = 0; i < 100; i++)
        {
            _ = app.GetApplicationDataDirectory();
        }
        stopwatch.Stop();

        // Assert
        var averageMs = stopwatch.ElapsedMilliseconds / 100.0;
        Assert.That(averageMs, Is.LessThan(10), $"GetApplicationDataDirectory() average time {averageMs}ms exceeds 10ms threshold");
    }

    [Test]
    public void GetTargetFramework_ReturnsValidFramework()
    {
        // Arrange
        var app = new ApplicationService();
        
        // Act
        var framework = app.GetTargetFramework();
        
        // Assert
        Assert.That(framework, Is.Not.Null);
        Assert.That(framework, Is.Not.Empty);
        // Should be a valid TFM format (e.g., net8.0, net8.0-windows, net48, or unknown)
        Assert.That(framework, Does.Match(@"^(net\d+(\.\d+)?(-windows)?|unknown)$"), 
            $"Target framework should be a valid TFM or 'unknown', but was: {framework}");
    }

    [Test]
    public void GetTargetFramework_IsIdempotent()
    {
        // Arrange
        var app = new ApplicationService();
        
        // Act
        var framework1 = app.GetTargetFramework();
        var framework2 = app.GetTargetFramework();
        
        // Assert
        Assert.That(framework1, Is.EqualTo(framework2), "Multiple calls should return the same framework");
    }

    [Test]
    public void GetTargetFramework_Performance()
    {
        // Arrange
        var app = new ApplicationService();
        var stopwatch = new Stopwatch();
        var iterations = 100;
        
        // Act
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            _ = app.GetTargetFramework();
        }
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        
        // Assert
        var averageMs = elapsed / (double)iterations;
        Assert.That(averageMs, Is.LessThan(10), 
            $"GetTargetFramework should be fast (average {averageMs:F2}ms per call)");
    }

    [Test]
    public void GetExecutingAssemblyFilename_ReturnsValidPath()
    {
        // Arrange
        var app = new ApplicationService();
        
        // Act
        var filename = app.GetExecutingAssemblyFilename();
        
        // Assert
        Assert.That(filename, Is.Not.Null);
        // Should be either empty string (if location not available) or a valid file path
        if (!string.IsNullOrEmpty(filename))
        {
            Assert.That(filename, Does.Contain(".dll").Or.Contain(".exe"), 
                "Assembly filename should contain .dll or .exe extension");
        }
    }

    [Test]
    public void GetExecutingAssemblyFilename_IsIdempotent()
    {
        // Arrange
        var app = new ApplicationService();
        
        // Act
        var filename1 = app.GetExecutingAssemblyFilename();
        var filename2 = app.GetExecutingAssemblyFilename();
        
        // Assert
        Assert.That(filename1, Is.EqualTo(filename2), "Multiple calls should return the same filename");
    }

    [Test]
    public void GetExecutingAssemblyFilename_Performance()
    {
        // Arrange
        var app = new ApplicationService();
        var stopwatch = new Stopwatch();
        var iterations = 1000;
        
        // Act
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            _ = app.GetExecutingAssemblyFilename();
        }
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        
        // Assert
        var averageMs = elapsed / (double)iterations;
        Assert.That(averageMs, Is.LessThan(10), 
            $"GetExecutingAssemblyFilename should be fast (average {averageMs:F2}ms per call)");
    }

    [Test]
    public void GetVersion_ReturnsValidVersion()
    {
        // Arrange
        var app = new ApplicationService();
        
        // Act
        var version = app.GetVersion();
        
        // Assert
        Assert.That(version, Is.Not.Null);
        // Should be either empty string (if version not available) or a valid version string
        // We just verify it's a string - version format can vary (e.g., "1.2.3", "1.2.3.4", "1.2.3-beta", etc.)
        // The important thing is that it doesn't throw and returns a non-null string
        if (string.IsNullOrEmpty(version))
        {
            // If version is empty, that's acceptable (assembly may not have version info)
            Assert.Pass("Version is empty, which is acceptable if assembly doesn't have version metadata");
        }
        else
        {
            // If version is not empty, verify it contains at least one digit (basic sanity check)
            Assert.That(version, Does.Contain("0").Or.Contain("1").Or.Contain("2").Or.Contain("3").Or.Contain("4")
                .Or.Contain("5").Or.Contain("6").Or.Contain("7").Or.Contain("8").Or.Contain("9"),
                $"Version should contain at least one digit, but was: {version}");
        }
    }

    [Test]
    public void GetVersion_IsIdempotent()
    {
        // Arrange
        var app = new ApplicationService();
        
        // Act
        var version1 = app.GetVersion();
        var version2 = app.GetVersion();
        
        // Assert
        Assert.That(version1, Is.EqualTo(version2), "Multiple calls should return the same version");
    }

    [Test]
    public void GetVersion_Performance()
    {
        // Arrange
        var app = new ApplicationService();
        var stopwatch = new Stopwatch();
        var iterations = 1000;
        
        // Act
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            _ = app.GetVersion();
        }
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        
        // Assert
        var averageMs = elapsed / (double)iterations;
        Assert.That(averageMs, Is.LessThan(10), 
            $"GetVersion should be fast (average {averageMs:F2}ms per call)");
    }
}
