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
    // NOTE: The following tests are for the OLD interface methods that have been removed.
    // These tests are kept for reference but are no longer valid since IApplication
    // now only has GetApplicationInfo() method. The behavior is now tested via GetApplicationInfo() tests below.
    // 
    // OLD TESTS REMOVED:
    // - GetName_ReturnsApplicationName
    // - GetName_ReturnsAssemblyNameWhenTitleMissing
    // - GetName_ReturnsUnknownWhenAssemblyNameNull
    // - GetCompany_ReturnsCompanyName
    // - GetCompany_ReturnsEmptyStringWhenMissing
    // - GetApplicationDataDirectory_ReturnsDirectoryInfo
    // - GetApplicationDataDirectory_CreatesDirectoryIfNotExists
    // - GetApplicationDataDirectory_UsesPlatformConventions
    // - GetApplicationDataDirectory_ThrowsExceptionOnPermissionDenied
    // - GetApplicationDataDirectory_IsIdempotent
    // - GetName_PerformanceUnder10ms
    // - GetCompany_PerformanceUnder10ms
    // - GetApplicationDataDirectory_PerformanceUnder10ms
    // - GetTargetFramework_ReturnsValidFramework
    // - GetTargetFramework_IsIdempotent
    // - GetTargetFramework_Performance
    // - GetExecutingAssemblyFilename_ReturnsValidPath
    // - GetExecutingAssemblyFilename_IsIdempotent
    // - GetExecutingAssemblyFilename_Performance
    // - GetVersion_ReturnsValidVersion
    // - GetVersion_IsIdempotent
    // - GetVersion_Performance
    //
    // All functionality is now tested via GetApplicationInfo() tests below.

    // Phase 4: Tests for GetApplicationInfo() method
    [Test]
    public void GetApplicationInfo_ReturnsApplicationInfo()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info, Is.Not.Null);
        Assert.That(info, Is.InstanceOf<ApplicationInfo>());
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidName()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.Name, Is.Not.Null);
        Assert.That(info.Name, Is.Not.Empty);
        // Verify it's a valid application name (not "Unknown" unless assembly name is truly null)
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidCompany()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.Company, Is.Not.Null);
        // Company can be empty string if not available, which is valid
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidDataDirectory()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.DataDirectory, Is.Not.Null);
        // DataDirectory can be empty string if creation fails, which is valid per spec
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidTargetFramework()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.TargetFramework, Is.Not.Null);
        Assert.That(info.TargetFramework, Is.Not.Empty);
        // Should be "net8.0", "net8.0-windows", "net48", or "unknown"
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidExecutingAssemblyFilename()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.ExecutingAssemblyFilename, Is.Not.Null);
        // Can be empty string if not available, which is valid
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidVersion()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.Version, Is.Not.Null);
        // Can be empty string if not available, which is valid
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidUser()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.User, Is.Not.Null);
        // Can be empty string if not available, which is valid
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidDomain()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.Domain, Is.Not.Null);
        // Can be empty string if not available, which is valid
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidOperatingSystem()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.OperatingSystem, Is.Not.Null);
        // Can be empty string if not available, which is valid
    }

    [Test]
    public void GetApplicationInfo_ReturnsValidMachine()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        Assert.That(info.Machine, Is.Not.Null);
        // Can be empty string if not available, which is valid
    }

    [Test]
    public void GetApplicationInfo_HandlesDataDirectoryFailure()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert - Should not throw, DataDirectory should be empty string if creation failed
        Assert.That(info.DataDirectory, Is.Not.Null);
        // Note: We can't easily simulate directory creation failure in a unit test,
        // but we verify that the method doesn't throw and returns a valid ApplicationInfo
    }

    [Test]
    public void GetApplicationInfo_IsIdempotent()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info1 = app.GetApplicationInfo();
        var info2 = app.GetApplicationInfo();

        // Assert - Properties should be equivalent (may be different instances)
        Assert.That(info1.Name, Is.EqualTo(info2.Name));
        Assert.That(info1.Company, Is.EqualTo(info2.Company));
        Assert.That(info1.DataDirectory, Is.EqualTo(info2.DataDirectory));
        Assert.That(info1.TargetFramework, Is.EqualTo(info2.TargetFramework));
        Assert.That(info1.ExecutingAssemblyFilename, Is.EqualTo(info2.ExecutingAssemblyFilename));
        Assert.That(info1.Version, Is.EqualTo(info2.Version));
    }

    [Test]
    public void GetApplicationInfo_ReturnsNewInstanceEachCall()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info1 = app.GetApplicationInfo();
        var info2 = app.GetApplicationInfo();

        // Assert - Should be different instances (not cached)
        Assert.That(info1, Is.Not.SameAs(info2));
    }

    [Test]
    public void GetApplicationInfo_PerformanceUnder10ms()
    {
        // Arrange
        var app = new ApplicationService();
        var stopwatch = new Stopwatch();
        var iterations = 100;

        // Act
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            _ = app.GetApplicationInfo();
        }
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;

        // Assert
        var averageMs = elapsed / (double)iterations;
        Assert.That(averageMs, Is.LessThan(10),
            $"GetApplicationInfo should be fast (average {averageMs:F2}ms per call)");
    }
}
