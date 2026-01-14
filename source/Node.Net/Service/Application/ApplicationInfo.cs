using System;

namespace Node.Net.Service.Application;

/// <summary>
/// Represents a snapshot of application metadata.
/// This class is immutable.
/// </summary>
public class ApplicationInfo
{
    /// <summary>
    /// Gets the application name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the company name.
    /// </summary>
    public string Company { get; }

    /// <summary>
    /// Gets the full path to the application data directory.
    /// </summary>
    public string DataDirectory { get; }

    /// <summary>
    /// Gets the target framework moniker (TFM) for the application.
    /// </summary>
    public string TargetFramework { get; }

    /// <summary>
    /// Gets the full filename (path) of the executing assembly.
    /// </summary>
    public string ExecutingAssemblyFilename { get; }

    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets the current user name.
    /// </summary>
    public string User { get; }

    /// <summary>
    /// Gets the current user domain.
    /// </summary>
    public string Domain { get; }

    /// <summary>
    /// Gets the operating system name.
    /// </summary>
    public string OperatingSystem { get; }

    /// <summary>
    /// Gets the machine name.
    /// </summary>
    public string Machine { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationInfo"/> class.
    /// </summary>
    /// <param name="name">The application name.</param>
    /// <param name="company">The company name.</param>
    /// <param name="dataDirectory">The application data directory path.</param>
    /// <param name="targetFramework">The target framework moniker.</param>
    /// <param name="executingAssemblyFilename">The full filename of the executing assembly.</param>
    /// <param name="version">The application version.</param>
    /// <param name="user">The current user name.</param>
    /// <param name="domain">The current user domain.</param>
    /// <param name="operatingSystem">The operating system name.</param>
    /// <param name="machine">The machine name.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public ApplicationInfo(string name, string company, string dataDirectory, string targetFramework, string executingAssemblyFilename, string version, string user, string domain, string operatingSystem, string machine)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Company = company ?? throw new ArgumentNullException(nameof(company));
        DataDirectory = dataDirectory ?? throw new ArgumentNullException(nameof(dataDirectory));
        TargetFramework = targetFramework ?? throw new ArgumentNullException(nameof(targetFramework));
        ExecutingAssemblyFilename = executingAssemblyFilename ?? throw new ArgumentNullException(nameof(executingAssemblyFilename));
        Version = version ?? throw new ArgumentNullException(nameof(version));
        User = user ?? throw new ArgumentNullException(nameof(user));
        Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
        Machine = machine ?? throw new ArgumentNullException(nameof(machine));
    }
}
