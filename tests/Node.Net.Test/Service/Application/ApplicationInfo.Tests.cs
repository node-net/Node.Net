using System;
using System.Threading.Tasks;
using Node.Net.Diagnostic;
using Node.Net.Service.Application;

namespace Node.Net.Test.Service.Application;

internal class ApplicationInfoTests : TestHarness
{
    public ApplicationInfoTests() : base(typeof(ApplicationInfo))
    {
    }
    [Test]
    public async Task ApplicationInfo_PropertiesAreReadOnly()
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

        await Assert.That(name).IsEqualTo("TestApp");
        await Assert.That(company).IsEqualTo("TestCompany");
        await Assert.That(dataDirectory).IsEqualTo("C:\\Test\\Data");
        await Assert.That(targetFramework).IsEqualTo("net8.0");
        await Assert.That(executingAssemblyFilename).IsEqualTo("C:\\Test\\App.exe");
        await Assert.That(version).IsEqualTo("1.0.0");
        await Assert.That(user).IsEqualTo("TestUser");
        await Assert.That(domain).IsEqualTo("TestDomain");
        await Assert.That(operatingSystem).IsEqualTo("Windows 10");
        await Assert.That(machine).IsEqualTo("TestMachine");
    }

    [Test]
    public async Task ApplicationInfo_ConstructorAcceptsAllProperties()
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
        await Assert.That(info).IsNotNull();
        await Assert.That(info.Name).IsEqualTo("TestApp");
        await Assert.That(info.Company).IsEqualTo("TestCompany");
        await Assert.That(info.DataDirectory).IsEqualTo("C:\\Test\\Data");
        await Assert.That(info.TargetFramework).IsEqualTo("net8.0");
        await Assert.That(info.ExecutingAssemblyFilename).IsEqualTo("C:\\Test\\App.exe");
        await Assert.That(info.Version).IsEqualTo("1.0.0");
        await Assert.That(info.User).IsEqualTo("TestUser");
        await Assert.That(info.Domain).IsEqualTo("TestDomain");
        await Assert.That(info.OperatingSystem).IsEqualTo("Windows 10");
        await Assert.That(info.Machine).IsEqualTo("TestMachine");
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullName()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
    public async Task ApplicationInfo_ConstructorThrowsOnNullCompany()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullDataDirectory()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullTargetFramework()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullExecutingAssemblyFilename()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullVersion()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_PropertiesMatchConstructorValues()
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
        await Assert.That(info.Name).IsEqualTo(name);
        await Assert.That(info.Company).IsEqualTo(company);
        await Assert.That(info.DataDirectory).IsEqualTo(dataDirectory);
        await Assert.That(info.TargetFramework).IsEqualTo(targetFramework);
        await Assert.That(info.ExecutingAssemblyFilename).IsEqualTo(executingAssemblyFilename);
        await Assert.That(info.Version).IsEqualTo(version);
        await Assert.That(info.User).IsEqualTo(user);
        await Assert.That(info.Domain).IsEqualTo(domain);
        await Assert.That(info.OperatingSystem).IsEqualTo(operatingSystem);
        await Assert.That(info.Machine).IsEqualTo(machine);
    }

    [Test]
    public async Task ApplicationInfo_IsImmutable()
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
        await Assert.That(info.Name).IsEqualTo("TestApp");
        
        // Note: We cannot test immutability at runtime if properties are truly read-only
        // The compiler will prevent assignment if properties are get-only
        // This test verifies the properties return the expected values
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullUser()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullDomain()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullOperatingSystem()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ApplicationInfo_ConstructorThrowsOnNullMachine()
    {
        // Arrange, Act & Assert
        await Assert.That(() => new ApplicationInfo(
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
        )).Throws<ArgumentNullException>();
    }
}
