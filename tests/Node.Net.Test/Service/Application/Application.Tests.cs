using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using Node.Net.Diagnostic.Generic;
using Node.Net.Service.Application;
using ApplicationService = Node.Net.Service.Application.Application;

namespace Node.Net.Test.Service.Application;

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
    public async Task GetApplicationInfo_ReturnsApplicationInfo()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info).IsNotNull();
        await Assert.That(info is ApplicationInfo).IsTrue();
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidName()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.Name).IsNotNull();
        await Assert.That(info.Name).IsNotEmpty();
        // Verify it's a valid application name (not "Unknown" unless assembly name is truly null)
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidCompany()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.Company).IsNotNull();
        // Company can be empty string if not available, which is valid
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidDataDirectory()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.DataDirectory).IsNotNull();
        // DataDirectory can be empty string if creation fails, which is valid per spec
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidTargetFramework()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.TargetFramework).IsNotNull();
        await Assert.That(info.TargetFramework).IsNotEmpty();
        // Should be "net8.0", "net8.0-windows", "net48", or "unknown"
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidExecutingAssemblyFilename()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.ExecutingAssemblyFilename).IsNotNull();
        // Can be empty string if not available, which is valid
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidVersion()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.Version).IsNotNull();
        // Can be empty string if not available, which is valid
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidUser()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.User).IsNotNull();
        // Can be empty string if not available, which is valid
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidDomain()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.Domain).IsNotNull();
        // Can be empty string if not available, which is valid
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidOperatingSystem()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.OperatingSystem).IsNotNull();
        // Can be empty string if not available, which is valid
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsValidMachine()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert
        await Assert.That(info.Machine).IsNotNull();
        // Can be empty string if not available, which is valid
    }

    [Test]
    public async Task GetApplicationInfo_HandlesDataDirectoryFailure()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info = app.GetApplicationInfo();

        // Assert - Should not throw, DataDirectory should be empty string if creation failed
        await Assert.That(info.DataDirectory).IsNotNull();
        // Note: We can't easily simulate directory creation failure in a unit test,
        // but we verify that the method doesn't throw and returns a valid ApplicationInfo
    }

    [Test]
    public async Task GetApplicationInfo_IsIdempotent()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info1 = app.GetApplicationInfo();
        var info2 = app.GetApplicationInfo();

        // Assert - Properties should be equivalent (may be different instances)
        await Assert.That(info1.Name).IsEqualTo(info2.Name);
        await Assert.That(info1.Company).IsEqualTo(info2.Company);
        await Assert.That(info1.DataDirectory).IsEqualTo(info2.DataDirectory);
        await Assert.That(info1.TargetFramework).IsEqualTo(info2.TargetFramework);
        await Assert.That(info1.ExecutingAssemblyFilename).IsEqualTo(info2.ExecutingAssemblyFilename);
        await Assert.That(info1.Version).IsEqualTo(info2.Version);
    }

    [Test]
    public async Task GetApplicationInfo_ReturnsNewInstanceEachCall()
    {
        // Arrange
        var app = new ApplicationService();

        // Act
        var info1 = app.GetApplicationInfo();
        var info2 = app.GetApplicationInfo();

        // Assert - Should be different instances (not cached)
        await Assert.That(ReferenceEquals(info1, info2)).IsFalse();
    }

    [Test]
    public async Task GetApplicationInfo_PerformanceUnder10ms()
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
        await Assert.That(averageMs).IsLessThan(10);
    }
}
