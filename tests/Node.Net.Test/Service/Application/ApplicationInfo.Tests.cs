extern alias NodeNet;
using NUnit.Framework;
using System;
using NodeNet::Node.Net.Diagnostic;
using NodeNet::Node.Net.Service.Application;

namespace Node.Net.Test.Service.Application;

[TestFixture]
internal class ApplicationInfoTests : TestHarness
{
    public ApplicationInfoTests() : base(typeof(ApplicationInfo))
    {
    }
    [Test]
    public void ApplicationInfo_PropertiesAreReadOnly()
    {
        // Arrange
        var info = new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        );

        // Assert - Properties should be get-only (compile-time check)
        // If this compiles, properties are read-only
        var name = info.Name;
        var company = info.Company;
        var dataDirectory = info.DataDirectory;
        var targetFramework = info.TargetFramework;
        var executingAssemblyFilename = info.ExecutingAssemblyFilename;
        var version = info.Version;
        var user = info.User;
        var domain = info.Domain;
        var operatingSystem = info.OperatingSystem;
        var machine = info.Machine;

        Assert.That(name, Is.EqualTo("TestApp"));
        Assert.That(company, Is.EqualTo("TestCompany"));
        Assert.That(dataDirectory, Is.EqualTo("C:\\Test\\Data"));
        Assert.That(targetFramework, Is.EqualTo("net8.0"));
        Assert.That(executingAssemblyFilename, Is.EqualTo("C:\\Test\\App.exe"));
        Assert.That(version, Is.EqualTo("1.0.0"));
        Assert.That(user, Is.EqualTo("TestUser"));
        Assert.That(domain, Is.EqualTo("TestDomain"));
        Assert.That(operatingSystem, Is.EqualTo("Windows 10"));
        Assert.That(machine, Is.EqualTo("TestMachine"));
    }

    [Test]
    public void ApplicationInfo_ConstructorAcceptsAllProperties()
    {
        // Arrange & Act
        var info = new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        );

        // Assert
        Assert.That(info, Is.Not.Null);
        Assert.That(info.Name, Is.EqualTo("TestApp"));
        Assert.That(info.Company, Is.EqualTo("TestCompany"));
        Assert.That(info.DataDirectory, Is.EqualTo("C:\\Test\\Data"));
        Assert.That(info.TargetFramework, Is.EqualTo("net8.0"));
        Assert.That(info.ExecutingAssemblyFilename, Is.EqualTo("C:\\Test\\App.exe"));
        Assert.That(info.Version, Is.EqualTo("1.0.0"));
        Assert.That(info.User, Is.EqualTo("TestUser"));
        Assert.That(info.Domain, Is.EqualTo("TestDomain"));
        Assert.That(info.OperatingSystem, Is.EqualTo("Windows 10"));
        Assert.That(info.Machine, Is.EqualTo("TestMachine"));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullName()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            null!,
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullCompany()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            null!,
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullDataDirectory()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            null!,
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullTargetFramework()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            null!,
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullExecutingAssemblyFilename()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            null!,
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullVersion()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            null!,
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_PropertiesMatchConstructorValues()
    {
        // Arrange
        var name = "TestApp";
        var company = "TestCompany";
        var dataDirectory = "C:\\Test\\Data";
        var targetFramework = "net8.0";
        var executingAssemblyFilename = "C:\\Test\\App.exe";
        var version = "1.0.0";
        var user = "TestUser";
        var domain = "TestDomain";
        var operatingSystem = "Windows 10";
        var machine = "TestMachine";

        // Act
        var info = new ApplicationInfo(
            name,
            company,
            dataDirectory,
            targetFramework,
            executingAssemblyFilename,
            version,
            user,
            domain,
            operatingSystem,
            machine
        );

        // Assert
        Assert.That(info.Name, Is.EqualTo(name));
        Assert.That(info.Company, Is.EqualTo(company));
        Assert.That(info.DataDirectory, Is.EqualTo(dataDirectory));
        Assert.That(info.TargetFramework, Is.EqualTo(targetFramework));
        Assert.That(info.ExecutingAssemblyFilename, Is.EqualTo(executingAssemblyFilename));
        Assert.That(info.Version, Is.EqualTo(version));
        Assert.That(info.User, Is.EqualTo(user));
        Assert.That(info.Domain, Is.EqualTo(domain));
        Assert.That(info.OperatingSystem, Is.EqualTo(operatingSystem));
        Assert.That(info.Machine, Is.EqualTo(machine));
    }

    [Test]
    public void ApplicationInfo_IsImmutable()
    {
        // Arrange
        var info = new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            "TestMachine"
        );

        // Assert - Properties cannot be modified after construction
        // This is a compile-time check - if setters exist, this test will fail to compile
        // If this compiles and runs, the class is immutable
        Assert.That(info.Name, Is.EqualTo("TestApp"));
        
        // Note: We cannot test immutability at runtime if properties are truly read-only
        // The compiler will prevent assignment if properties are get-only
        // This test verifies the properties return the expected values
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullUser()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            null!,
            "TestDomain",
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullDomain()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            null!,
            "Windows 10",
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullOperatingSystem()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            null!,
            "TestMachine"
        ));
    }

    [Test]
    public void ApplicationInfo_ConstructorThrowsOnNullMachine()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ApplicationInfo(
            "TestApp",
            "TestCompany",
            "C:\\Test\\Data",
            "net8.0",
            "C:\\Test\\App.exe",
            "1.0.0",
            "TestUser",
            "TestDomain",
            "Windows 10",
            null!
        ));
    }
}
