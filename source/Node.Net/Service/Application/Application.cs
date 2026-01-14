using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace Node.Net.Service.Application;

/// <summary>
/// Concrete implementation of IApplication that uses reflection to read executing assembly metadata
/// and determine appropriate data directory location.
/// </summary>
public class Application : IApplication
{
    private readonly Assembly _assembly;
    private string? _cachedName;
    private string? _cachedCompany;
    private string? _cachedTargetFramework;
    private string? _cachedExecutingAssemblyFilename;
    private string? _cachedVersion;

    /// <summary>
    /// Initializes a new instance of the Application class
    /// </summary>
    public Application()
    {
        _assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
    }

    /// <summary>
    /// Gets all application information in a single call.
    /// </summary>
    /// <returns>An <see cref="ApplicationInfo"/> instance containing all application metadata.</returns>
    public ApplicationInfo GetApplicationInfo()
    {
        // Use cached values or compute them
        string name = GetNameInternal();
        string company = GetCompanyInternal();
        string dataDirectory = GetDataDirectoryInternal(name, company);
        string targetFramework = GetTargetFrameworkInternal();
        string executingAssemblyFilename = GetExecutingAssemblyFilenameInternal();
        string version = GetVersionInternal();
        string user = GetUserInternal();
        string domain = GetDomainInternal();
        string operatingSystem = GetOperatingSystemInternal();
        string machine = GetMachineInternal();

        return new ApplicationInfo(
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
    }

    private string GetNameInternal()
    {
        if (_cachedName != null)
        {
            return _cachedName;
        }

        var titleAttribute = _assembly.GetCustomAttribute<AssemblyTitleAttribute>();
        if (titleAttribute != null && !string.IsNullOrEmpty(titleAttribute.Title))
        {
            _cachedName = titleAttribute.Title;
            return _cachedName;
        }

        var assemblyName = _assembly.GetName().Name;
        _cachedName = assemblyName ?? "Unknown";
        return _cachedName;
    }

    private string GetCompanyInternal()
    {
        if (_cachedCompany != null)
        {
            return _cachedCompany;
        }

        var companyAttribute = _assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        _cachedCompany = companyAttribute?.Company ?? string.Empty;
        return _cachedCompany;
    }

    private string GetDataDirectoryInternal(string appName, string companyName)
    {
        var appDataBase = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // Ensure company and appName are valid for pathing
        var safeCompanyName = string.IsNullOrEmpty(companyName) ? "UnknownCompany" : SanitizePathPart(companyName);
        var safeAppName = string.IsNullOrEmpty(appName) ? "UnknownApplication" : SanitizePathPart(appName);

        var appDataPath = Path.Combine(appDataBase, safeCompanyName, safeAppName);

        try
        {
            Directory.CreateDirectory(appDataPath);
            return appDataPath;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.Error.WriteLine($"Failed to create application data directory due to permissions: {appDataPath}. Error: {ex.Message}");
            return string.Empty; // Return empty string as per clarification
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.Error.WriteLine($"Failed to create application data directory due to invalid path: {appDataPath}. Error: {ex.Message}");
            return string.Empty; // Return empty string as per clarification
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine($"An I/O error occurred while creating application data directory: {appDataPath}. Error: {ex.Message}");
            return string.Empty; // Return empty string as per clarification
        }
    }

    private string GetTargetFrameworkInternal()
    {
        if (_cachedTargetFramework != null)
        {
            return _cachedTargetFramework;
        }

        try
        {
            var assembly = _assembly;

            // Method 1: Try to extract from assembly location path (most reliable)
            var assemblyLocation = assembly.Location;
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                if (!string.IsNullOrEmpty(assemblyDir))
                {
                    var dirName = Path.GetFileName(assemblyDir);
                    if (dirName != null && dirName.StartsWith("net", StringComparison.OrdinalIgnoreCase))
                    {
                        _cachedTargetFramework = dirName;
                        return _cachedTargetFramework;
                    }

                    // Check parent directory
                    var parentDir = Path.GetDirectoryName(assemblyDir);
                    if (!string.IsNullOrEmpty(parentDir))
                    {
                        var parentDirName = Path.GetFileName(parentDir);
                        if (parentDirName != null && parentDirName.StartsWith("net", StringComparison.OrdinalIgnoreCase))
                        {
                            _cachedTargetFramework = parentDirName;
                            return _cachedTargetFramework;
                        }
                    }
                }
            }

            // Method 2: Use TargetFrameworkAttribute
            var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
            if (targetFrameworkAttribute != null && !string.IsNullOrEmpty(targetFrameworkAttribute.FrameworkName))
            {
                var frameworkName = targetFrameworkAttribute.FrameworkName;

                if (frameworkName.Contains(".NETCoreApp"))
                {
                    var versionMatch = Regex.Match(frameworkName, @"Version=v(\d+)\.(\d+)");
                    if (versionMatch.Success)
                    {
                        var major = int.Parse(versionMatch.Groups[1].Value);
                        var minor = int.Parse(versionMatch.Groups[2].Value);

                        if (major >= 8)
                        {
                            try
                            {
                                var windowsBaseType = Type.GetType("System.Windows.Application, WindowsBase, PresentationFramework");
                                if (windowsBaseType != null)
                                {
                                    _cachedTargetFramework = $"net{major}.{minor}-windows";
                                    return _cachedTargetFramework;
                                }
                            }
                            catch { /* Ignore */ }
                            _cachedTargetFramework = $"net{major}.{minor}";
                            return _cachedTargetFramework;
                        }
                        _cachedTargetFramework = $"net{major}.{minor}";
                        return _cachedTargetFramework;
                    }
                }
                else if (frameworkName.Contains(".NETFramework"))
                {
                    var versionMatch = Regex.Match(frameworkName, @"Version=v(\d+)\.(\d+)");
                    if (versionMatch.Success)
                    {
                        var major = int.Parse(versionMatch.Groups[1].Value);
                        var minor = int.Parse(versionMatch.Groups[2].Value);
                        _cachedTargetFramework = $"net{major}{minor}";
                        return _cachedTargetFramework;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error determining target framework: {ex.Message}");
        }
        _cachedTargetFramework = "unknown";
        return _cachedTargetFramework;
    }

    private string GetExecutingAssemblyFilenameInternal()
    {
        if (_cachedExecutingAssemblyFilename != null)
        {
            return _cachedExecutingAssemblyFilename;
        }

        try
        {
            _cachedExecutingAssemblyFilename = _assembly.Location;
            if (string.IsNullOrEmpty(_cachedExecutingAssemblyFilename))
            {
                _cachedExecutingAssemblyFilename = string.Empty;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error getting executing assembly filename: {ex.Message}");
            _cachedExecutingAssemblyFilename = string.Empty;
        }
        return _cachedExecutingAssemblyFilename;
    }

    private string GetVersionInternal()
    {
        if (_cachedVersion != null)
        {
            return _cachedVersion;
        }

        try
        {
            var informationalVersionAttribute = _assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (informationalVersionAttribute != null && !string.IsNullOrEmpty(informationalVersionAttribute.InformationalVersion))
            {
                _cachedVersion = informationalVersionAttribute.InformationalVersion;
                return _cachedVersion;
            }

            var version = _assembly.GetName().Version;
            if (version != null)
            {
                _cachedVersion = version.ToString();
                return _cachedVersion;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error getting assembly version: {ex.Message}");
        }
        _cachedVersion = string.Empty;
        return _cachedVersion;
    }

    private string GetUserInternal()
    {
        try
        {
            return Environment.UserName;
        }
        catch
        {
            return string.Empty;
        }
    }

    private string GetDomainInternal()
    {
        try
        {
            return Environment.UserDomainName;
        }
        catch
        {
            return string.Empty;
        }
    }

    private string GetOperatingSystemInternal()
    {
        try
        {
            return Environment.OSVersion.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }

    private string GetMachineInternal()
    {
        try
        {
            return Environment.MachineName;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static string SanitizePathPart(string part)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            part = part.Replace(c, '_');
        }
        foreach (char c in Path.GetInvalidPathChars())
        {
            part = part.Replace(c, '_');
        }
        return part;
    }
}
